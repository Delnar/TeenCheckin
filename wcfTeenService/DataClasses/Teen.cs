using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace wcfTeenService.DataClasses
{
    [DataContract(Name = "Teen", Namespace = "")]
    public class Teen
    {
        #region Enermation
        public enum DistinctType
        {
            FirstName,
            LastName,
            Zip,
            City
        }
        #endregion
        #region Static Fields
        static public List<DateTime> DateList = new List<DateTime>() {
                new DateTime(2015, 11, 09),
                new DateTime(2015, 11, 10),
                new DateTime(2015, 11, 11),
                new DateTime(2015, 11, 12)
            };
        #endregion
        #region static methods

        static public List<string> GetDistinctList(DistinctType d)
        {
            List<string> ret = new List<string>();
            var Teens = GetAllTeens();
            switch(d)
            {
                case DistinctType.LastName:
                    ret = Teens.Select(x => x.LastName).Distinct().ToList();
                    break;
                case DistinctType.FirstName:
                    ret = Teens.Select(x => x.FirstName).Distinct().ToList();
                    break;
                case DistinctType.Zip:
                    ret = Teens.Select(x => x.zip).Distinct().ToList();
                    break;
                case DistinctType.City:
                    ret = Teens.Select(x => x.City).Distinct().ToList();
                    break;
            }
            
            return ret;
        }
        static public List<Teen> GetAllTeens()
        {
            List<Teen> ret = new List<Teen>();
            using(var db = Code.TeenDataHelper.GetEE())
            {
                var g = db.Teens.ToList();
                g.ForEach(x =>
                {
                    var t = new Teen(x);
                    t.PopulateNightData(x.TeenNights.ToList());
                    ret.Add(t);
                });
            }
            return ret;
        }
        static public Teen GetTeen(int id)
        {
            Teen ret = null;
            using(var db = Code.TeenDataHelper.GetEE())
            {
                var g = db.Teens.Where(x => x.id == id).FirstOrDefault();
                if(g != null)
                {
                    ret = new Teen(g);
                    ret.PopulateNightData(g.TeenNights.ToList());
                }
                    
            }
            return ret;
        }

        static public Teen GetTeenByGuestOfKey(string GuestOfKey)
        {
            Teen ret = null;
            using (var db = Code.TeenDataHelper.GetEE())
            {
                var g = db.Teens.Where(x => x.GuestOfKey == GuestOfKey).FirstOrDefault();
                if (g != null) {
                    ret = new Teen(g);
                }
            }
            return ret;
        }

        static public void DeleteTeen(int id)
        {
            TeenNight.DeleteTeenNights(id);
            using (var db = Code.TeenDataHelper.GetEE())
            {
                var g = db.Teens.Where(x => x.id == id).FirstOrDefault();
                if (g == null) return;
                db.Teens.Remove(g);
                db.SaveChanges();
            }
        }
        static public int GetNextTeenId(int id)
        {
            int ret = -1;
            using (var db = Code.TeenDataHelper.GetEE())
            {
                var recs = db.Teens.Where(x => x.id > id).FirstOrDefault();
                ret = (recs == null) ? db.Teens.Max(x => x.id) : ret = recs.id;
            }
            return ret;
        }
        static public int GetPrevTeenId(int id)
        {
            int ret = -1;
            using (var db = Code.TeenDataHelper.GetEE())
            {
                var recs = db.Teens.Where(x => x.id < id).FirstOrDefault();
                ret = (recs == null) ? db.Teens.Min(x => x.id) : ret = recs.id;
            }
            return ret;
        }
        static public int GetLastTeenId()
        {
            int ret = -1;
            using (var db = Code.TeenDataHelper.GetEE())
            {
                ret = db.Teens.Max(x => x.id);
            }
            return ret;
        }
        static public int GetFirstTeenId()
        {
            int ret = -1;
            using (var db = Code.TeenDataHelper.GetEE())
            {
                ret = db.Teens.Min(x => x.id);
            }
            return ret;
        }
        static public int GetRecordsToId(int id)
        {
            int ret = -1;
            using(var db = Code.TeenDataHelper.GetEE())
            {
                ret = db.Teens.Where(x => x.id <= id).Count();
            }
            return ret;
        }
        static public int TotalTeenCount()
        {
            int ret = -1;
            using(var db = Code.TeenDataHelper.GetEE())
            {
                ret = db.Teens.Count();
            }
            return ret;
        }
        static public TeenReport GetTeenReport()
        {
            TeenReport ret = new TeenReport();
            List<Teen> lstAllteens = GetAllTeens();

            var AllGuests = lstAllteens.Where(x => !string.IsNullOrEmpty(x.GuestOf)).ToList();

            for (int dx = 0; dx < DateList.Count; dx++)
            {
                DateTime d = DateList[dx];
                var AllAttenders = lstAllteens.Where(x => x.LstAttended[dx]).ToList();
                ret.lstNightTotals.Add(AllAttenders.Count());
                ret.lstGuestTotals.Add(AllGuests.Where(x => x.LstAttended[dx]).Count());
                AllGuests.RemoveAll(x => x.LstAttended[dx]);  // Remove the guests from the guest list so they are not double counted.. :)
            }

            return ret;
        }

        static public List<string> GetGuestList(int TeenId)
        {
            List<string> ret = new List<string>();
            List<Teen> lstAllteens = GetAllTeens();
            string GuestOfKey = lstAllteens.Where(x => x.id == TeenId).Select(x=> x.GuestOfKey).FirstOrDefault();
            if (string.IsNullOrEmpty(GuestOfKey)) return ret;
            ret = lstAllteens.Where(x => x.GuestOf == GuestOfKey).Select(x => x.Title).ToList();
            return ret;
        }

        static public List<KeyValuePair<Teen, int>> GetTop5List()
        {
            List<KeyValuePair<Teen, int>> ret = new List<KeyValuePair<Teen, int>>();
            List<Teen> lstAllteens = GetAllTeens();

            var GuestList = lstAllteens.Where(x => !string.IsNullOrEmpty(x.GuestOf))
                                       .GroupBy(x => x.GuestOf)
                                       .Select(grps => new { Key = grps.Key, Count = grps.Count() })
                                       .OrderByDescending(o => o.Count)
                                       .Take(5)
                                       .ToList();

            GuestList.ForEach(x => {
                var TeenRec = lstAllteens.Where(y => y.GuestOfKey == x.Key).FirstOrDefault();
                if (TeenRec == null) return; // Do nothing withou the TeenRec
                ret.Add(new KeyValuePair<Teen, int>(TeenRec, x.Count));
            });

            return ret;
        }

        static public List<Teen> GetTeensByName(string FirstName, string LastName)
        {
            List<Teen> ret = new List<Teen>();
            using (var db = Code.TeenDataHelper.GetEE())
            {
                var g = db.Teens.Where(x=> x.FirstName.ToUpper().StartsWith(FirstName) && x.LastName.ToUpper().StartsWith(LastName)).ToList();
                g.ForEach(x =>
                {
                    var t = new Teen(x);
                    ret.Add(t);
                });
            }
            return ret;
        }

        #endregion

        #region Private Fields
        private int _id = -1;
        private string _FirstName = "";
        private string _LastName = "";
        private string _PhoneArea = "";
        private string _PhonePrefix = "";
        private string _PhonePostfix = "";
        private string _Street1 = "";
        private string _Street2 = "";
        private string _City = "";
        private string _State = "";
        private string _zip = "";
        private string _Sex = "";
        private string _Bus = "";
        private string _Grade = "";
        private string _GuestOf = "";
        private string _GuestOfKey = "";
        private string _GuestOfTitle = "";

        private List<bool> _LstAttended = new List<bool>() { false, false, false, false };
        private List<string> _LstDecision = new List<string>() { "", "", "", "" };        

        #endregion
        #region Public Properties
        [DataMember(Name = "id", Order = 1)]
        public int id 
        {
            get { return _id; }
            set { _id = value; } 
        }
        [DataMember(Name = "FirstName", Order = 2)]
        public string FirstName 
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        [DataMember(Name = "LastName", Order = 3)]
        public string LastName 
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
        [DataMember(Name = "PhoneArea", Order = 4)]
        public string PhoneArea 
        {
            get { return _PhoneArea; }
            set { _PhoneArea = value; } 
        }
        [DataMember(Name = "PhonePrefix", Order = 5)]
        public string PhonePrefix 
        {
            get { return _PhonePrefix; }
            set { _PhonePrefix = value; } 
        }
        [DataMember(Name = "PhonePostfix", Order = 6)]
        public string PhonePostfix 
        {
            get { return _PhonePostfix; }
            set { _PhonePostfix = value; }
        }
        [DataMember(Name = "Street1", Order = 7)]
        public string Street1 
        {
            get { return _Street1; }
            set { _Street1 = value; }
        }
        [DataMember(Name = "Street2", Order = 8)]
        public string Street2 
        {
            get { return _Street2; }
            set { _Street2 = value; }
        }
        [DataMember(Name = "City", Order = 9)]
        public string City 
        {
            get { return _City; }
            set { _City = value; }
        }
        [DataMember(Name = "State", Order = 10)]
        public string State 
        {
            get { return _State; }
            set { _State = value; }
        }
        [DataMember(Name = "zip", Order = 11)]
        public string zip 
        {
            get { return _zip; }
            set { _zip = value; }
        }
        [DataMember(Name = "Sex", Order = 12)]
        public string Sex 
        {
            get { return _Sex; }
            set { _Sex = value; }
        }
        [DataMember(Name = "Bus", Order = 13)]
        public string Bus 
        {
            get { return _Bus; }
            set { _Bus = value; }
        }
        [DataMember(Name = "Grade", Order = 14)]
        public string Grade 
        {
            get { return _Grade; }
            set { _Grade = value; } 
        }
        [DataMember(Name = "GuestOf", Order = 15)]
        public string GuestOf 
        {
            get { return _GuestOf; }
            set { _GuestOf = value; } 
        }
        [DataMember(Name = "GuestOfKey", Order = 16)]
        public string GuestOfKey 
        {
            get { return _GuestOfKey; }
            set { _GuestOfKey = value; } 
        }

        [DataMember(Name = "LstAttended", Order = 17)]
        public List<bool> LstAttended
        {
            get { return _LstAttended; }
            set { _LstAttended = value; }
        }

        [DataMember(Name = "LstDecision", Order = 18)]
        public List<string> LstDecision
        {
            get { return _LstDecision; }
            set { _LstDecision = value; }
        }

        [DataMember(Name = "Title", Order = 19)]
        public string Title
        {
            get {  return string.Format("{0} {1} ({2}) {3}-{4}", FirstName, LastName, PhoneArea, PhonePrefix, PhonePostfix);}
            set { }
        }

        [DataMember(Name = "GuestOfTitle", Order = 19)]
        public string GuestOfTitle
        {
            get { return _GuestOfTitle; }
            set { _GuestOfTitle = value; }
        }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
            set { }
        }

        #endregion
        #region Constructor
        public Teen()
        {

        }
        public Teen(Entity.TeenData.Teen r)
        {
            PopulateFromEE(r);
        }
        #endregion
        #region Methods
        public void PopulateFromEE(Entity.TeenData.Teen r)
        {
            id = r.id;
            FirstName = r.FirstName;
            LastName = r.LastName;
            PhoneArea = r.PhoneArea;
            PhonePrefix = r.PhonePrefix;
            PhonePostfix = r.PhonePostfix;
            Street1 = r.Street1;
            Street2 = r.Street2;
            City = r.City;
            State = r.State;
            zip = r.zip;
            Sex = r.Sex;
            Bus = r.Bus;
            Grade = r.Grade;
            GuestOf = r.GuestOf;
            GuestOfKey = r.GuestOfKey;
        }
        public Entity.TeenData.Teen ToEE()
        {
            Entity.TeenData.Teen r = new Entity.TeenData.Teen();
            r.id = id;
            r.FirstName = FirstName;
            r.LastName = LastName;
            r.PhoneArea = PhoneArea;
            r.PhonePrefix = PhonePrefix;
            r.PhonePostfix = PhonePostfix;
            r.Street1 = Street1;
            r.Street2 = Street2;
            r.City = City;
            r.State = State;
            r.zip = zip;
            r.Sex = Sex;
            r.Bus = Bus;
            r.Grade = Grade;
            r.GuestOf = GuestOf ?? "";
            r.GuestOfKey = GuestOfKey ?? "";
            return r;
        }
        public void PopulateNightData(List<Entity.TeenData.TeenNight> TeenNightList)
        {
            LstAttended = new List<bool>();
            LstDecision = new List<string>();

            for (int dx = 0; dx < DateList.Count; dx++)
            {
                DateTime d = DateList[dx];
                var dRec = TeenNightList.Where(x => x.AttendanceDate.Date.Equals(d.Date)).FirstOrDefault();
                if (dRec == null)
                {
                    LstDecision.Add("");
                    LstAttended.Add(false);
                    continue;
                }
                LstDecision.Add(dRec.Decision);
                LstAttended.Add(dRec.Attended.Trim().ToUpper() == "Y");
            }

        }

        public Teen SaveRecord()
        {
            using (var db = Code.TeenDataHelper.GetEE())
            {
                try
                {
                    var ee = ToEE();
                    db.Teens.Add(ee);
                    db.SaveChanges();
                    this.id = ee.id;
                }
                catch (Exception ex)
                {
                    Code.LogMan.Log(ex);
                    throw;
                }

            }

            StoreAttendance();

            return this;
        }

        public void StoreAttendance()
        {
            for (int x = 0; x < LstAttended.Count; x++)
            {
                var tnd = TeenNight.ReadSingleTeenNights(this.id, DateList[x]);
                if (tnd == null)
                {
                    tnd = new TeenNight();
                    tnd.Attended = LstAttended[x] ? "Y" : "N";
                    tnd.Decision = LstDecision[x];
                    tnd.AttendanceDate = DateList[x];
                    tnd.TeenId = id;
                    tnd.Save();
                }
                else
                {
                    tnd.Attended = LstAttended[x] ? "Y" : "N";
                    tnd.Decision = LstDecision[x];
                    tnd.Update();
                }
            }
        }

        public Teen UpdateRecord()
        {
            using (var db = Code.TeenDataHelper.GetEE())
            {
                try
                {
                    var ee = ToEE();
                    var t = db.Teens.Attach(ee);
                    var Entry = db.Entry(t);
                    Entry.State = System.Data.Entity.EntityState.Modified;
                    Entry.Property(e => e.GuestOfKey).IsModified = false;
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    Code.LogMan.Log(ex);
                    throw;
                }
            }

            StoreAttendance();

            return this;
        }

        #endregion

    }
}