using DataAccess.InterFaces;
using DataAccess.Model.Common;
using DataAccess.Model.setup;
using Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class SetupRepository: ISetupRepository, IDisposable
    {
        private readonly DatabaseContext db;
        public SetupRepository(DatabaseContext context)
        {
            this.db = context;
        }

        public async Task<List<Province>> GetProvincesAsync()
        {
            var q = @"Select Id, Name, NameNp,DisplayOrder from Setup.Province Order BY DisplayOrder";
            var data = await db.ExecuteDataTableAsync(CommandType.Text, q);
            return data.TransformToList<Province>().ToList();
        }
        public async Task<List<District>> GetDistrictAsync()
        {
            var q = @"Select Id, Name, NameNp, ProvinceId, DisplayOrder from Setup.District Order BY DisplayOrder";
            var data = await db.ExecuteDataTableAsync(CommandType.Text, q);
            return data.TransformToList<District>().ToList();
        }
        public async Task<List<LocalBody>> GetLocalBodyAsync()
        {
            var q = @"Select Id, Name , NameNp, DistrictId, IsMunicipality,DisplayOrder from Setup.LocalBody Order by  DisplayOrder ";
            var data  =  await db.ExecuteDataTableAsync(CommandType.Text, q);
            return data.TransformToList<LocalBody>().ToList();
        }
        public async Task<List<Ward>> GetWards()
        {
            var q = @"select Id, Name , NameNp from Setup.Ward";
            var data =  await db.ExecuteDataTableAsync(CommandType.Text, q);
            return data.TransformToList<Ward>().ToList();
        }
        public async Task<List<Department>> GetDepartments()
        {
            var q = @"Select * from Setup.Department order By DisplayOrder";
            var data = await db.ExecuteDataTableAsync(CommandType.Text, q);
            return data.TransformToList<Department>().ToList();
        }
        public async Task<GeneralSetting> GetCompanyInfo()
        {
            var q = @"Select * From [dbo].[GeneralSetting]";
            var data = await db.ExecuteDataTableAsync(CommandType.Text, q);
            var result = data.TransformToObject<GeneralSetting>();
            return result;
        }

        public async Task<List<Permission>> GetPermissions()
        {
            var q = @"Select Id, Name , DisplayName, Module  from dbo.Permission";
            var data = await db.ExecuteDataTableAsync(CommandType.Text, q);
            return data.TransformToList<Permission>().ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
