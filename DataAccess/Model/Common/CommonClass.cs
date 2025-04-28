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
    public class Province
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class District
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public int ProvinceId { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class LocalBody
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public int DistrictId { get; set; }
        public bool IsMunicipality { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class Ward
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public int DisplayOrder { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Module { get; set; }
    }

    public class Vdc
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
