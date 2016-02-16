using System;
using System.Collections.Generic;
using System.Web;
using System.Collections.Specialized;
using System.Deployment.Application;
using System.Windows.Forms;

namespace TeenCheckin
{
    public class ClickOnceParams
    {
        public Dictionary<string, string> dictParams = new Dictionary<string, string>();
        public ClickOnceParams()
        {
            NameValueCollection parms = GetQueryStringParameters();
            foreach (string s in parms.AllKeys)
            {
                dictParams.Add(s.ToUpper(), parms[s]);
            }

            if (dictParams.Count == 0) // Ok we didn't launch from ClickOnce
            {
                foreach (string arg in Environment.GetCommandLineArgs())
                {
                    if (arg.Contains("=") == false)
                        continue; // No a parameters that I want...
                    dictParams.Add(arg.Split('=')[0].ToUpper(), arg.Split('=')[1]);
                }
            }
        }

        private NameValueCollection GetQueryStringParameters()
        {
            NameValueCollection nameValueTable = new NameValueCollection();
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                if (ApplicationDeployment.CurrentDeployment == null) return nameValueTable;
                if (ApplicationDeployment.CurrentDeployment.ActivationUri == null) return nameValueTable;
                if (ApplicationDeployment.CurrentDeployment.ActivationUri.Query == null) return nameValueTable;
                string queryString = HttpUtility.UrlDecode(ApplicationDeployment.CurrentDeployment.ActivationUri.Query);
                nameValueTable = HttpUtility.ParseQueryString(queryString);
            }
            return (nameValueTable);
        }
    }
}
