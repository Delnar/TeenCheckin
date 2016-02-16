using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Text;

using Nancy;
using Nancy.Hosting.Self;
using System.Net.Sockets;

namespace TeenWindowService
{
    public partial class TeenService : ServiceBase
    {

        #region Service Implementations
        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public long dwServiceType;
            public ServiceState dwCurrentState;
            public long dwControlsAccepted;
            public long dwWin32ExitCode;
            public long dwServiceSpecificExitCode;
            public long dwCheckPoint;
            public long dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
        #endregion

        #region Private Fields
        private System.ServiceModel.Web.WebServiceHost _HTTPserviceHost = null;
        private NancyHost _WebHost = null;

        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        public TeenService()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        #region Service Methods
        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);


            // System.Diagnostics.Debugger.Launch();  // If I need to debug the service.. :)

            // Initial wcfTeenServiceX
            StartWebHost();
            StartHTTPService();

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            StopHTTPService();
            StopWebService();
        }
        #endregion

        #region WCF Service Methods
        /// <summary>
        /// Start the HTTP Service
        /// </summary>
        /// <param name="NetTCPServiceHost"></param>
        public void StartHTTPService()
        {
            if (_HTTPserviceHost != null)
            {
                _HTTPserviceHost.Close();
            }
            _HTTPserviceHost = null;

            _HTTPserviceHost = new System.ServiceModel.Web.WebServiceHost(typeof(wcfTeenService.wcf.TeenService));
            _HTTPserviceHost.Open();
        }

        public void StopHTTPService()
        {
            if (_HTTPserviceHost != null)
            {
                _HTTPserviceHost.Close();
                _HTTPserviceHost = null;
            }
        }
        #endregion

        #region Nacy WEB Serverver Methods
        public void StartWebHost()
        {
            if (_WebHost != null)
            {
                _WebHost.Stop();
                _WebHost.Dispose();
            }
            string URL = "http://localhost";
            _WebHost = new NancyHost(new Uri(URL));
            _WebHost.Start();
        }

        public void StopWebService()
        {
            if (_WebHost != null)
            {
                _WebHost.Stop();
                _WebHost.Dispose();
            }
        }
        #endregion

        #region Interactive Code
        public void InteractiveStart()
        {
            StartHTTPService();
            StartWebHost();
        }

        public void InteractiveEnd()
        {
            StopHTTPService();
            StopWebService();
        }
        #endregion

        #endregion


    }
}
