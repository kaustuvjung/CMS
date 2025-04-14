using DataAccess.InterFaces;
using DataAccess.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tset.Models
{
    public class Configurations
    {
        private static GeneralSetting _generalSetting;
        public static GeneralSetting GeneralSettings
        {

            get
            {
                return _generalSetting ??
                    (Helper.DI.Instance.Resolve<ISetupRepository>().GetCompanyInfo().Result);
            }
        }
    }
}
