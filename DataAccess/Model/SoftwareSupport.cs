using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class SoftwareSupport
    {
        public int Id { get; set; } 
        public int SoftwareUserId { get; set; } 
        public string SupportRemarks { get; set; }
        private string _miti;
        [NotMapped]
        public string Miti
        {
            get {
                return _miti ?? DateMiti.GetDateMiti.GetMiti(Date, "-");
            }
            set {
                _miti = value;
                _date =  DateMiti.GetDateMiti.GetDate(value);
            }
        }
        private DateTime _date { get; set; }
        public DateTime Date
        {
            get
            {
                return _date == DateTime.MinValue ? DateTime.Now : _date;
            }
            set
            {
                _miti = DateMiti.GetDateMiti.GetMiti(value);   
            }
        }


        public string SupportReqBy{ get; set; } 
        public int SupportedBy { get; set; } 
        public int SoftwareId { get; set; } 
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
