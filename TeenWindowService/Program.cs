using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace TeenWindowService
{
    static class Program
    {
        static public bool Interactive = false;
        static public bool Config = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if(args.Length > 0)
            {
                foreach(string arg in args)
                {
                    if(arg.ToUpper().Trim() == "-I") { Interactive = true; }
                    if(arg.ToUpper().Trim() == "-C") { Config = true; }
                }
            }

            wcfTeenService.Settings.LoadSettings();

            if (Config)
            {
                ReviewSettings();
                Console.WriteLine("Enter Server (Enter to not change):");
                string Server = Console.ReadLine();
                Console.WriteLine("Enter UserId (Enter to not change):");
                string UserName = Console.ReadLine();
                Console.WriteLine("Enter Password (Enter to not change):");
                string Password = Console.ReadLine();

                if(!string.IsNullOrEmpty(Server))
                {
                    if (Server == "BLANK") Server = "";
                    wcfTeenService.Settings.Server = Server;
                }

                if(!string.IsNullOrEmpty(UserName))
                {
                    if (UserName == "BLANK") UserName = "";
                    wcfTeenService.Settings.UserName = UserName;
                }

                if(!string.IsNullOrEmpty(Password))
                {
                    if (Password == "BLANK") Password = "";
                    wcfTeenService.Settings.Password = Password;
                }

                ReviewSettings(true);
                wcfTeenService.Settings.SaveSettings();
                Console.WriteLine("Configuration Saved!");
                Console.WriteLine("Press any key to quit.");
                Console.ReadLine();

                return;
            }

            if(!Interactive)
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new TeenService()
                };
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                Console.WriteLine("Interactive Mode Started");
                Console.WriteLine("Press any key to quit");
                TeenService t = new TeenService();
                t.InteractiveStart();
                Console.ReadLine();
                t.InteractiveEnd();
            }
        }

        static public void ReviewSettings(bool newSettings = false)
        {
            Console.WriteLine("{0} Settings:", newSettings ? "New" : "Current");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("Server: {0}", wcfTeenService.Settings.Server);
            Console.WriteLine("User Name: {0}", wcfTeenService.Settings.UserName);
            Console.WriteLine("Password: {0}", wcfTeenService.Settings.Password);
            
        }
    }
}
