using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TeenCheckin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
            catch (Exception ex)
            {
                frmException.Show(ex);
            }
        }
    }
}
