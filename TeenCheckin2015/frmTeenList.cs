using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TeenCheckin
{
    public partial class frmTeenList : Form
    {
        public List<ListItem> lArray;
        public frmTeenList()
        {
            InitializeComponent();
        }

        private void frmTeenList_Load(object sender, EventArgs e)
        {
            if (lArray == null)
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }

            foreach (ListItem li in lArray)
            {
                lbTeenList.Items.Add(li);
            }

            lbTeenList.SelectedIndex = 0;
        }

        private void lbTeenList_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (lbTeenList.SelectedIndex < 0) return;
                    DialogResult = DialogResult.OK;
                    this.Close();
                    break;
            }
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            if ((e.Control == true) && (e.KeyCode == Keys.N))
            {
                lbTeenList.SelectedIndex = 0;
                DialogResult = DialogResult.OK;
            }
        }

        private void frmTeenList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            if ((e.Control == true) && (e.KeyCode == Keys.N))
            {
                lbTeenList.SelectedIndex = 0;
                DialogResult = DialogResult.OK;
            }
        }

        private void lbTeenList_DoubleClick(object sender, EventArgs e)
        {
            if (lbTeenList.SelectedIndex < 0) return;
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
