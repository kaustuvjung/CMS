using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model.Common;
using DataAccess.Model.setup;
using Helper;

namespace DataAccess.InterFaces
{
    public  interface ISetupRepository: IDisposable
    {
        Task<DataTable> GetSoftware(int skip, int take, string filter);
        Task<int> GetSoftwareCount(string filter);
        Task<DataTable> SofteareById(int id);
         Task<DbResponse> SaveSoftware(Software model);
        Task<DbResponse> DeleteSoftware(int id);
        #region Common Repositry
        Task<List<District>> GetDistrictAsync();
        Task<List<Province>> GetProvincesAsync();
        Task<List<LocalBody>> GetLocalBodyAsync();
        Task<GeneralSetting> GetCompanyInfo();
        Task<List<Department>> GetDepartments();
        Task<List<Ward>> GetWards();
        Task<List<Permission>> GetPermissions();
        #endregion
    }
}
