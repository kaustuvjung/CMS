using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static class Extensions
    {
        public static string BuildSQLFilter(this string search, Func<string, string> processExp = null)
        {
            if (string.IsNullOrEmpty(search))
                return string.Empty;

            List<List<string>> filter = new List<List<string>>();
            if (!string.IsNullOrEmpty(search))
            {
                var filterExpressions = search.Split(new string[] { ",\"and\",", ",\"or\"," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in filterExpressions)
                {
                    filter.Add(JsonConvert.DeserializeObject<List<string>>(item.Replace("[[", "[").Replace("]]", "]")));
                }
            }

            if (filter.Any())
            {
                List<string> filterList = new List<string>();
                foreach (var flist in filter)
                {
                    var exp = processExp != null ? processExp(flist[0]) : flist[0];
                    var comparision = flist[1];
                    var value = flist[2];

                    filterList.Add(GetSQLComparision(exp, comparision, value));
                }

                if (search.Contains("and"))
                    return string.Join(" And ", filterList);
                else
                    return string.Join(" Or ", filterList);
            }

            return string.Empty;
        }

        public static string GetSQLComparision(string exp, string comparision, string value)
        {
            switch (comparision)
            {
                case "contains":
                    return $"{exp} like N'%{value}%'";
                case "startswith":
                    return $"{exp} like N'{value}%'";
                case "=":
                    return $"{exp} = N'{value}'";
                case ">=":
                    return $"{exp} >= N'{value}'";
                case "<=":
                    return $"{exp} <= N'{value}'";
                default:
                    return string.Empty;
            }
        }
    }
}
