using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wcfTeenService.wcf.Extensions;
using System.Configuration;
using System.Web;
using System.ServiceModel.Web;

namespace wcfTeenService.wcf
{
    public class TeenService : ITeenService
    {
        public TeenService()
        {
            // Code.TeenDataHelper.ServerAddress = "james-pc";
            // Code.TeenDataHelper.UserName = "TeenApp";
            // Code.TeenDataHelper.Password = "passw0rd";
            Code.LogMan.Initialize("log.txt");
            Settings.LoadSettings();
        }
        public string HelloWorld()
        {
            BypassCrossDomain();
            return "Hello World";
        }

        public DataClasses.Teen LoadTeen(int idx)
        {
            BypassCrossDomain();
            return DataClasses.Teen.GetTeen(idx);
        }

        public List<string> GetDistinctLastName()
        {
            BypassCrossDomain();
            return DataClasses.Teen.GetDistinctList(DataClasses.Teen.DistinctType.LastName);
        }

        public List<string> GetDistinctFirstName()
        {
            BypassCrossDomain();
            return DataClasses.Teen.GetDistinctList(DataClasses.Teen.DistinctType.FirstName);
        }

        public List<string> GetDistinctZip()
        {
            BypassCrossDomain();
            return DataClasses.Teen.GetDistinctList(DataClasses.Teen.DistinctType.Zip);
        }

        public List<string> GetDistinctCity()
        {
            BypassCrossDomain();
            return DataClasses.Teen.GetDistinctList(DataClasses.Teen.DistinctType.City);
        }

        public void DeleteTeen(int id)
        {
            BypassCrossDomain();
            DataClasses.Teen.DeleteTeen(id);
        }

        public int GetNextTeen(int id)
        {
            BypassCrossDomain();
            return DataClasses.Teen.GetNextTeenId(id);
        }

        public int GetPrevTeen(int id)
        {
            BypassCrossDomain();
            return DataClasses.Teen.GetPrevTeenId(id);
        }

        public int GetFirstTeen()
        {
            BypassCrossDomain();
            return DataClasses.Teen.GetFirstTeenId();
        }

        public int GetLastTeen()
        {
            BypassCrossDomain();
            return DataClasses.Teen.GetLastTeenId();
        }


        public int GetRecordsToId(int id)
        {
            BypassCrossDomain();
            return DataClasses.Teen.GetRecordsToId(id);
        }

        public int TotalTeenCount()
        {
            BypassCrossDomain();
            return DataClasses.Teen.TotalTeenCount();
        }


        public DataClasses.TeenReport GetTeenReport()
        {
            BypassCrossDomain();
            return DataClasses.Teen.GetTeenReport();
        }


        public List<string> ListGuestsBrought(int id)
        {
            BypassCrossDomain();
            return DataClasses.Teen.GetGuestList(id);
        }


        public List<KeyValuePair<DataClasses.Teen, int>> ListTop5GuestBringers()
        {
            BypassCrossDomain();
            return DataClasses.Teen.GetTop5List();
        }


        public List<DataClasses.Teen> RealAllTeens()
        {
            BypassCrossDomain();
            return DataClasses.Teen.GetAllTeens();
        }


        public DataClasses.Teen SaveTeenRecord(DataClasses.Teen teenData)
        {
            BypassCrossDomain();
            return teenData.SaveRecord();
        }

        public DataClasses.Teen UpdateTeenRecord(DataClasses.Teen teenData)
        {
            BypassCrossDomain();
            return teenData.UpdateRecord();
        }

        public List<string> GetDecisionList()
        {
            BypassCrossDomain();
            return DataClasses.TeenNight.GetDecisionList();
        }

        public List<DataClasses.Teen> GetTeenByName(DataClasses.Teen teenData)
        {
            BypassCrossDomain();
            return DataClasses.Teen.GetTeensByName(teenData.FirstName, teenData.LastName);
        }


        public DataClasses.Teen GetTeenByGuestOfKey(string GuestOfKey)
        {
            BypassCrossDomain();
            return DataClasses.Teen.GetTeenByGuestOfKey(GuestOfKey);
        }

        public void DoWork()
        {
        }


        public string ServerStatus()
        {
            BypassCrossDomain();
            return string.Format("GOOD");
        }


        public string ProcessRecords(List<TeenRecord> lstRecords)
        {
            BypassCrossDomain();
            if (lstRecords == null) return "GOOD";
            if (lstRecords.Count <= 0) return "GOOD"; // Nothing to process?

            lstRecords.Where(r => r.NewRec == "Y").ToList().Insert();
            lstRecords.Where(r => r.UpdateRec == "Y").ToList().Update();
            lstRecords.Where(r => r.UpdateCheckin == "Y").ToList().UpdateCheckIn();
            return string.Format("GOOD");
        }


        public List<TeenRecord> GetRecords()
        {
            BypassCrossDomain();
            List<TeenRecord> recs = new List<TeenRecord>();
            recs.FillFromDataBase();
            return recs;
        }

        private void BypassCrossDomain()
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, Key");
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "*");
        }

        public void ProcessRecordsOptions()
        {
            BypassCrossDomain();
        }

        public void GetRecordsOptions()
        {
            BypassCrossDomain();
        }

        public void ServerStatusOptions()
        {
            BypassCrossDomain();
        }
    }
}