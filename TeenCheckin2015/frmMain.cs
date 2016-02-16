using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace TeenCheckin
{
    public partial class frmMain : Form
    {
        public List<TeenProxy.wcfTeens.Teen> lstAllTeens = new List<TeenProxy.wcfTeens.Teen>();
        public bool bIsClear = false;
        public bool bIsNew = false;
        public int RecordIdx = 0;
        

        public bool isClear
        {
            get { return bIsClear; }
            set
            {
                bIsClear = value;
                if (bIsClear == true)
                {
                    this.Text = "Teen Checkin:  Enter first and last name.";
                    SetControlsEnabledFlag(this.Controls, "NOTFORNEW", false);
                }
                else
                {
                    SetControlsEnabledFlag(this.Controls, "NOTFORNEW", true);
                }
            }
        }

        public bool isNew
        {
            get { return bIsNew; }
            set
            {
                bIsNew = value;
                if (bIsNew == true)
                    this.Text = "Teen Checkin:  New record.";
                else
                    this.Text = "Teen Checkin:  Edit Record.";
            }
        }

        public void SetControlsEnabledFlag(System.Windows.Forms.Control.ControlCollection cl, string LookupTag, bool Enabled)
        {
            foreach (Control c in cl)
            {
                if (c.Controls.Count > 0)
                    SetControlsEnabledFlag(c.Controls, LookupTag, Enabled);

                if (c.Tag == null) continue;
                if (c.Tag.ToString().Contains(LookupTag)) c.Enabled = Enabled;
            }
        }

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            string Server = "";
            ClickOnceParams prms = new ClickOnceParams();
            if (prms.dictParams.ContainsKey("SERV") == true) Server = prms.dictParams["SERV"];
            if (string.IsNullOrEmpty(Server))
            {
                MessageBox.Show("Server missing from parameters");
                this.Close();
                return;
            }

            TeenProxy.ClientProxy.TargetServer = Server;
            lblGuestList.TabStop = false;
            ClearForm();
            initializeNightlyRecordsToolStripMenuItem.Enabled = false;
            
            AutoPopulateAutoFill();            
            DoAllPopulation();            
        }

        public void AutoPopulateAutoFill()
        {
            var source = new AutoCompleteStringCollection();
            source.AddRange(Database.GetDistinctCityList().ToArray());
            txtCity.AutoCompleteCustomSource = source;

            source = new AutoCompleteStringCollection();
            source.AddRange(Database.GetDistinctZipList().ToArray());
            txtZip.AutoCompleteCustomSource = source;

            source = new AutoCompleteStringCollection();
            source.AddRange(Database.GetDistinctFirstList().ToArray());
            txtFirstName.AutoCompleteCustomSource = source;

            source = new AutoCompleteStringCollection();
            source.AddRange(Database.GetDistinctLastList().ToArray());
            txtLastName.AutoCompleteCustomSource = source;
        }

        public void ClearForm()
        {
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtCity.Text = "";
            txtZip.Text = "";
            txtState.Text = "CA";

            chkMonday.Checked = false;
            chkTuesday.Checked = false;
            chkWednessday.Checked = false;
            chkThursday.Checked = false;

            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtPhone3.Text = "";
            txtPhone4.Text = "";
            txtAreaCode.Text = "530";

            cmbGrade.SelectedIndex = 0;
            cmbSex.SelectedIndex = 0;
            cmbBus.SelectedIndex = 0;
            cmbGuestOf.Items.Clear();
            cmbGuestOf.Text = "";

            cmbTuesday.Text = "";
            cmbMonday.Text = "";
            cmbWednessday.Text = "";
            cmbThursday.Text = "";

            PopulateDescList();

            txtFirstName.Focus();
            isClear = true;
            tspTxtCurrentRec.Text = "";
            DoAllPopulation();
        }

        private void cmbGuestOf_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void LoadFromEntry()
        {
            var lTeenList = GetTeenListForUpdate(txtFirstName.Text, txtLastName.Text);

            if (lTeenList == null)
            {
                isNew = true;
                isClear = false;
            }
            else if (lTeenList.Count <= 1)
            {
                isNew = true;
                isClear = false;
            }
            else
            {
                frmTeenList dlg = new frmTeenList();
                dlg.lArray = lTeenList;
                dlg.ShowDialog();
                if (dlg.DialogResult != DialogResult.OK)
                {
                    txtFirstName.Text = "";
                    txtLastName.Text = "";
                    return;
                }

                isClear = false;
                if (dlg.lbTeenList.SelectedIndex <= 0)
                {
                    isNew = true;
                    isClear = false;
                }

                ListItem li = (ListItem)dlg.lbTeenList.Items[dlg.lbTeenList.SelectedIndex];

                if (li.idx < 0)
                {
                    isNew = true;
                    isClear = false;
                }
                else
                {
                    if (LoadRecord(li.idx) == false)
                    {
                        ClearForm();
                        return;
                    }
                    isNew = false;
                    isClear = false;
                    
                }
            }   
        }

        private void txtLastName_Leave(object sender, EventArgs e)
        {
            if (txtFirstName.Text == "")
            {
                txtFirstName.Focus();             
                return;
            }
            if (txtLastName.Text == "")
            {
                txtLastName.Focus();
                return;
            }

            if (isClear == false) return;
            LoadFromEntry();
            txtPhone3.Focus();
        }

        public string GenerateGuestOfKey()
        {
            string ret = "";

            ret = txtFirstName.Text.ToUpper() + " " + txtLastName.Text.ToUpper() + " " + "(" + txtAreaCode.Text + ")" +
                  " " + txtPhone3.Text + "-" + txtPhone4.Text;
                  
            return ret;
        }

        public string GetGuestOfByKey(string GuestOfKey)
        {
            if (string.IsNullOrEmpty(GuestOfKey)) return "";
            var TeenRec = Database.GetGuestOfDataSet(GuestOfKey);
            if (TeenRec == null) return "";
            return TeenRec.Title;
        }

        public List<ListItemStr> GetGuestListOf()
        {
            var filteredTeens = lstAllTeens.Where(x => x.Title.ToUpper().StartsWith(cmbGuestOf.Text.ToUpper())).ToList();
            var ret = new List<ListItemStr>();
            filteredTeens.ForEach(x =>
            {
                ret.Add(new ListItemStr(x.GuestOfKey, x.Title));
            });
            return ret;
        }

        public List<ListItem> GetTeenListForUpdate(string firstname, string lastname)
        {            
            var TeenList = Database.GetTeenListForUpdateDataSet(firstname, lastname);
            var ret = new List<ListItem>()
            {
                new ListItem(-1, "*** New Teen Record ***")
            };

            TeenList.ForEach(x =>
            {
                string Text = string.Format("{0} {1} ({2}) {3}-{4}", x.FirstName, x.LastName, x.PhoneArea, x.PhonePrefix, x.PhonePostfix);
                ret.Add(new ListItem(x.id, Text));
            });

            return ret;
        }

        public bool NoGuestUpdate = false;

        private void cmbGuestOf_TextChanged(object sender, EventArgs e)
        {
            cmbGuestOf.TextChanged -= cmbGuestOf_TextChanged;
            if (NoGuestUpdate == true) {
                NoGuestUpdate = false;
                return;
            }

            //cmbGuestOf.Items.Clear();
            string OldText = cmbGuestOf.Text;
            while (cmbGuestOf.Items.Count != 0) {
                cmbGuestOf.Items.RemoveAt(0);
            }

            var lArray = GetGuestListOf();
            if (lArray.Count <= 0) return;  //Nothing to Add.
            lArray.ForEach(x => {
                cmbGuestOf.Items.Add(x);
            });

            NoGuestUpdate = true;

            cmbGuestOf.Text = OldText;
            cmbGuestOf.SelectionStart = cmbGuestOf.Text.Length;
            cmbGuestOf.SelectionLength = 1;

            cmbGuestOf.TextChanged += cmbGuestOf_TextChanged;
        }

        private void cmbGuestOf_KeyDown(object sender, KeyEventArgs e)
        {
            int cnt = 0;

            if (e.KeyCode == Keys.Down)
            {
                NoGuestUpdate = true;
                return;
            }
            if (e.KeyCode == Keys.Up)
            {
                NoGuestUpdate = true;
                return;
            }


            for (int x = 0; x < cmbGuestOf.Text.Length; x++)
            {
                if (cmbGuestOf.Text[x] == ' ') cnt++;
            }

            if ((cnt >= 2) && (e.KeyCode == Keys.Space))
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        public bool LoadRecord(int idx)
        {
            RecordIdx = idx;

            var TeenRec = Database.LoadTeenRecord(idx);

            txtFirstName.Text = TeenRec.FirstName;
            txtLastName.Text = TeenRec.LastName;

            txtAreaCode.Text = TeenRec.PhoneArea;
            txtPhone3.Text = TeenRec.PhonePrefix;
            txtPhone4.Text = TeenRec.PhonePostfix;

            txtAddress1.Text = TeenRec.Street1;
            txtAddress2.Text = TeenRec.Street2;
            txtCity.Text = TeenRec.City;
            txtState.Text = TeenRec.State;
            txtZip.Text = TeenRec.zip;

            chkMonday.Checked = TeenRec.LstAttended[0];
            chkTuesday.Checked = TeenRec.LstAttended[1];
            chkWednessday.Checked = TeenRec.LstAttended[2];
            chkThursday.Checked = TeenRec.LstAttended[3];

            cmbMonday.Text = TeenRec.LstDecision[0];
            cmbTuesday.Text = TeenRec.LstDecision[1];
            cmbWednessday.Text = TeenRec.LstDecision[2];
            cmbThursday.Text = TeenRec.LstDecision[3];

            int x;
            string rGrade = TeenRec.Grade;
            string rSex = TeenRec.Sex;
            string rBus = TeenRec.Bus;

            for (x = 0; x < cmbGrade.Items.Count; x++)
            {
                string selGrade = cmbGrade.Items[x].ToString();
                if (selGrade == rGrade) cmbGrade.SelectedIndex = x;
            }

            for (x = 0; x < cmbSex.Items.Count; x++)
            {
                string selSex = cmbSex.Items[x].ToString();
                if (selSex == rSex) cmbSex.SelectedIndex = x;
            }

            for (x = 0; x < cmbBus.Items.Count; x++)
            {
                string selBus = cmbBus.Items[x].ToString();
                if (selBus == rBus) cmbBus.SelectedIndex = x;
            }

            if (!string.IsNullOrEmpty(TeenRec.GuestOf))
            {
                lstAllTeens.ForEach(y =>
                {
                    if (y.GuestOfKey != TeenRec.GuestOf) return;
                    cmbGuestOf.Text = y.Title;
                    cmbGuestOf_TextChanged(null, null);
                });
            }
            else
            {
                cmbGuestOf.Text = "";
            }
            return true;
        }

        public bool ValidateRecord()
        {
            if (cmbSex.SelectedText == null)
            {
                MessageBox.Show("Sex not specified", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (cmbGrade.SelectedText == null)
            {
                MessageBox.Show("Grade not specified", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if ((txtPhone3.Text == "") ||
               (txtPhone4.Text == "") ||
               (txtAreaCode.Text == ""))
            {
                MessageBox.Show("Enter a phone number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtPhone3.Text.Length != 3)
            {
                MessageBox.Show("Phone Number is invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtPhone4.Text.Length != 4)
            {
                MessageBox.Show("Phone Number is invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtAreaCode.Text.Length != 3)
            {
                MessageBox.Show("Phone Number is invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            if ((txtAddress1.Text == "") ||
               (txtCity.Text == "") ||
               (txtState.Text == "") ||
               (txtZip.Text == ""))
            {
                MessageBox.Show("Enter the address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string GuestIdx = "UNKNOWN";

            if (cmbGuestOf.Text != "")
            {
                int x;
                for (x = 0; x < cmbGuestOf.Items.Count; x++)
                {
                    ListItemStr li = (ListItemStr)cmbGuestOf.Items[x];
                    if (li.text == cmbGuestOf.Text) 
                        GuestIdx = li.idx;
                }

                if (GuestIdx == "UNKNOWN")
                {
                    MessageBox.Show("Complete the Guest Of Text or Clear it out.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                bool NightVisitMask = chkMonday.Checked || chkTuesday.Checked || chkWednessday.Checked || chkThursday.Checked;

                if (NightVisitMask == false)
                {
                    MessageBox.Show("Must mark the first night that this person was a guest.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            if (enforceAttendenceToolStripMenuItem.Checked)
            {
                DayOfWeek WeekDay = DateTime.Today.DayOfWeek;

                switch (WeekDay)
                {
                    case DayOfWeek.Monday:
                        if (!chkMonday.Checked)
                        {
                            MessageBox.Show("Monday night must be checked because the attendance is being enforced");
                            return false;
                        }
                        break;
                    case DayOfWeek.Tuesday:
                        if (!chkTuesday.Checked)
                        {
                            MessageBox.Show("Tuesday night must be checked because the attendance is being enforced");
                            return false;
                        }
                        break;
                    case DayOfWeek.Wednesday:
                        if (!chkWednessday.Checked)
                        {
                            MessageBox.Show("Wednesday night must be checked because the attendance is being enforced");
                            return false;
                        }
                        break;
                    case DayOfWeek.Thursday:
                        if (!chkThursday.Checked)
                        {
                            MessageBox.Show("Thursday night must be checked because the attendance is being enforced");
                            return false;
                        }
                        break;
                }
            }

            return true;
        }

        private bool SaveRecord()
        {
            if (ValidateRecord() == false) return false;

            var TeenData = ToTeenRec();
            TeenData.GuestOfKey = Guid.NewGuid().ToString(); // Set the guest of Guid.
            Database.SaveRecord(TeenData);
            DoAllPopulation();
            return true;
        }

        private TeenProxy.wcfTeens.Teen ToTeenRec()
        {
            var TeenData = new TeenProxy.wcfTeens.Teen();

            TeenData.id = RecordIdx;
            TeenData.Sex = cmbSex.Items[cmbSex.SelectedIndex].ToString();
            TeenData.Grade = cmbGrade.Items[cmbGrade.SelectedIndex].ToString();
            TeenData.Bus = cmbBus.Items[cmbBus.SelectedIndex].ToString();
            TeenData.PhoneArea = txtAreaCode.Text;
            TeenData.PhonePrefix = txtPhone3.Text;
            TeenData.PhonePostfix = txtPhone4.Text;
            TeenData.FirstName = txtFirstName.Text;
            TeenData.LastName = txtLastName.Text;
            TeenData.Street1 = txtAddress1.Text;
            TeenData.Street2 = txtAddress2.Text;
            TeenData.City = txtCity.Text;
            TeenData.State = txtState.Text;
            TeenData.zip = txtZip.Text;

            TeenData.LstAttended = new bool[4];
            TeenData.LstDecision = new string[4];

            TeenData.LstAttended[0] = chkMonday.Checked;
            TeenData.LstAttended[1] = chkTuesday.Checked;
            TeenData.LstAttended[2] = chkWednessday.Checked;
            TeenData.LstAttended[3] = chkThursday.Checked;

            TeenData.LstDecision[0] = cmbMonday.Text;
            TeenData.LstDecision[1] = cmbTuesday.Text;
            TeenData.LstDecision[2] = cmbWednessday.Text;
            TeenData.LstDecision[3] = cmbThursday.Text;

            var GuestOfItem = (cmbGuestOf.SelectedItem as ListItemStr);
            if (GuestOfItem == null) TeenData.GuestOf = ""; else TeenData.GuestOf = GuestOfItem.idx;
            return TeenData;
        }

        private bool UpdateRecord()
        {
            if (ValidateRecord() == false) return false;

            var TeenData = ToTeenRec();
            Database.UpdateRecord(TeenData);
            DoAllPopulation();
            return true;
        }

        private void txtNumOnly_KeyDown(object sender, KeyEventArgs e)
        {           
            switch (e.KeyCode)
            {
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                  e.Handled = false;
                    break;
                default:
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void txtNumOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar)) return;
            if (Char.IsDigit(e.KeyChar)) return;
            if (e.KeyChar.ToString() == "\b") return;
            e.Handled = true;
        }

        private void txtFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {                
                e.Handled = true;
                txtLastName.Focus();
            }
        }

        private void txtLastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
                txtAreaCode.Focus();
            }
        }

        private void AllowIsNotClear_Enter(object sender, EventArgs e)
        {
            if (isClear == true) txtFirstName.Focus();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void PopulateDescList()
        {
            var lArray = Database.LoadDecisions();

            cmbMonday.Items.Clear();
            cmbTuesday.Items.Clear();
            cmbWednessday.Items.Clear();
            cmbThursday.Items.Clear();

            lArray.ForEach(x => {
                if (string.IsNullOrEmpty(x.text)) return;
                cmbMonday.Items.Add(x);
                cmbTuesday.Items.Add(x);
                cmbWednessday.Items.Add(x);
                cmbThursday.Items.Add(x);
            });
        }

        private void cmbDesc_TextChanged(object sender, EventArgs e)
        {

        }

        public void LoadTeenRecord(string fname, string lname)
        {
            frmSelectTeens dlg = new frmSelectTeens("Select a Teen Record", false);
            dlg.txtFilter.Text = "";
            if (fname != "") dlg.txtFilter.Text = fname + "%";
            if (lname != "") dlg.txtFilter.Text += "%" + lname + "%";
            if (dlg.ShowDialog() != DialogResult.OK) return;

            ListItem li = (ListItem)dlg.lstTeens.SelectedItem;
            if (li == null) return;
            LoadRecord(li.idx);

            isNew = false;
            isClear = false;
            DoAllPopulation();
        }

        private void loadRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            LoadTeenRecord("", "");
        }

        private void txtPhone3_TextChanged(object sender, EventArgs e)
        {
            if (txtPhone3.Text.Length >= 3) txtPhone4.Focus();
        }

        private void txtPhone4_TextChanged(object sender, EventArgs e)
        {
            if (txtPhone4.Text.Length >= 4) cmbGuestOf.Focus();
        }

        private void cmbBus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = cmbBus.SelectedItem.ToString();
            switch (value)
            {
                case "1": lblBusType.Text = "Bus #1";
                    break;
                case "2": lblBusType.Text = "Bus #2";
                    break;
                case "3": lblBusType.Text = "Bus #3";
                    break;
                case "D": lblBusType.Text = "Driven In";
                    break;
                case "U": lblBusType.Text = "Unknown";
                    break;
            }
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ClearForm();
            }
            if (isClear == true)
            {
                if ((e.Control == true) && (e.KeyCode == Keys.A)) LoadTeenRecord(txtFirstName.Text, txtLastName.Text);
                return;
            }
            if ((e.KeyCode == Keys.Enter) && (e.Control == true))
            {
                bool ret;
                // Save or update here..
                if (isNew == true) ret = SaveRecord(); else ret = UpdateRecord();
                if (ret == false) return; // Can't clear the phone because the update is needed.
                ClearForm();
            }

            if ((e.Control == true) && (e.KeyValue == 49)) chkMonday.Checked = !chkMonday.Checked;
            if ((e.Control == true) && (e.KeyValue == 50)) chkTuesday.Checked = !chkTuesday.Checked;
            if ((e.Control == true) && (e.KeyValue == 51)) chkWednessday.Checked = !chkWednessday.Checked;
            if ((e.Control == true) && (e.KeyValue == 52)) chkThursday.Checked = !chkThursday.Checked;

            if (e.KeyCode == Keys.F1) chkMonday.Checked = !chkMonday.Checked;
            if (e.KeyCode == Keys.F2) chkTuesday.Checked = !chkTuesday.Checked;
            if (e.KeyCode == Keys.F3) chkWednessday.Checked = !chkWednessday.Checked;
            if (e.KeyCode == Keys.F4) chkThursday.Checked = !chkThursday.Checked;
        }

        private void initializeNightlyRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            initializeNightlyRecordsToolStripMenuItem.Enabled = false;
            MessageBox.Show("This function has been Depricated.");
        }

        private void cmbSex_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Top5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {            
            LinkLabel lnk = (LinkLabel)sender;
            if (lnk.Tag.GetType() != typeof(int)) return;
            int idx = (int)lnk.Tag;
            if (idx < 0) return;
            LoadRecord(int.Parse(lnk.Tag.ToString()));
            isNew = false;
            isClear = false;
        }

        public void PopulateTeenData()
        {
            var TeenRpt = Database.GetTeenData();
            lblData.Text = "";
            List<string> lstNights = new List<string>() { "Monday", "Tuesday", "Wednessday", "Thursday" };
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < TeenRpt.lstNightTotals.Length; x++)
            {
                sb.AppendLine(string.Format("{0} Total Teens: {1} Guests: {2}", lstNights[x].PadLeft(10), TeenRpt.lstNightTotals[x].ToString("0000"), TeenRpt.lstGuestTotals[x].ToString("0000")));
            }

            sb.AppendLine();
            sb.AppendLine(string.Format("Total Guest: {0}", TeenRpt.GuestTotal.ToString()));
            sb.AppendLine(string.Format("Total Teens: {0}", TeenRpt.TeenTotal.ToString()));
            lblData.Text = sb.ToString(); 
        }
        public void PopulateReportData()
        {
            lblTop01.Text = "";
            lblTop01.Tag = -1;
            lblTop02.Text = "";
            lblTop02.Tag = -1; 
            lblTop03.Text = "";
            lblTop03.Tag = -1;
            lblTop04.Text = "";
            lblTop04.Tag = -1;
            lblTop05.Text = "";
            lblTop05.Tag = -1;

            List<KeyValuePair<TeenProxy.wcfTeens.Teen, int>> Top5 = Database.GetTopFiveList();
            for(int x = 0;x < 5; x++)
            {
                if (Top5.Count <= x) continue;
                string FullNameText = string.Format("Guests: {0} {1} {2}", Top5[x].Key.FirstName, Top5[x].Key.LastName, Top5[x].Value);
                switch(x)
                {
                    case 0:
                        lblTop01.Text = FullNameText;
                        lblTop01.Tag = Top5[x].Key.id;
                        break;
                    case 1:
                        lblTop02.Text = FullNameText;
                        lblTop02.Tag = Top5[x].Key.id;
                        break;
                    case 2:
                        lblTop03.Text = FullNameText;
                        lblTop03.Tag = Top5[x].Key.id;
                        break;
                    case 3:
                        lblTop04.Text = FullNameText;
                        lblTop04.Tag = Top5[x].Key.id;
                        break;
                    case 4:
                        lblTop05.Text = FullNameText;
                        lblTop05.Tag = Top5[x].Key.id;
                        break;
                }
            }
        }

        public void PopulateRecordNav()
        {
            tspTxtTotal.Text = "";
            tspTxtCurrentRec.Text = "";

            tspTxtTotal.Text = Database.GetTotalRecords().ToString();
            if (!isClear) tspTxtCurrentRec.Text = Database.GetRecordsToIdx(RecordIdx).ToString(); else tspTxtCurrentRec.Text = "";

        }

        public void DoAllPopulation()
        {
            PopulateTeenData();
            PopulateReportData();
            PopulateRecordNav();
            lstAllTeens = Database.GetAllTeens();
            cmbGuestOf_TextChanged(null, null);
        }

        private void lblGuestList_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string FileName = Path.GetTempFileName() + ".txt";

            var lstGuests = Database.GetListGuestBrought(RecordIdx);
            StringBuilder sb = new StringBuilder();
            if(lstGuests.Count <= 0)
            {
                sb.AppendLine("No Guests");
            }
            else
            {
                lstGuests.ForEach(g => { sb.AppendLine(g); });
            }
            
            File.WriteAllText(FileName, sb.ToString());
            Process.Start(FileName);
        }

        private void lnkRefreshGuestData_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PopulateReportData();
        }

        private void lnkRefreshTeenData_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PopulateTeenData();
        }

        private void tsbFirst_Click(object sender, EventArgs e)
        {
            int RecToLoad = 0;
            RecToLoad = Database.GetFirstIdx();
            LoadRecord(RecToLoad);
            isNew = false;
            isClear = false;
            DoAllPopulation();
        }

        private void tsbEnd_Click(object sender, EventArgs e)
        {
            int RecToLoad = 0;
            RecToLoad = Database.GetLastIdx();
            LoadRecord(RecToLoad);
            isNew = false;
            isClear = false;
            DoAllPopulation();
        }

        private void tsbPrevious_Click(object sender, EventArgs e)
        {
            int RecToLoad = 0;
            RecToLoad = Database.GetPrevIdx(RecordIdx);
            LoadRecord(RecToLoad);
            isNew = false;
            isClear = false;
            DoAllPopulation();
        }

        private void tsbNext_Click(object sender, EventArgs e)
        {
            int RecToLoad = 0;
            RecToLoad = Database.GetNextIdx(RecordIdx);
            LoadRecord(RecToLoad);
            isNew = false;
            isClear = false;
            DoAllPopulation();
        }

        private void tspDelete_Click(object sender, EventArgs e)
        {
            if (isClear) return;
            if (MessageBox.Show("Are you sure, you can not undo this?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes) return;
            Database.DeleteRecord(RecordIdx);
            tsbPrevious_Click(null, null);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtAddress1.Text = "Unknown";
            txtCity.Text = "Unknown";
            txtZip.Text = "00000";
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtAddress1.Text = "Out of Area";
            txtCity.Text = "Out of Area";
            txtZip.Text = "00000";
        }

        private void txtPhone3_Leave(object sender, EventArgs e)
        {
            if (txtPhone3.Text == "") txtPhone3.Text = "000";
        }

        private void txtPhone4_Leave(object sender, EventArgs e)
        {
            if (txtPhone4.Text == "") txtPhone4.Text = "0000";
        }

        private void tsbEscapeRecord_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void tsbSaveRecord_Click(object sender, EventArgs e)
        {
            bool ret;
            // Save or update here..
            if (isNew == true) ret = SaveRecord(); else ret = UpdateRecord();
            if (ret == false) return; // Can't clear the phone because the update is needed.
            ClearForm();
        }

        private void geoCodingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            geocoding g = new geocoding();
            g.Process();
        }
    }

    public class ListItem
    {
        public string text;
        public int idx;

        public override string ToString()
        {
            return text;
        }

        public ListItem(int pIdx, string pText)
        {
            text = pText;
            idx = pIdx;
        }
    }

    public class ListItemStr
    {
        public string text;
        public string idx;

        public override string ToString()
        {
            return text;
        }

        public ListItemStr(string pIdx, string pText)
        {
            text = pText;
            idx = pIdx;
        }
    }
}

