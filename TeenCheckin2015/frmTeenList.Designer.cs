namespace TeenCheckin
{
    partial class frmTeenList
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
            this.lbTeenList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbTeenList
            // 
            this.lbTeenList.FormattingEnabled = true;
            this.lbTeenList.Location = new System.Drawing.Point(13, 13);
            this.lbTeenList.Name = "lbTeenList";
            this.lbTeenList.Size = new System.Drawing.Size(506, 251);
            this.lbTeenList.TabIndex = 0;
            this.lbTeenList.DoubleClick += new System.EventHandler(this.lbTeenList_DoubleClick);
            this.lbTeenList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lbTeenList_KeyUp);
            // 
            // frmTeenList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 279);
            this.Controls.Add(this.lbTeenList);
            this.Name = "frmTeenList";
            this.Text = "Select A Teen Record";
            this.Load += new System.EventHandler(this.frmTeenList_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmTeenList_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox lbTeenList;

    }
}