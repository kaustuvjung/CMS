using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model.Common;
using DataAccess.Model.setup;

namespace DataAccess.InterFaces
{
    public  interface ISetupRepository: IDisposable
    {
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
