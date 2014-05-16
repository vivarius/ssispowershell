namespace SSISPowerShellTask110
{
    partial class frmEditProperties
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditProperties));
            this.btSave = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btExpressionPath = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbOut = new System.Windows.Forms.ComboBox();
            this.textBoxScript = new System.Windows.Forms.TextBox();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lstVariables = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(299, 442);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 36);
            this.btSave.TabIndex = 6;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(380, 442);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 36);
            this.btCancel.TabIndex = 7;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Script:";
            // 
            // btExpressionPath
            // 
            this.btExpressionPath.Location = new System.Drawing.Point(336, 109);
            this.btExpressionPath.Name = "btExpressionPath";
            this.btExpressionPath.Size = new System.Drawing.Size(75, 36);
            this.btExpressionPath.TabIndex = 17;
            this.btExpressionPath.Text = "Expression";
            this.btExpressionPath.UseVisualStyleBackColor = true;
            this.btExpressionPath.Visible = false;
            this.btExpressionPath.Click += new System.EventHandler(this.btExpressionPath_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 419);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Output variable name:";
            // 
            // cmbOut
            // 
            this.cmbOut.FormattingEnabled = true;
            this.cmbOut.Location = new System.Drawing.Point(126, 411);
            this.cmbOut.Name = "cmbOut";
            this.cmbOut.Size = new System.Drawing.Size(329, 21);
            this.cmbOut.TabIndex = 22;
            // 
            // textBoxScript
            // 
            this.textBoxScript.AllowDrop = true;
            this.textBoxScript.Location = new System.Drawing.Point(12, 37);
            this.textBoxScript.MaxLength = 0;
            this.textBoxScript.Multiline = true;
            this.textBoxScript.Name = "textBoxScript";
            this.textBoxScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxScript.Size = new System.Drawing.Size(443, 210);
            this.textBoxScript.TabIndex = 23;
            this.textBoxScript.Text = "\"Get-Process\"";
            this.textBoxScript.WordWrap = false;
            this.textBoxScript.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxScript_DragDrop);
            this.textBoxScript.DragOver += new System.Windows.Forms.DragEventHandler(this.textBoxScript_DragOver);
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxOutput.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBoxOutput.Location = new System.Drawing.Point(12, 288);
            this.textBoxOutput.MaxLength = 0;
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxOutput.Size = new System.Drawing.Size(443, 117);
            this.textBoxOutput.TabIndex = 24;
            this.textBoxOutput.WordWrap = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(380, 249);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 33);
            this.button1.TabIndex = 25;
            this.button1.Text = "Run Script";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lstVariables
            // 
            this.lstVariables.FormattingEnabled = true;
            this.lstVariables.Location = new System.Drawing.Point(461, 37);
            this.lstVariables.Name = "lstVariables";
            this.lstVariables.Size = new System.Drawing.Size(172, 446);
            this.lstVariables.TabIndex = 26;
            this.lstVariables.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstVariables_DragDrop);
            this.lstVariables.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstVariables_DragEnter);
            this.lstVariables.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstVariables_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(458, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(168, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Variables (drag and drop variable )";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 269);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Execution result:";
            // 
            // frmEditProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(645, 493);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstVariables);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.textBoxScript);
            this.Controls.Add(this.cmbOut);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btExpressionPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEditProperties";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit task properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btExpressionPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbOut;
        private System.Windows.Forms.TextBox textBoxScript;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lstVariables;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
    }
}