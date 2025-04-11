using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class DateCollection
    {
        public object GetDates(int start, int end)
        {
            var dates = DateMiti.GetDateMiti.Dates(start, end, "-").Select(x => new { key = x.Key.ToString("yyy/MM/dd"), value = x.Value }).ToList();
            return dates;
        }
    }

    public static class DateHelper
    {
        public static DateTime? GetDate(string miti)
        {
            if (!string.IsNullOrEmpty(miti))
                miti = miti.Replace("/", "-");
            var _dt = DateMiti.GetDateMiti.GetDate(miti);
            return _dt == DateTime.MinValue ? (DateTime?)null : _dt;
        }

        public static DateTime? GetDate(int? year, int? month, int day)
        {
            if (!year.HasValue || !month.HasValue)
                return (DateTime?)null;

            int _day = day == 0 ? GetTotalDaysInMonth(year.Value, month.Value) : day;
            string miti = $"{year}-{month.Value.ToString("00")}-{_day.ToString("00")}";
            return GetDate(miti);
        }

        public static int GetTotalDaysInMonth(int year, int month)
        {
            return DateMiti.GetDateMiti.TotalDaysInMonth(year, month);
        }

        public static string GetMiti(DateTime? date)
        {
            return DateMiti.GetDateMiti.GetMiti(date);
        }

        public static string GetMiti(DateTime date)
        {
            return DateMiti.GetDateMiti.GetMiti(date);
        }

    }
    public static class UserAgent
    {
        public static string ClientIpAddress
        {
            get
            {
                var context = DI.Instance.Resolve<IHttpContextAccessor>().HttpContext;

                string ipAddress = context.Connection.RemoteIpAddress.ToString();
                return ipAddress;
            }
        }

        public static string ClientUserAgent
        {
            get
            {
                var context = DI.Instance.Resolve<IHttpContextAccessor>().HttpContext;
                return context.Request.Headers["User-Agent"].ToString();

            }
        }

        public static string ClientDevice
        {
            get
            {
                //var context = DI.Instance.Resolve<IHttpContextAccessor>().HttpContext;
                return string.Empty;

                //HttpRequest currentRequest = HttpContext.Current.Request;
                //string userAgent = currentRequest != null ? currentRequest.ServerVariables["REMOTE_HOST"] : "NA";

                //return userAgent;
            }
        }
    }
}
