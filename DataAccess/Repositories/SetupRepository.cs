using DataAccess.InterFaces;
using DataAccess.Model.Common;
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

        public async Task<GeneralSetting> GetCompanyInfo()
        {
            var q = @"Select * From [dbo].[GeneralSetting]";
            var data = await db.ExecuteDataTableAsync(CommandType.Text, q);
            var result = data.TransformToObject<GeneralSetting>();
            return result;
        }



        public void Dispose()
        {
            db.Dispose();
        }
    }
}
