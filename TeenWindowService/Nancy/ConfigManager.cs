using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeenWindowService.Nancy
{
    public class ConfigInfo
    {
        public String Server
        {
            get { return wcfTeenService.Settings.Server; }
            set { wcfTeenService.Settings.Server = value; }
        }
        public String UserName
        {
            get { return wcfTeenService.Settings.UserName; }
            set { wcfTeenService.Settings.UserName = value; }
        }
        public String Password
        {
            get { return wcfTeenService.Settings.Password; }
            set { wcfTeenService.Settings.Password = value; }
        }
    }
    public class ConfigManager
    {
        public ConfigInfo config;
        public void LoadConfig()
        {
            wcfTeenService.Settings.LoadSettings();
        }

        public void SaveConfig()
        {
            wcfTeenService.Settings.SaveSettings();
        }
    }
}
