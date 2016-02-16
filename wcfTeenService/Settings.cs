using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wcfTeenService
{
    static public class Settings
    {

        #region Static Private Fields
        static private string _UserName = "";
        static private string _Password = "";
        static private string _Server = "";
        static private readonly string RegKey = "Software\\SBS\\TeenCheckin";
        #endregion
        #region Static Public Fields
        static public string UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                Code.TeenDataHelper.UserName = UserName;
            }
        }
        static public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                Code.TeenDataHelper.Password = _Password;
            }
        }
        static public string Server
        {
            get { return _Server; }
            set
            {
                _Server = value;
                Code.TeenDataHelper.ServerAddress = _Server; 
            }
        }
        #endregion
        #region Static Methods

        static private string ReadRegistryString(string key)
        {
            Microsoft.Win32.RegistryKey rkey;
            rkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegKey, true);
            if (rkey == null)
            {
                rkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RegKey);
            }

            if (rkey.GetValue(key) == null)
            {
                rkey.SetValue(key, "");
            }
            return rkey.GetValue(key).ToString();
        }

        static private void SetRegistryString(string key, string value)
        {
            Microsoft.Win32.RegistryKey rkey;

            rkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegKey, true);
            if (rkey == null) rkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RegKey);
            rkey.SetValue(key, value);
        }

        static private bool ReadRegistryBool(string key)
        {
            string val = ReadRegistryString(key);
            if (val.ToUpper() == "Y") return true; else return false;
        }

        static private void SetRegistryBool(string key, bool value)
        {
            if (value)
                SetRegistryString(key, "Y");
            else
                SetRegistryString(key, "N");
        }

        static public void LoadSettings()
        {
            Server = ReadRegistryString("Server");
            UserName = ReadRegistryString("UserName");
            Password = ReadRegistryString("Password");
        }

        static public void SaveSettings()
        {
            SetRegistryString("Server", Server);
            SetRegistryString("UserName", UserName);
            SetRegistryString("Password", Password);
        }
        #endregion

    }
}