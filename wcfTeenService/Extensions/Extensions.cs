using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace wcfTeenService.wcf.Extensions
{
    static public class Extensions
    {
        static public void Insert(this List<TeenRecord> r)
        {
            try
            {
                using (var db = Code.TeenDataHelper.GetEE())
                {
                    r.ForEach(x =>
                    {
                        x.Insert(db);
                        x.NewRec = "N";
                        x.UpdateRec = "N";
                    });

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Code.LogMan.Log(ex);
                throw;
            }
        }

        static public void Update(this List<TeenRecord> r)
        {
            try
            {
                using (var db = Code.TeenDataHelper.GetEE())
                {
                    r.ForEach(rec =>
                    {
                        var dbRec = db.Teens.Where(x => x.id == rec.PublicRecId).FirstOrDefault();
                        if (dbRec == null) return;

                        dbRec.Bus = rec.Bus;
                        dbRec.City = rec.City;
                        dbRec.FirstName = rec.FirstName;
                        dbRec.Grade = rec.Grade;
                        dbRec.GuestOf = rec.GuestOf;
                        dbRec.LastName = rec.LastName;
                        dbRec.PhoneArea = rec.PhoneArea;
                        dbRec.PhonePostfix = rec.PhoneSuffix;
                        dbRec.PhonePrefix = rec.PhonePrefix;
                        dbRec.Sex = rec.Sex;
                        dbRec.State = rec.State;
                        dbRec.Street1 = rec.Street1;
                        dbRec.Street2 = rec.Street2;
                        dbRec.zip = rec.Zip;
                        
                        db.SaveChanges();
                    });
                }

            }
            catch (Exception ex)
            {
                Code.LogMan.Log(ex);
                throw;
            }
        }

        static public void FillFromDataBase(this List<TeenRecord> r)
        {
            try
            {
                using (var db = Code.TeenDataHelper.GetEE())
                {
                    var TeenRecs = db.Teens.ToList(); // Read all the records from the DB 
                    if (TeenRecs.Count <= 0) return;
                    TeenRecs.ForEach(rec =>
                    {
                        TeenRecord e = new TeenRecord();
                        e.Bus = rec.Bus;
                        e.City = rec.City;
                        e.CheckIn = "N";
                        e.Dirty = "N";
                        e.FirstName = rec.FirstName;
                        e.Grade = rec.Grade;
                        e.GuestOf = rec.GuestOf;
                        e.GuestOfKey = rec.GuestOfKey;
                        e.LastName = rec.LastName;
                        e.NewRec = "N";
                        e.PhoneArea = rec.PhoneArea;
                        e.PhonePrefix = rec.PhonePrefix;
                        e.PhoneSuffix = rec.PhonePostfix;
                        e.PrivateRecId = -1;
                        e.PublicRecId = rec.id;
                        e.Sex = rec.Sex;
                        e.State = rec.State;
                        e.Street1 = rec.Street1;
                        e.Street2 = rec.Street2;
                        e.UpdateCheckin = "N";
                        e.UpdateRec = "N";
                        e.Zip = rec.zip;

                        var NightRec = rec.TeenNights.Where(x => x.AttendanceDate.Date == DateTime.Now.Date).FirstOrDefault();

                        if(NightRec != null)
                        {
                            e.CheckIn = NightRec.Attended;
                        }

                        r.Add(e);
                    });
                }
            }
            catch (Exception ex)
            {
                Code.LogMan.Log(ex);
                throw;
            }
        }

        static public void UpdateCheckIn(this List<TeenRecord> r)
        {
            try
            {
                using (var db = Code.TeenDataHelper.GetEE())
                {
                    r.ForEach(rec =>
                    {
                        Entity.TeenData.Teen dbRec = null;
                        if(rec.PublicRecId == -1)
                        {
                            dbRec = db.Teens.Where(x => x.GuestOfKey == rec.GuestOfKey).FirstOrDefault();
                        }
                        else
                        {
                            dbRec = db.Teens.Where(x => x.id == rec.PublicRecId).FirstOrDefault();
                        }

                        if (dbRec == null) return;

                        Entity.TeenData.TeenNight NightRec = dbRec.TeenNights.Where(x => x.AttendanceDate.Date == DateTime.Now.Date).FirstOrDefault();

                        if (NightRec != null)
                        {
                            NightRec.Attended = rec.CheckIn;
                        }
                        else
                        {
                            NightRec = new Entity.TeenData.TeenNight();
                            NightRec.AttendanceDate = DateTime.Today;
                            NightRec.Attended = rec.CheckIn;
                            NightRec.TeenId = dbRec.id;
                            dbRec.TeenNights.Add(NightRec);
                        }
                    });
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Code.LogMan.Log(ex);
                throw;
            }
        }
    }
}