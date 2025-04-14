using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model.Common
{
    public class GeneralSetting
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle1 { get; set; }
        public string SubTitle2 { get; set; }
        public string SubTitle3 { get; set; }
        public string ProductName { get; set; }
        public string ProductNameNp { get; set; }
        public string Footer { get; set; }
        public string LandAreaUnit { get; set; }
        public string MeasurementUnit { get; set; }
        public string LandAreaMeasurementUnit { get; set; }
        public string SetBackUnit { get; set; }
        public string RowOpenSpaceUnit { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

}
