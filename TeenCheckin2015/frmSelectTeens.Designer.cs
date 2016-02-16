namespace TeenCheckin
{
    partial class frmSelectTeens
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
            this.lstTeens = new System.Windows.Forms.ListBox();
            this.butLoad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lstTeens
            // 
            this.lstTeens.FormattingEnabled = true;
            this.lstTeens.Location = new System.Drawing.Point(13, 13);
            this.lstTeens.Name = "lstTeens";
            this.lstTeens.Size = new System.Drawing.Size(442, 329);
            this.lstTeens.TabIndex = 0;
            this.lstTeens.DoubleClick += new System.EventHandler(this.lstTeens_DoubleClick);
            // 
            // butLoad
            // 
            this.butLoad.Location = new System.Drawing.Point(12, 392);
            this.butLoad.Name = "butLoad";
            this.butLoad.Size = new System.Drawing.Size(442, 23);
            this.butLoad.TabIndex = 1;
            this.butLoad.Text = "Load Teen";
            this.butLoad.UseVisualStyleBackColor = true;
            this.butLoad.Click += new System.EventHandler(this.butLoad_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 349);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Filter By Name";
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(16, 366);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(439, 20);
            this.txtFilter.TabIndex = 3;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // frmSelectTeens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 422);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.butLoad);
            this.Controls.Add(this.lstTeens);
            this.Name = "frmSelectTeens";
            this.Text = "frmSelectTeens";
            this.Load += new System.EventHandler(this.frmSelectTeens_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button butLoad;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ListBox lstTeens;
        public System.Windows.Forms.TextBox txtFilter;
    }
}