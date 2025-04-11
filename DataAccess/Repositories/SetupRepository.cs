using DataAccess.InterFaces;
using System;
using System.Collections.Generic;
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




        public void Dispose()
        {
            db.Dispose();
        }
    }
}
