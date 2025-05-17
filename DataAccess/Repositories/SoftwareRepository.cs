using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.InterFaces;

namespace DataAccess.Repositories
{
    public class SoftwareRepository : ISoftwareRepository, IDisposable
    {
        private readonly DatabaseContext db;

        public SoftwareRepository(DatabaseContext context)
        {
            this.db = context;
        }
        public void Dispose()
        {
            db.Dispose();
            //throw new NotImplementedException();
        }
    }
  
}
