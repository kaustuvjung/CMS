using DataAccess.InterFaces;
using DataAccess.Model.Common;
using DataAccess.Model.setup;
using Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        #region common
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

        #endregion

        public async Task<DataTable> GetSoftware(int skip, int take, string filter)
        {
            string qFilter = string.Empty;
            if (!string.IsNullOrEmpty(filter))
                qFilter = filter.BuildSQLFilter();
            var p = new List<SqlParameter>()
            {
                new SqlParameter("@Skip", skip),
                new SqlParameter("@take", take),
                new SqlParameter("@Filter", qFilter)
            };
            var q = @" select * from dbo.Software where IsDeleted = 0";
             var data =  await db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return data;
        }

        public async  Task<int> GetSoftwareCount(string filter)
        {
            string qFilter = string.Empty;
            if (!string.IsNullOrEmpty(filter))
                qFilter = filter.BuildSQLFilter();
        
            var q = @" select count(*) from dbo.Software where IsDeleted = 0";
            if (!string.IsNullOrEmpty(qFilter))
                q += " And " + qFilter;
            var data = await db.ExecuteScalarAsync(CommandType.Text, q);
            return (int)data.Response;
        }
        public async Task<DataTable> SofteareById(int id)
        {
            var p = new List<SqlParameter>()
            {
                new SqlParameter("@Id", id)
            };
            var q = @" select * from dbo.Software where Id = @Id";
            var data = await db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return data;
        }
        public  async Task<DbResponse> SaveSoftware(Software model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["Software_save"];
            var dbResponse = await db.ExecuteScalarAsync(CommandType.Text, q, p);
            if (!dbResponse.HasError)
                model.Id = (int)dbResponse.Response;
            return dbResponse;
        }
        public async Task<DbResponse> DeleteSoftware(int id)
        {
            var p = new List<SqlParameter>()
            {
                new SqlParameter("@Id", id)
            };
            var q = @"Update dbo.Software set IsDeleted = 1 where Id = @Id";
            var dbResponse = await db.ExecuteNonQueryAsync(CommandType.Text, q, p);
         
            return dbResponse;
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
