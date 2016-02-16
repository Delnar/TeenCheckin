using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace wcfTeenService.Code
{
    public class TeenDataHelper
    {
        static private string _ServerAddress = "laptop4";
        static private string _UserName = "sa";
        static private string _Password = "passw0rd";
        static public string ServerAddress 
        {
            get { return _ServerAddress; }
            set { _ServerAddress = value; }
        }

        static public string UserName 
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        static public string Password 
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private string EntityConnStr { get; set; }

        static public Entity.TeenData.TeenDB GetEE(string ConStr = "")
        {
            return new Entity.TeenData.TeenDB((new TeenDataHelper()).ToString());
        }

        public TeenDataHelper()
        {
            StringBuilder sb = new StringBuilder();
            string ModelPath = "Entity.TeenData.TeensDB";

            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder();
            csb.ApplicationName = "EntityFrameWork";
            csb.InitialCatalog = "TeenData";
            csb.Password = Password;
            csb.UserID = UserName;
            csb.IntegratedSecurity = false;
            csb.DataSource = ServerAddress;

            sb.Append("metadata=");
            sb.Append("res://*/"); sb.Append(ModelPath); sb.Append(".csdl|");
            sb.Append("res://*/"); sb.Append(ModelPath); sb.Append(".ssdl|");
            sb.Append("res://*/"); sb.Append(ModelPath); sb.Append(".msl;");
            sb.Append("provider=System.Data.SqlClient;");
            sb.Append("provider connection string=\"");
            sb.Append(csb.ToString());
            sb.Append(";MultipleActiveResultSets=True;");
            sb.Append("App=EntityFramework\"");

            EntityConnStr = sb.ToString();
        }

        public override string ToString()
        {
            return EntityConnStr;
        }
    }
}