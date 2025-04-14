using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model.Common;

namespace DataAccess.InterFaces
{
    public  interface ISetupRepository: IDisposable
    {
        #region Common Repositry

        Task<GeneralSetting> GetCompanyInfo();
        #endregion
    }
}
