using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace wcfTeenService.DataClasses
{
    [DataContract(Name = "TeenNight", Namespace = "")]
    public class TeenNight
    {
        #region Static Methods
        static public List<TeenNight> ReadTeenNights(int TeenId)
        {
            List<TeenNight> ret = new List<TeenNight>();
            using (var db = Code.TeenDataHelper.GetEE())
            {
                var g = db.TeenNights.Where(x => x.TeenId == TeenId).ToList();
                g.ForEach(x =>
                {
                    ret.Add(new TeenNight(x));
                });
            }
            return ret;
        }

        static public TeenNight ReadSingleTeenNights(int TeenId, DateTime dt)
        {
            List<TeenNight> allNights = ReadTeenNights(TeenId);
            return allNights.Where(x => x.AttendanceDate.Date.Equals(dt.Date)).FirstOrDefault();
        }

        static public void DeleteTeenNights(int TeenId)
        {
            using (var db = Code.TeenDataHelper.GetEE())
            {
                var g = db.TeenNights.Where(x => x.TeenId == TeenId).ToList();
                if (g.Count == 0) return;
                db.TeenNights.RemoveRange(g);
                db.SaveChanges();
            }
        }

        static public List<string> GetDecisionList()
        {
            List<string> ret = new List<string>();
            using (var db= Code.TeenDataHelper.GetEE())
            {
                ret.AddRange(db.TeenNights.Select(x => x.Decision).Distinct());
            }
            return ret;
        }        
        #endregion
        #region Private Fields

        private int _id = -1;
        private int _TeenId = -1;
        private System.DateTime _AttendanceDate = DateTime.Now;
        private string _Decision = "";
        private string _Attended = "N";

        #endregion
        #region Public Properties
        [DataMember(Name = "id", Order = 1)]
        public int id 
        {
            get { return _id; }
            set { _id = value; }
        }
        [DataMember(Name = "TeenId", Order = 2)]
        public int TeenId 
        {
            get { return _TeenId; }
            set { _TeenId = value; } 
        }
        [DataMember(Name = "AttendanceDate ", Order = 3)]
        public System.DateTime AttendanceDate 
        {
            get { return _AttendanceDate; }
            set { _AttendanceDate = value; } 
        }
        [DataMember(Name = "Decision", Order = 4)]
        public string Decision 
        {
            get { return _Decision; }
            set { _Decision = value; }
        }
        [DataMember(Name = "Attended", Order = 5)]
        public string Attended 
        {
            get { return _Attended; }
            set { _Attended = value; } 
        }

        #endregion
        #region Constructor
        public TeenNight()
        {

        }

        public TeenNight(Entity.TeenData.TeenNight r)
        {
            PopulateFromEE(r);
        }
        #endregion
        #region Methods
        public void PopulateFromEE(Entity.TeenData.TeenNight r)
        {
            id = r.id;
            TeenId = r.TeenId;
            AttendanceDate = r.AttendanceDate;
            Decision = r.Decision;
            Attended = r.Attended;
        }

        public Entity.TeenData.TeenNight ToEE()
        {
            Entity.TeenData.TeenNight r = new Entity.TeenData.TeenNight();

            r.id = id;
            r.TeenId = TeenId;
            r.AttendanceDate = AttendanceDate;
            r.Decision = Decision;
            r.Attended = Attended;

            return r;
        }

        public TeenNight Save()
        {
            using (var db = Code.TeenDataHelper.GetEE())
            {
                try
                {
                    var ee = ToEE();
                    db.TeenNights.Add(ee);
                    db.SaveChanges();
                    this.id = ee.id;
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            string ErrorMsg = string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                            throw new Exception(ErrorMsg);
                        }
                    }
                    throw;
                }
            }
            return this;
        }

        public TeenNight Update()
        {
            using (var db = Code.TeenDataHelper.GetEE())
            {
                try
                {
                    var ee = ToEE();
                    var t = db.TeenNights.Attach(ee);
                    var Entry = db.Entry(t);
                    Entry.State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            string ErrorMsg = string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                            throw new Exception(ErrorMsg);
                        }
                    }
                    throw;
                }
            }
            return this;
        }
        #endregion

    }
}