using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace TeenProxy
{
    public class ClientProxy
    {
        #region Static Private Fields
        static private string _TargetServer = "localhost";
        static private TimeSpan _TimeOut = new TimeSpan(2, 0, 0);
        static private string _ServerAddress = "http://{0}:8001/TeenService/";
        // static private string _ServerAddress = "http://{0}/wcfTeenServiceX/TeenServiceX.svc";

        #endregion
        #region Static Public Properties
        /// <summary>
        /// Target server for the ExportData Service
        /// </summary>
        static public string TargetServer
        {
            get { return _TargetServer; }
            set { _TargetServer = value; }
        }
        /// <summary>
        /// The Time out of the Export Data Web Service
        /// </summary>
        static internal TimeSpan TimeOut
        {
            get { return _TimeOut; }
            set { _TimeOut = value; }
        }
        /// <summary>
        /// Address of the Export Data Service
        /// </summary>
        static public string ServerAddress
        {
            get { return _ServerAddress; }
            set { _ServerAddress = value; } 
        }
        #endregion
        #region Static Public Methods
        /// <summary>
        /// Create the Export Data Client
        /// </summary>
        /// <returns>The Export Data Client</returns>
        static public wcfTeens.TeenServiceClient CreateTeenClient()
        {
            if (string.IsNullOrEmpty(TargetServer)) { throw new Exception("Target server needs to be set"); }

            WSHttpBinding binding = new WSHttpBinding();
            binding.CloseTimeout = TimeOut;
            binding.OpenTimeout = TimeOut;
            binding.ReceiveTimeout = TimeOut;
            binding.SendTimeout = TimeOut;
            binding.MaxBufferPoolSize = long.MaxValue;  // Making it long
            binding.MaxBufferPoolSize = long.MaxValue;
            binding.Security.Mode = SecurityMode.None;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas();
            binding.ReaderQuotas.MaxDepth = 32;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;

            string RemoteEndpointURI = string.Format(_ServerAddress, TargetServer);
            EndpointAddress endpoint = new EndpointAddress(RemoteEndpointURI);

            wcfTeens.TeenServiceClient ret = new wcfTeens.TeenServiceClient(binding, endpoint);

            return ret;
        }
        #endregion
    }
}
