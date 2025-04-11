using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class QueryBuilder
    {
        private static QueryBuilder _instance;

        private QueryBuilder() { }

        public static QueryBuilder GetCommandText
        {
            get
            {
                if (_instance == null) _instance = new QueryBuilder();
                return _instance;
            }
        }

        public string this[string procedure]
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                Stream stream = assembly.GetManifestResourceStream(string.Format("DataAccess.SQL.{0}.txt", procedure));

                if (stream == null)
                    throw new Exception(string.Format("Resource {0} not found. Contact your administrator", procedure));

                StreamReader reader = new StreamReader(stream);
                string query = reader.ReadToEnd();
                return query;
            }
        }

        public string this[string procedure, Dictionary<string, string> parameter]
        {
            get
            {
                string query = this[procedure];

                foreach (var p in parameter)
                {
                    query = query.Replace(p.Key, p.Value);
                }

                return query;
            }
        }

        public string this[string namespc, string name]
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                Stream stream = assembly.GetManifestResourceStream(string.Format("{0}.{1}.txt", namespc, name));

                if (stream == null)
                    throw new Exception(string.Format("Resource {0} cannot be found.", name));

                StreamReader reader = new StreamReader(stream);
                string query = reader.ReadToEnd();
                return query;
            }
        }
    }
}
