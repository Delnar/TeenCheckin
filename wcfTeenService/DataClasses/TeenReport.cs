using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace wcfTeenService.DataClasses
{
    [DataContract(Name = "TeenReport", Namespace = "")]
    public class TeenReport
    {
        #region Private Fields
        private List<int> _lstNightTotals = new List<int>();
        private List<int> _lstGuestTotals = new List<int>();
        #endregion
        #region Public Properties
        [DataMember(Name = "lstNightTotals", Order = 1)]
        public List<int> lstNightTotals
        {
            get { return _lstNightTotals; }
            set { _lstNightTotals = value; }
        }
        [DataMember(Name = "lstGuestTotals", Order = 2)]
        public List<int> lstGuestTotals
        {
            get { return _lstGuestTotals; }
            set { _lstGuestTotals = value; }
        }
        [DataMember(Name = "TeenTotal", Order = 3)]
        public int TeenTotal
        {
            get { return _lstNightTotals.Sum(); }
            set { }
        }
        [DataMember(Name = "GuestTotal", Order = 4)]
        public int GuestTotal
        {
            get { return _lstGuestTotals.Sum(); }
            set { }
        }         
        #endregion
        #region Constructor
        public TeenReport()
        {

        }
        #endregion
        #region Methods
        #endregion

    }
}