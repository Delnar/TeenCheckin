using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace wcfTeenService.Code
{
    public class LogMan
    {

        #region Enumerations
        public enum LogLevelEnum
        {
            DEBUG = 0,
            INFO,
            WARN,
            ERROR,
            FATAL
        }
        #endregion
        #region Private Fields
        public static bool _LoggingEnabled = false;        
        #endregion
        #region Public Properties
        public static bool LoggingEnabled
        {
            get { return _LoggingEnabled; }
            set { _LoggingEnabled = value; }
        }

        public static readonly ILog logger = LogManager.GetLogger(typeof(LogMan));
        #endregion
        #region Constructor
        #endregion
        #region Methods
        public static void Initialize(string ConfigFile)
        {
            LoggingEnabled = false;

            // var fullFile = Path.Combine(Settings.VirtPath, ConfigFile);
            var fullFile = ConfigFile;
            var fileInfo = new FileInfo(fullFile);

            if (!File.Exists(fullFile)) return;
                
            XmlConfigurator.ConfigureAndWatch(fileInfo);
            LoggingEnabled = true;
        }

        public static void Log(string msg, params object[] args)
        {
            Log(LogLevelEnum.INFO, msg, args);
        }

        public static void Log(LogLevelEnum logLevel, string msg, params object[] args)
        {
            if (!LoggingEnabled) return; // No logging allowed..
            if(args != null)
            {
                if(args.Length > 0)
                    msg = string.Format(msg, args);
            }
            
            switch(logLevel)
            {
                case LogLevelEnum.DEBUG:
                    logger.Debug(msg);
                    break;
                case LogLevelEnum.INFO:
                    logger.Info(msg);
                    break;
                case LogLevelEnum.WARN:
                    logger.Warn(msg);
                    break;
                case LogLevelEnum.ERROR:
                    logger.Error(msg);
                    break;
                case LogLevelEnum.FATAL:
                    logger.Fatal(msg);
                    break;
            }
        }

        public static void Log(Exception ex)
        {
            if(ex is DbEntityValidationException)
            {
                Log(ex as DbEntityValidationException);
                return;
            }

            Log(LogLevelEnum.ERROR, "-------------------------------------");
            Log(LogLevelEnum.ERROR, ex.Message);
            var iex = ex.InnerException;
            while(iex != null)
            {
                Log(LogLevelEnum.ERROR, iex.Message);
                iex = iex.InnerException;
            }

            Log(LogLevelEnum.ERROR, ex.StackTrace);
            Log(LogLevelEnum.ERROR, ex.Source);
            Log(LogLevelEnum.ERROR, "-------------------------------------");

        }

        public static void Log(DbEntityValidationException ex)
        {
            Log(LogLevelEnum.ERROR, "-------------------------------------");
            StringBuilder sb = new StringBuilder();
            foreach (var eve in ex.EntityValidationErrors)
            {
                sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State));
                foreach (var ve in eve.ValidationErrors)
                {
                    sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                        ve.PropertyName, ve.ErrorMessage));
                }
            }

            string error = sb.ToString();
            Log(LogLevelEnum.ERROR, error);
            Log(LogLevelEnum.ERROR, ex.StackTrace);
            Log(LogLevelEnum.ERROR, ex.Source);
            Log(LogLevelEnum.ERROR, "-------------------------------------");

        }
        #endregion
    }
}