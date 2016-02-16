using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Reflection;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Win32;

namespace TeenCheckin
{
    public partial class frmException : Form
    {
        static public Exception myException;
        static public void Show(Exception ex)
        {
            myException = ex;
            System.Threading.Thread myThread = new System.Threading.Thread(ExceptionWindowProc);
            myThread.SetApartmentState(System.Threading.ApartmentState.STA);
            myThread.Start();
            myThread.Join();
        }

        public static void ExceptionWindowProc()
        {
            frmException dlg = new frmException();
            dlg.ShowDialog();
            return;
        }

        public frmException()
        {
            InitializeComponent();
        }

        private void frmException_Load(object sender, EventArgs e)
        {
            Exception cException = myException;
            lstExceptionList.Items.Add(new ExceptionInfo(cException, "Exception #01"));
            int ExceptionCount = 0;
            bool Done = false;

            if (cException.InnerException == null) Done = true;
            while (!Done)
            {
                cException = cException.InnerException;
                ExceptionCount++;
                if (ExceptionCount > 25) Done = true;
                lstExceptionList.Items.Add(new ExceptionInfo(cException, "Exception #" + ExceptionCount.ToString("00")));
            }
            lstExceptionList.SelectedIndex = 0;
        }

        private void lstExceptionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExceptionInfo exi = (ExceptionInfo)lstExceptionList.SelectedItem;
            txtMessage.Text = exi.ex.Message;
            txtStacktrace.Text = exi.ex.StackTrace;
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butExport_Click(object sender, EventArgs e)
        {
            //Export to text file
            String sSystemInformation = GetSystemInformation();

            string TimeStamp = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + " " + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            //string ErrorLogPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\RocketSolvers01\Logs\ErrorLog\" + TimeStamp + "Log.txt";
            //System.IO.File.WriteAllText(ErrorLogPath, sSystemInformation);

            //Displays a SaveFileDialog so the user can save the error log file
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
            saveFileDialog1.FileName = "RocketSolversErrorLog.txt";
            saveFileDialog1.Filter = "Text File|*.txt";
            saveFileDialog1.Title = "Save Rocket Solvers log file";
            saveFileDialog1.FilterIndex = 1;

            //If the user clicked on the OK button
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) 
            {
                //If the file name is not an empty string open it for saving.
                if (saveFileDialog1.FileName != "")
                {
                    //Save the file to the user selected location
                    System.IO.File.WriteAllText(saveFileDialog1.FileName, sSystemInformation);

                    //Let the user know that they have successfully save a file
                    MessageBox.Show("The log file was successfully saved to: " + saveFileDialog1.FileName, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private string GetSystemInformation()
        {
            string sSystemInformation = "***** ROCKET SOLVERS ERROR LOG ***** " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "\r\n" + "\r\n";
                        
            try //Need to catch any errors that might occur as this function will be called when an error already has occured
            {
                if (myException != null)
                {
                    sSystemInformation += "Exception Message:" + "\r\n";
                    sSystemInformation += "\r\n" + myException.Message + "\r\n";
                    sSystemInformation += "Exception StackTrace:" + "\r\n";
                    sSystemInformation += "\r\n" + myException.StackTrace + "\r\n";
                    Exception ReadException = myException;
                    while (ReadException.InnerException != null) //Keep reading inner exceptions as long as there are more to read
                    {
                        ReadException = ReadException.InnerException;
                        sSystemInformation += "Inner Exception Message:" + "\r\n";
                        sSystemInformation += "\r\n" + ReadException.Message + "\r\n";
                        sSystemInformation += "Inner Exception StackTrace:" + "\r\n";
                        sSystemInformation += "\r\n" + ReadException.StackTrace + "\r\n";
                    }
                }
                else //For testing this, an exception is not thrown the form is just opened 
                {
                    sSystemInformation += "An exception did not occur." + "\r\n";
                }

                sSystemInformation += "\r\n" + "***** DEBUG SYSTEM INFORMATION ***** " + "\r\n";
                sSystemInformation += "ComputerName: " + SystemInformation.ComputerName + "\r\n";
                sSystemInformation += "Windows Version and Service pack: " + Environment.OSVersion.ToString() + "\r\n";

                Process proc = Process.GetCurrentProcess();
                sSystemInformation += "Memory: " + proc.PrivateMemorySize64 + "\r\n";

                ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
                disk.Get();
                sSystemInformation += "Hard Disk Free: " + disk["FreeSpace"] + " bytes" + "\r\n";
                sSystemInformation += "Hard Disk Total: " + disk["Size"] + " bytes" + "\r\n";

                sSystemInformation += ".NET Version Installed: " + Environment.Version.ToString() + "\r\n";
                sSystemInformation += "DirectX Version Installed: " + GetDirectxMajorVersion() + "\r\n";

                ArrayList ManagementInformation = new ArrayList();
                ManagementInformation.AddRange(GetManagementInformation("Win32_DiskDrive"));
                ManagementInformation.AddRange(GetManagementInformation("Win32_OperatingSystem"));
                ManagementInformation.AddRange(GetManagementInformation("Win32_Processor"));
                ManagementInformation.AddRange(GetManagementInformation("Win32_ComputerSystem"));
                ManagementInformation.AddRange(GetManagementInformation("Win32_StartupCommand"));
                ManagementInformation.AddRange(GetManagementInformation("Win32_ProgramGroup"));
                ManagementInformation.AddRange(GetManagementInformation("Win32_SystemDevices"));
                sSystemInformation += "\r\n" + "***** MANAGEMENT INFORMATION ***** " + "\r\n";
                foreach (PropertyData item in ManagementInformation)
                {
                    sSystemInformation += item.Name + " : " + item.Value + "\r\n";
                }

                sSystemInformation += "\r\n" + "***** ENVIRONMENT VARIABLES ***** " + "\r\n";
                //Add each property of the Environment.GetEnvironmentVariables to the message string
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                foreach (DictionaryEntry de in environmentVariables)
                {
                    sSystemInformation += de.Key + " : " + de.Value + "\r\n";
                }

                sSystemInformation += "\r\n" + "***** SYSTEM INFORMATION ***** " + "\r\n";
                //Add each property of the SystemInformation class to the message string
                Type t = typeof(System.Windows.Forms.SystemInformation);
                PropertyInfo[] pi = t.GetProperties();
                for (int i = 0; i < pi.Length; i++)
                {
                    object propval = pi[i].GetValue(SystemInformation.PowerStatus, null);
                    sSystemInformation += pi[i].Name + ": " + propval.ToString() + "\r\n";
                }
            }
            catch (Exception Ex)
            {
                sSystemInformation += "Exception ocurred when getting system information: " + Ex.Message + "\r\n";
            }
            
            return sSystemInformation;
        }

        private int GetDirectxMajorVersion()
        {
            int directxMajorVersion = 0;

            var OSVersion = Environment.OSVersion;

            // if Windows Vista or later
            if (OSVersion.Version.Major >= 6)
            {
                // if Windows 7 or later
                if (OSVersion.Version.Major > 6 || OSVersion.Version.Minor >= 1)
                {
                    directxMajorVersion = 11;
                }
                // if Windows Vista
                else
                {
                    directxMajorVersion = 10;
                }
            }
            // if Windows XP or earlier.
            else
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\DirectX"))
                {
                    string versionStr = key.GetValue("Version") as string;
                    if (!string.IsNullOrEmpty(versionStr))
                    {
                        var versionComponents = versionStr.Split('.');
                        if (versionComponents.Length > 1)
                        {
                            int directXLevel;
                            if (int.TryParse(versionComponents[1], out directXLevel))
                            {
                                directxMajorVersion = directXLevel;
                            }
                        }
                    }
                }
            }

            return directxMajorVersion;
        }

        public ArrayList GetManagementInformation(string queryObject)
        {
            ManagementObjectSearcher searcher;
            int i = 0;
            ArrayList hd = new ArrayList();
            try
            {
                searcher = new ManagementObjectSearcher(
                  "SELECT * FROM " + queryObject);
                foreach (ManagementObject wmi_HD in searcher.Get())
                {
                    i++;
                    PropertyDataCollection searcherProperties = wmi_HD.Properties;
                    foreach (PropertyData sp in searcherProperties)
                    {
                        hd.Add(sp);
                    }
                }
            }
            catch (Exception)
            {
                //Ignore any errors getting management data information from the client computer
            }
            return hd;
        }
    }

    public class ExceptionInfo
    {
        public string Text;
        public Exception ex;
        public ExceptionInfo(Exception ex, string Text)
        {
            this.Text = Text;
            this.ex = ex;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
