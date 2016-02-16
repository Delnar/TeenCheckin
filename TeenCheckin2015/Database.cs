using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace TeenCheckin
{
    public class Database
    {
        static public TeenProxy.wcfTeens.Teen LoadTeenRecord(int idx)
        {            
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();            
            return Proxy.LoadTeen(idx);
        }

        static public List<ListItem> LoadDecisions()
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            var ListOfDecisions = Proxy.GetDecisionList().ToList();
            List<ListItem> ret = new List<ListItem>();
            ret.Add(new ListItem(0,"Salvation"));
            ListOfDecisions.ForEach(x =>
            {
                if (string.IsNullOrEmpty(x)) return;
                if (x.ToUpper() == "SALVATION") return;
                ret.Add(new ListItem(0, x));
            });
            return ret;
        }

        static public TeenProxy.wcfTeens.Teen GetGuestOfDataSet(string GuestOfKey)
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.GetTeenByGuestOfKey(GuestOfKey);
        }

        static public List<TeenProxy.wcfTeens.Teen> GetTeenListForUpdateDataSet(string FirstName, string LastName)
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            var TeenRec = new TeenProxy.wcfTeens.Teen() { FirstName = FirstName, LastName = LastName };
            return Proxy.GetTeenByName(TeenRec).ToList();
        }

        static public TeenProxy.wcfTeens.Teen SaveRecord(TeenProxy.wcfTeens.Teen teenRec)
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.SaveTeenRecord(teenRec);
        }

        static public TeenProxy.wcfTeens.Teen UpdateRecord(TeenProxy.wcfTeens.Teen teenRec)
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.UpdateTeenRecord(teenRec);
        }


        static public List<TeenProxy.wcfTeens.Teen> GetAllTeens()
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.RealAllTeens().ToList(); 
        }

        static public List<KeyValuePair<TeenProxy.wcfTeens.Teen, int>> GetTopFiveList()
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.ListTop5GuestBringers().ToList(); 
        }

        static public List<string> GetListGuestBrought(int idx)
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.ListGuestsBrought(idx).ToList(); 
        }

        static public TeenProxy.wcfTeens.TeenReport GetTeenData()
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.GetTeenReport(); 
        }

        static public int GetTotalRecords()
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.TotalTeenCount();
        }

        static public int GetRecordsToIdx(int idx)
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.GetRecordsToId(idx);
        }

        static public int GetFirstIdx()
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.GetFirstTeen();
        }

        static public int GetLastIdx()
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.GetLastTeen();
        }

        static public int GetPrevIdx(int idx)
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.GetPrevTeen(idx);
        }

        static public int GetNextIdx(int idx)
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.GetNextTeen(idx);
        }

        static public void DeleteRecord(int idx)
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            Proxy.DeleteTeen(idx);
        }

        static public List<string> GetDistinctCityList()
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.GetDistinctCity().ToList();
        }

        static public List<string> GetDistinctZipList()
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.GetDistinctZip().ToList();
        }

        static public List<string> GetDistinctFirstList()
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.GetDistinctFirstName().ToList();
        }

        static public List<string> GetDistinctLastList()
        {
            var Proxy = TeenProxy.ClientProxy.CreateTeenClient();
            return Proxy.GetDistinctLastName().ToList();
        }

        static public List<string> GetAddressHTTP()
        {
#warning GeoCoding Removed
            return new List<string>();
/*            string cmdStr = "sp_readAddressList";

            List<SqlParam> lstParams = null;

            DataSet ds = ExecuteDataSet(cmdStr, lstParams);

            List<string> ret = new List<string>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ret.Add(dr["addressCode"].ToString());
            }

            return ret;*/
        }

        static public void ClearGeoCode()
        {
#warning GeoCoding Removed...
//            string cmdStr = "sp_clearGeocode";
//            ExecuteSql(cmdStr);
        }
    }

    public class SqlParam
    {

        static public SqlParam Create(string id, object value)
        {
            return new SqlParam(id, value);
        }

        public string id;
        public object value;



        public SqlParam()
        {
            id = "";
            value = null;
        }

        public SqlParam(string id, object value)
        {
            this.id = id;
            this.value = value;
        }

        public SqlDbType GetParamType()
        {
            SqlDbType myType = SqlDbType.VarChar;

            if (value.GetType() == typeof(string)) myType = SqlDbType.VarChar;
            if (value.GetType() == typeof(int)) myType = SqlDbType.Int;
            if (value.GetType() == typeof(DateTime)) myType = SqlDbType.DateTime;
            if (value.GetType() == typeof(string)) myType = SqlDbType.VarChar;
            if (value.GetType() == typeof(string)) myType = SqlDbType.VarChar;
            if (value.GetType() == typeof(string)) myType = SqlDbType.VarChar;

            return myType;
        }
    }


}
