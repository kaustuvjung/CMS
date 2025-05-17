using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class SoftwareUser
    {
        public int Id { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int LocalBodyId { get; set; }
        public int SoftwareId { get; set; }
        
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate{ get; set;}

    }
}
