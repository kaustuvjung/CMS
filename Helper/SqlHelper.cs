using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Helper
{
    public static class ObjectHelper
    {
        public static List<SqlParameter> PrepareSQLParameters<T>(this T item)
        {
            var properties = item.GetType().GetProperties().Where(x => x.DeclaringType == typeof(T));
            var model = new List<SqlParameter>();
            foreach (PropertyInfo prop in properties)
            {
                if (prop.GetCustomAttribute<NotMappedAttribute>() != null)
                    continue;

                var p = new SqlParameter
                {
                    ParameterName = prop.Name
                };
                var val = prop.GetValue(item);
                bool gotVal = false;
                if (prop.GetCustomAttribute<TitleCaseAttribute>() != null && prop.PropertyType == typeof(string) && val != null)
                {
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    p.Value = textInfo.ToTitleCase(val.ToString().ToLower());
                    gotVal = true;
                }
                if (prop.GetCustomAttribute<LowerCaseAttribute>() != null && prop.PropertyType == typeof(string) && val != null)
                {
                    p.Value = val.ToString().ToLower();
                    gotVal = true;
                }
                if (prop.GetCustomAttribute<UpperCaseAttribute>() != null && prop.PropertyType == typeof(string) && val != null)
                {
                    p.Value = val.ToString().ToUpper();
                    gotVal = true;
                }

                if (prop.GetCustomAttribute<SentenceCaseAttribute>() != null && prop.PropertyType == typeof(string) && val != null)
                {
                    var values = string.Empty;
                    int idx = 0;
                    foreach (char c in val.ToString().ToLower())
                    {
                        if (idx == 0)
                        {
                            values += c.ToString().ToUpper();
                        }
                        else
                            values += c.ToString().ToLower();
                        idx++;
                    }
                    p.Value = values.Contains("\n") ? values : Regex.Replace(values, @"\s+", " ");
                    gotVal = true;
                }

                else if (val != null && !gotVal)
                    p.Value = val;
                else if (!gotVal)
                    p.Value = DBNull.Value;
                model.Add(p);
            }
            return model;
        }

        public static IList<T> TransformToList<T>(this DataTable dt) where T : class
        {
            var result = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = Activator.CreateInstance<T>();
                var properties = item.GetType().GetProperties().Where(x => x.DeclaringType == typeof(T));
                foreach (PropertyInfo pinfo in item.GetType().GetProperties())
                {
                    if (dt.Columns[pinfo.Name] != null)
                    {
                        if (row[pinfo.Name] != DBNull.Value)
                        {
                            item.GetType().GetProperty(pinfo.Name).SetValue(item, row[pinfo.Name]);
                        }
                    }
                }
                result.Add(item);
            }
            return result;
        }

        public static IList<T> TransformToDIList<T>(this DataTable dt) where T : class
        {
            var result = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = DI.Instance.Resolve<T>();
                var properties = item.GetType().GetProperties().Where(x => x.DeclaringType == typeof(T));
                foreach (PropertyInfo pinfo in item.GetType().GetProperties())
                {
                    if (dt.Columns[pinfo.Name] != null)
                    {
                        if (row[pinfo.Name] != DBNull.Value)
                        {
                            item.GetType().GetProperty(pinfo.Name).SetValue(item, row[pinfo.Name]);
                        }
                    }
                }
                result.Add(item);
            }
            return result;
        }

        public static IList<T> TransformToList<T>(this EnumerableRowCollection<DataRow> rows) where T : class
        {
            var result = new List<T>();
            foreach (DataRow row in rows)
            {
                T item = Activator.CreateInstance<T>();
                var properties = item.GetType().GetProperties().Where(x => x.DeclaringType == typeof(T));
                foreach (PropertyInfo pinfo in item.GetType().GetProperties())
                {
                    if (row.Table.Columns[pinfo.Name] != null)
                    {
                        if (row[pinfo.Name] != DBNull.Value)
                        {
                            item.GetType().GetProperty(pinfo.Name).SetValue(item, row[pinfo.Name]);
                        }
                    }
                }
                result.Add(item);
            }
            return result;
        }

        public static T TransformToObject<T>(this DataTable dt) where T : class
        {
            if (dt.Rows.Count == 0)
                return null;

            T item = Activator.CreateInstance<T>();
            var row = dt.Rows[0];
            foreach (PropertyInfo pinfo in item.GetType().GetProperties())
            {
                if (dt.Columns[pinfo.Name] != null)
                {
                    if (row[pinfo.Name] != DBNull.Value)
                    {
                        item.GetType().GetProperty(pinfo.Name).SetValue(item, row[pinfo.Name]);
                    }
                }
            }
            return item;
        }

        public static T TransformToDIObject<T>(this DataTable dt) where T : class
        {
            if (dt.Rows.Count == 0)
                return null;

            T item = DI.Instance.Resolve<T>();
            var row = dt.Rows[0];
            foreach (PropertyInfo pinfo in item.GetType().GetProperties())
            {
                if (dt.Columns[pinfo.Name] != null)
                {
                    if (row[pinfo.Name] != DBNull.Value)
                    {
                        item.GetType().GetProperty(pinfo.Name).SetValue(item, row[pinfo.Name]);
                    }
                }
            }
            return item;
        }

        public static T TransformApiDataToObject<T>(this DataTable dt) where T : class
        {
            if (dt.Rows.Count == 0)
                return null;

            T item = Activator.CreateInstance<T>();
            var row = dt.Rows[0];
            foreach (PropertyInfo pinfo in item.GetType().GetProperties())
            {
                if (dt.Columns[pinfo.Name] != null)
                {
                    if (row[pinfo.Name] != DBNull.Value)
                    {
                        item.GetType().GetProperty(pinfo.Name).SetValue(item, row[pinfo.Name]);
                    }
                }
            }
            return item;
        }

        public static T TransformToObject<T>(this DataRow row) where T : class
        {
            T item = Activator.CreateInstance<T>();
            foreach (PropertyInfo pinfo in item.GetType().GetProperties())
            {
                if (row.Table.Columns[pinfo.Name] != null)
                {
                    if (row[pinfo.Name] != DBNull.Value)
                    {
                        item.GetType().GetProperty(pinfo.Name).SetValue(item, row[pinfo.Name]);
                    }
                }
            }
            return item;
        }

        public static IList<T> ConvertToClassList<T>(this object jsonObject) where T : class
        {
            var model = new List<T>();
            string jobj = jsonObject == null || jsonObject == DBNull.Value ? null : (string)jsonObject;
            if (jobj != null)
            {
                model = JsonConvert.DeserializeObject<List<T>>(jobj);
            }
            return model;
        }

        public static T TransformToObject<T>(this object jsonObject) where T : class
        {
            return jsonObject.ConvertToClassList<T>().FirstOrDefault();
        }

        public static string ToJsonString(this DataTable table)
        {
            if (table == null)
                return null;

            string JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class TitleCaseAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SentenceCaseAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class LowerCaseAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class UpperCaseAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class IncludeWhitespaceAttribute : Attribute
    {

    }
}
