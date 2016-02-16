using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace wcfTeenService.wcf
{
    [DataContract(Name = "TeenRecord", Namespace = "")]
    public class TeenRecord
    {
        [DataMember(Name = "PublicRecId", Order = 1)]
        public int PublicRecId { get; set; }
        [DataMember(Name = "PrivateRecId", Order = 2)]
        public int PrivateRecId { get; set; }
        [DataMember(Name = "FirstName", Order = 3)]
        public string FirstName { get; set; }
        [DataMember(Name = "LastName", Order = 4)]
        public string LastName { get; set; }
        [DataMember(Name = "PhoneArea", Order = 5)]
        public string PhoneArea { get; set; }
        [DataMember(Name = "PhonePrefix", Order = 6)]
        public string PhonePrefix { get; set; }
        [DataMember(Name = "PhoneSuffix", Order = 7)]
        public string PhoneSuffix { get; set; }
        [DataMember(Name = "Street1", Order = 8)]
        public string Street1 { get; set; }
        [DataMember(Name = "Street2", Order = 9)]
        public string Street2 { get; set; }
        [DataMember(Name = "City", Order = 10)]
        public string City { get; set; }
        [DataMember(Name = "State", Order = 11)]
        public string State { get; set; }
        [DataMember(Name = "Zip", Order = 12)]
        public string Zip { get; set; }
        [DataMember(Name = "Sex", Order = 13)]
        public string Sex { get; set; }
        [DataMember(Name = "Bus", Order = 14)]
        public string Bus { get; set; }
        [DataMember(Name = "Grade", Order = 15)]
        public string Grade { get; set; }
        [DataMember(Name = "GuestOf", Order = 16)]
        public string GuestOf { get; set; }
        [DataMember(Name = "CheckIn", Order = 17)]
        public string CheckIn { get; set; }
        [DataMember(Name = "Dirty", Order = 18)]
        public string Dirty { get; set; }
        [DataMember(Name = "NewRec", Order = 18)]
        public string NewRec { get; set; }
        [DataMember(Name = "UpdateRec", Order = 18)]
        public string UpdateRec { get; set; }
        [DataMember(Name = "UpdateCheckin", Order = 18)]
        public string UpdateCheckin { get; set; }
        [DataMember(Name = "GuestOfKey", Order = 18)]
        public string GuestOfKey { get; set; }

        public TeenRecord()
        {
            PublicRecId = -1;
            PrivateRecId = -1;
            FirstName = "";
            LastName = "";
            PhoneArea = "";
            PhonePrefix = "";
            PhoneSuffix = "";
            Street1 = "";
            Street2 = "";
            City = "";
            State = "";
            Zip = "";
            Sex = "";
            Bus = "";
            Grade = "";
            GuestOf = "";
            CheckIn = "";
            Dirty = "";
            NewRec = "";
            UpdateRec = "";
            UpdateCheckin = "";
            GuestOfKey = "";
        }

        public void Insert(Entity.TeenData.TeenDB db)
        {
            var rec = new Entity.TeenData.Teen()
            {
                FirstName = FirstName,
                LastName = LastName,
                PhoneArea = PhoneArea,
                PhonePrefix = PhonePrefix,
                PhonePostfix = PhoneSuffix,
                Street1 = Street1,
                Street2 = Street2,
                City = City, 
                State = State,
                zip = Zip,
                Sex = Sex,
                Bus = Bus,
                Grade = Grade,                
                GuestOf = GuestOf,
                GuestOfKey = GuestOfKey
            };
            
            db.Teens.Add(rec);
        }

        public void Update()
        {
            // var rec = new Entity.TeenData.Teen()
            {

            }
        }
    }


}