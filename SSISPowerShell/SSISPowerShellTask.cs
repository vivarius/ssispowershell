using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using DTSExecResult = Microsoft.SqlServer.Dts.Runtime.DTSExecResult;
using VariableDispenser = Microsoft.SqlServer.Dts.Runtime.VariableDispenser;

namespace SSISPowerShellTask110.SSIS
{
    [DtsTask(
        DisplayName = "PowerShell Task",
        UITypeName = "SSISPowerShellTask110.SSISPowerShellTaskUIInterface,SSISPowerShellTask110," +
        "Version=1.0.0.30," +
        "Culture=Neutral," +
        "PublicKeyToken=e5d3d163e34f88b1",
        IconResource = "SSISPowerShellTask110.powershell.ico",
        TaskContact = "cosmin.vlasiu@gmail.com"
        )]
    public class SSISPowerShellTask : Task, IDTSComponentPersist
    {
        #region Constructor
        public SSISPowerShellTask()
        {
        }

        #endregion

        #region Public Properties

        [Category("PowerShell"), Description("PowerShellScript: Input PowerShell script")]
        public string PowerShellScript { get; set; }
        [Category("PowerShell"), Description("Output Variable Name")]
        public string OutputVariableName { get; set; }
        #endregion

        #region Private Properties

        Variables _vars = null;

        #endregion

        #region Validate

        /// <summary>
        /// Validate local parameters
        /// </summary>
        public override DTSExecResult Validate(Connections connections, VariableDispenser variableDispenser, IDTSComponentEvents componentEvents, IDTSLogging log)
        {
            bool isBaseValid = true;

            if (base.Validate(connections, variableDispenser, componentEvents, log) != DTSExecResult.Success)
            {
                componentEvents.FireError(0, "SSISPowerShellTask", "Base validation failed", "", 0);
                isBaseValid = false;
            }

            if (string.IsNullOrEmpty(PowerShellScript))
            {
                componentEvents.FireError(0, "SSISPowerShellTask", "Input PowerShell script is missing.", "", 0);
                isBaseValid = false;
            }
            /*else if (string.IsNullOrEmpty(OutputVariableName))
            {
                componentEvents.FireError(0, "SSISPowerShellTask", "Output variable specified.", "", 0);
                isBaseValid = false;
            }*/

            return isBaseValid ? DTSExecResult.Success : DTSExecResult.Failure;
        }

        #endregion

        #region Execute

        /// <summary>
        /// This method is a run-time method executed dtsexec.exe
        /// </summary>
        /// <param name="connections"></param>
        /// <param name="variableDispenser"></param>
        /// <param name="componentEvents"></param>
        /// <param name="log"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public override DTSExecResult Execute(Connections connections, VariableDispenser variableDispenser, IDTSComponentEvents componentEvents, IDTSLogging log, object transaction)
        {
            Executing(variableDispenser, componentEvents);

            return base.Execute(connections, variableDispenser, componentEvents, log, transaction);
        }

        private void Executing(VariableDispenser variableDispenser, IDTSComponentEvents componentEvents)
        {
            bool refire = false;

            try
            {
                PowerShellCoreScripting.GetNeededVariables(variableDispenser, ref _vars, componentEvents, this);

                componentEvents.FireInformation(0, "SSISPowerShell", String.Format("Launching PowerShell Script: {0} ", EvaluateExpression(PowerShellScript, variableDispenser)), string.Empty, 0, ref refire);

                var futureDevis = System.Threading.Tasks.Task.Factory.StartNew(() => PowerShellCoreScripting.RunScript(PowerShellScript, variableDispenser));
                System.Threading.Tasks.Task.WaitAll(futureDevis);
                string retVal = futureDevis.Result.Trim();

                componentEvents.FireInformation(0, "SSISPowerShell", String.Format("Execution of PowerShell Script succeeded"), string.Empty, 0, ref refire);

                componentEvents.FireInformation(0, "SSISPowerShell", string.Format("The result: {0}", retVal), string.Empty, 0, ref refire);

                componentEvents.FireInformation(0, "SSISPowerShell", string.Format("Attach result to the output variable: {0}", OutputVariableName), string.Empty, 0, ref refire);
                string output = PowerShellCoreScripting.GetVariableFromNamespaceContext(OutputVariableName).Trim();

                try
                {
                    _vars[output].Value = retVal;
                }
                catch (Exception exception)
                {
                    componentEvents.FireError(0, "SSISPowerShell", string.Format("Error : {0}", exception.Message), "", 0);
                }
            }
            catch (Exception ex)
            {
                componentEvents.FireError(0, "SSISPowerShell", string.Format("Error : {0}", ex.Message), "", 0);
            }
            finally
            {
                if (_vars != null)
                    if (_vars.Locked != null)
                        if (_vars.Locked)
                            _vars.Unlock();
            }
        }

        #endregion

        #region Methods



        /// <summary>
        /// This method evaluate expressions like @([System::TaskName] + [System::TaskID]) or any other operation created using 
        /// ExpressionBuilder
        /// </summary>
        /// <param name="mappedParam"></param>
        /// <param name="variableDispenser"></param>
        /// <returns></returns>
        private static object EvaluateExpression(string mappedParam, VariableDispenser variableDispenser)
        {
            object variableObject;

            try
            {
                var expressionEvaluatorClass = new ExpressionEvaluator
                {
                    Expression = mappedParam
                };

                expressionEvaluatorClass.Evaluate(DtsConvert.GetExtendedInterface(variableDispenser), out variableObject, false);
            }
            catch (Exception) // for already initialized values
            {
                variableObject = mappedParam;
            }

            return variableObject;
        }

        #endregion

        #region Implementation of IDTSComponentPersist

        void IDTSComponentPersist.SaveToXML(XmlDocument doc, IDTSInfoEvents infoEvents)
        {
            try
            {
                XmlElement taskElement = doc.CreateElement(string.Empty, "SSISPowerShell", string.Empty);

                XmlAttribute powerShellScript = doc.CreateAttribute(string.Empty, Keys.PowerShellScript, string.Empty);
                powerShellScript.Value = PowerShellScript;

                XmlAttribute outputVariableName = doc.CreateAttribute(string.Empty, Keys.OutputVariableName, string.Empty);
                outputVariableName.Value = OutputVariableName;


                taskElement.Attributes.Append(powerShellScript);
                taskElement.Attributes.Append(outputVariableName);

                doc.AppendChild(taskElement);
            }
            catch (Exception exception)
            {
                MessageBox.Show("There is an error trying to save components properties: " + exception.Message);
            }

        }

        void IDTSComponentPersist.LoadFromXML(XmlElement node, IDTSInfoEvents infoEvents)
        {
            if (node.Name != "SSISPowerShell")
            {
                throw new Exception("Unexpected task element when loading task.");
            }

            try
            {
                PowerShellScript = node.Attributes.GetNamedItem(Keys.PowerShellScript).Value;
                OutputVariableName = node.Attributes.GetNamedItem(Keys.OutputVariableName).Value;
            }
            catch (Exception exception)
            {
                MessageBox.Show("The component is corrupted! Impossible to load the properties: " + exception.Message);
            }
        }

        #endregion
    }
}

