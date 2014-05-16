using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DataTransformationServices.Controls;
using Microsoft.SqlServer.Dts.Runtime;
using Task = System.Threading.Tasks.Task;


namespace SSISPowerShellTask110
{
    public partial class frmEditProperties : Form
    {
        #region Private Properties
        private readonly TaskHost _taskHost;
        private readonly Variables _variables;
        #endregion

        #region Public Properties
        private Variables Variables
        {
            get { return _taskHost.Variables; }
        }
        private VariableDispenser VariableDispenser
        {
            get { return _taskHost.VariableDispenser; }
        }
        #endregion

        #region .ctor
        public frmEditProperties(TaskHost taskHost)
        {
            InitializeComponent();

            _taskHost = taskHost;

            if (taskHost == null)
            {
                throw new ArgumentNullException("taskHost");
            }

            _variables = taskHost.Variables;

            try
            {
                InitializeForm();
                if (_taskHost.Properties[Keys.PowerShellScript].GetValue(_taskHost) != null)
                {
                    textBoxScript.Text = _taskHost.Properties[Keys.PowerShellScript].GetValue(_taskHost).ToString();
                }

                if (_taskHost.Properties[Keys.OutputVariableName].GetValue(_taskHost) != null)
                    cmbOut.SelectedIndex = GetSelectedComboBoxIndex(cmbOut, _taskHost.Properties[Keys.OutputVariableName].GetValue(_taskHost));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void InitializeForm()
        {
            cmbOut.Items.Clear();
            cmbOut.Items.AddRange(LoadUserVariables("System.String").ToArray());
            lstVariables.Items.AddRange(LoadAllVariables("System.String").ToArray());
        }

        #endregion

        #region Methods

        private static int GetSelectedComboBoxIndex(ComboBox comboBox, object value)
        {
            int retValue = -1;

            if (value == null)
                return retValue;

            if (string.IsNullOrEmpty(value.ToString()))
                return retValue;

            string strValue = value.ToString();

            if (comboBox.FindString(strValue) > -1)
            {
                retValue = comboBox.FindString(strValue);
            }
            else
            {
                comboBox.Items.Add(strValue);
                retValue = comboBox.FindString(strValue);
            }

            return retValue;
        }

        private List<string> LoadUserVariables(string parameterInfo)
        {
            return (from Variable var in _variables
                    where var.DataType == Type.GetTypeCode(Type.GetType(parameterInfo)) && var.Namespace.ToLower() == "user"
                    select string.Format("@[{0}::{1}]", var.Namespace, var.Name)).ToList();
        }

        private List<string> LoadAllVariables(string parameterInfo)
        {
            return (from Variable var in _variables
                    where var.DataType == Type.GetTypeCode(Type.GetType(parameterInfo))
                    select string.Format("@[{0}::{1}]", var.Namespace, var.Name)).ToList();
        }

        #endregion

        #region Events

        private void btSave_Click(object sender, EventArgs e)
        {
            _taskHost.Properties[Keys.PowerShellScript].SetValue(_taskHost, textBoxScript.Text);
            _taskHost.Properties[Keys.OutputVariableName].SetValue(_taskHost, cmbOut.Text);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btExpressionPath_Click(object sender, EventArgs e)
        {
            using (ExpressionBuilder expressionBuilder = ExpressionBuilder.Instantiate(_taskHost.Variables, _taskHost.VariableDispenser, typeof(string), textBoxScript.Text))
            {
                if (expressionBuilder.ShowDialog() == DialogResult.OK)
                {
                    textBoxScript.Text = expressionBuilder.Expression;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            textBoxOutput.Clear();

            try
            {
                var futureDevis = Task.Factory.StartNew(() => PowerShellCoreScripting.RunScript(textBoxScript.Text, VariableDispenser));
                Task.WaitAll(futureDevis);

                textBoxOutput.Text = futureDevis.Result.Trim();
            }
            catch (Exception error)
            {
                textBoxOutput.Text = String.Format("\r\nError in script : {0}\r\n", error.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void lstVariables_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void lstVariables_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void lstVariables_MouseDown(object sender, MouseEventArgs e)
        {
            if (lstVariables.Items.Count == 0)
                return;

            int index = lstVariables.IndexFromPoint(e.X, e.Y);
            string s = lstVariables.Items[index].ToString();
            DragDropEffects dde1 = DoDragDrop(s, DragDropEffects.Copy);

            if (dde1 == DragDropEffects.All)
            {
                lstVariables.Items.RemoveAt(lstVariables.IndexFromPoint(e.X, e.Y));
            }
        }

        private void textBoxScript_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void textBoxScript_DragDrop(object sender, DragEventArgs e)
        {
            TextBox textBox1 = sender as TextBox;
            Point newPoint = new Point(e.X, e.Y);
            newPoint = textBox1.PointToClient(newPoint);
            int index = textBox1.GetCharIndexFromPosition(newPoint);

            if (e.Data.GetDataPresent("System.String"))
            {
                object item = e.Data.GetData("System.String"); //lstVariables.Items[lstVariables.IndexFromPoint(newPoint)];

                if (textBox1.Text.Trim() == string.Empty)
                {
                    textBox1.Text = item.ToString();
                }
                else
                {
                    var text = textBox1.Text;
                    var lastCharPosition = textBox1.GetPositionFromCharIndex(index);
                    if (lastCharPosition.X < newPoint.X)
                    {
                        text += item.ToString();
                    }
                    else
                    {
                        text = text.Insert(index, item.ToString());
                    }

                    textBox1.Text = text;
                }
            }
        }
        #endregion
    }
}
