using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class DI
    {
        private readonly IServiceCollection _serviceCollection;
        public DI(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }
        public static DI _instance;
        public static DI Instance
        {
            get
            {
                return _instance;
            }

        }


        public  static void Initialize(IServiceCollection serviceCollection)
        {
            _instance = new DI(serviceCollection);
            _instance.Services.AddSingleton<AppConfig>();
        }

        public ServiceProvider Resolver
        {
            get
            {
                return _serviceCollection.BuildServiceProvider();
            }
        }
        public IServiceCollection Services
        {
            get
            {
                return _serviceCollection;
            }
        }

        public T Resolve<T>()
        {
            return Resolver.GetService<T>();
        }
    }
}
