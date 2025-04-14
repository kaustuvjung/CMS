using DataAccess.InterFaces;
using DataAccess.Repositories;
using Helper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DataDependencyRegistrar
    {
        public static void RegisterDependencies()
        {
            DI.Instance.Services.AddScoped<DatabaseContext>();
            DI.Instance.Services.AddScoped<IUserRepository, UserRepository>();
            DI.Instance.Services.AddScoped<ISetupRepository, SetupRepository>();
        }
    }
}
