using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace TeenCheckin
{
    public partial class frmSelectTeens : Form
    {
        public string txtTitle = "";
        public bool MultiSelect = false;
        List<TeenProxy.wcfTeens.Teen> lstAllTeens = new List<TeenProxy.wcfTeens.Teen>();

        public frmSelectTeens(string pTitle, bool pMultiSelect)
        {
            txtTitle = pTitle;
            MultiSelect = pMultiSelect;
            InitializeComponent();
        }

        private void frmSelectTeens_Load(object sender, EventArgs e)
        {
            this.Text = txtTitle;
            if(MultiSelect == true)
                lstTeens.SelectionMode = SelectionMode.MultiSimple;
            else
                lstTeens.SelectionMode = SelectionMode.One;
            lstAllTeens = Database.GetAllTeens();
            FillTeenList();
        }

        public void FillTeenList()
        {
            ListItem[] lArray = GetListOfTeens();

            lstTeens.Items.Clear();
            if (lArray == null)
            {
                butLoad.Enabled = false;
                return;
            }

            butLoad.Enabled = true;
            foreach (ListItem li in lArray)
            {
                lstTeens.Items.Add(li);
            }             
        }

        public ListItem[] GetListOfTeens()
        {
            List<ListItem> ret = new List<ListItem>();
            var teenList = lstAllTeens.Where(x => x.Title.ToUpper().StartsWith(txtFilter.Text.ToUpper().Trim())).ToList();
            teenList.ForEach(x =>
            {
                string Text = x.Title;
                int idx = x.id;
                ret.Add(new ListItem(idx, Text));
            });
            return ret.ToArray();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            FillTeenList();
        }

        private void butLoad_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void lstTeens_DoubleClick(object sender, EventArgs e)
        {
            if (lstTeens.SelectedIndex >= 0)
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
