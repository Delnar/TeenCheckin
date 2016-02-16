namespace TeenCheckin
{
    partial class frmException
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
            this.lstExceptionList = new System.Windows.Forms.ListBox();
            this.butOK = new System.Windows.Forms.Button();
            this.txtStacktrace = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.butExport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstExceptionList
            // 
            this.lstExceptionList.FormattingEnabled = true;
            this.lstExceptionList.Location = new System.Drawing.Point(12, 30);
            this.lstExceptionList.Name = "lstExceptionList";
            this.lstExceptionList.Size = new System.Drawing.Size(146, 277);
            this.lstExceptionList.TabIndex = 0;
            this.lstExceptionList.SelectedIndexChanged += new System.EventHandler(this.lstExceptionList_SelectedIndexChanged);
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(738, 317);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 23);
            this.butOK.TabIndex = 4;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // txtStacktrace
            // 
            this.txtStacktrace.Location = new System.Drawing.Point(165, 30);
            this.txtStacktrace.Multiline = true;
            this.txtStacktrace.Name = "txtStacktrace";
            this.txtStacktrace.Size = new System.Drawing.Size(649, 167);
            this.txtStacktrace.TabIndex = 2;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(165, 216);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(649, 93);
            this.txtMessage.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(165, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Stack Trace";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(165, 200);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Message";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Exception List";
            // 
            // butExport
            // 
            this.butExport.Location = new System.Drawing.Point(652, 317);
            this.butExport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.butExport.Name = "butExport";
            this.butExport.Size = new System.Drawing.Size(77, 22);
            this.butExport.TabIndex = 8;
            this.butExport.Text = "Export";
            this.butExport.UseVisualStyleBackColor = true;
            this.butExport.Click += new System.EventHandler(this.butExport_Click);
            // 
            // frmException
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 357);
            this.Controls.Add(this.butExport);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.txtStacktrace);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.lstExceptionList);
            this.Name = "frmException";
            this.Text = "Exception Thrown";
            this.Load += new System.EventHandler(this.frmException_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstExceptionList;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.TextBox txtStacktrace;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button butExport;
    }
}