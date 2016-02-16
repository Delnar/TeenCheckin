using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace wcfTeenService.wcf
{
    [ServiceContract]
    public interface ITeenService
    {
        [WebGet(UriTemplate = "/helloworld", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string HelloWorld();

        [OperationContract]
        DataClasses.Teen LoadTeen(int idx);

        [OperationContract]
        List<string> GetDistinctLastName();
        [OperationContract]
        List<string> GetDistinctFirstName();
        [OperationContract]
        List<string> GetDistinctZip();
        [OperationContract]
        List<string> GetDistinctCity();
        [OperationContract]
        void DeleteTeen(int id);
        [OperationContract]
        int GetNextTeen(int id);
        [OperationContract]
        int GetPrevTeen(int id);
        [OperationContract]
        int GetFirstTeen();
        [OperationContract]
        int GetLastTeen();
        [OperationContract]
        int GetRecordsToId(int id);
        [OperationContract]
        int TotalTeenCount();
        [OperationContract]
        DataClasses.TeenReport GetTeenReport();
        [OperationContract]
        List<string> ListGuestsBrought(int id);
        [OperationContract]
        List<KeyValuePair<DataClasses.Teen, int>> ListTop5GuestBringers();
        [OperationContract]
        List<DataClasses.Teen> RealAllTeens();
        [OperationContract]
        DataClasses.Teen SaveTeenRecord(DataClasses.Teen teenData);
        [OperationContract]
        DataClasses.Teen UpdateTeenRecord(DataClasses.Teen teenData);
        [OperationContract]
        List<string> GetDecisionList();
        [OperationContract]
        List<DataClasses.Teen> GetTeenByName(DataClasses.Teen teenData);
        [OperationContract]
        DataClasses.Teen GetTeenByGuestOfKey(string GuestOfKey);

        [OperationContract]
        void DoWork();

        [WebGet(UriTemplate = "/ServerStatus", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        string ServerStatus();

        [WebInvoke(UriTemplate = "/ServerStatus", Method = "OPTIONS", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        void ServerStatusOptions();

        [WebInvoke(UriTemplate = "/PostData", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        string ProcessRecords(List<TeenRecord> lstRecords);

        [WebInvoke(UriTemplate = "/PostData", Method = "OPTIONS", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        void ProcessRecordsOptions();

        [WebInvoke(UriTemplate = "/GetData", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        List<TeenRecord> GetRecords();

        [WebInvoke(UriTemplate = "/GetData", Method = "OPTIONS", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        void GetRecordsOptions();

    }
}