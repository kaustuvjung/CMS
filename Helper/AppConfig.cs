using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class AppConfig
    {
        private readonly IConfiguration _config;
        public AppConfig(IConfiguration config)
        {
            _config = config;
        }
        public bool EnableSSl
        {
            get
            {
                 return _config.GetSection("AppSettings").GetSection("EnableSSL") != null;
            }
        }

        public string GetValue(string key)
        {
            return _config.GetSection("AppSettings").GetSection(key).Value;
        }

    }
}
