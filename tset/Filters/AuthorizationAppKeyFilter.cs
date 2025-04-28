using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using tset.Controllers;

namespace tset.Filters
{
    public class AuthorizationAppKeyFilter: ActionFilterAttribute
    {
        private string _appkey;

        public AuthorizationAppKeyFilter(string appkey)
        {
            _appkey = appkey;
        }

        public override void OnActionExecuting(ActionExecutingContext filtercontext)
        {
            var app_key = string.Empty;

            if (Utility.Configuration["IsSSOIntegrationEnabled"] ==  null  || !Convert.ToBoolean(Utility.Configuration["IsSSOIntegrationEnabled"]))
            {
                filtercontext.Result = new ObjectResult(new { error = "Inavalid API Key" }) { StatusCode = 401 };
                return;
            }

            if (filtercontext.ActionArguments.ContainsKey(app_key))
            {
                app_key = filtercontext.ActionArguments["app_key"].ToString();
            }
            else if (filtercontext.ActionArguments.ContainsKey("model") && filtercontext.ActionArguments["model"].GetType()== typeof(UserMapRequest))
            {
                UserMapRequest usermapRequest = (UserMapRequest)Convert.ChangeType(filtercontext.ActionArguments["model"], typeof(UserMapRequest));
                app_key = usermapRequest.app_key;
            }

            if (string.IsNullOrEmpty(app_key))
            {
                filtercontext.Result = new ObjectResult(new { error = "API Key is Missing." }) { StatusCode = 403 };
                return;
            }

            if (app_key != _appkey)
            {

                filtercontext.Result = new ObjectResult(new { error = "Inavlid Status code." }) { StatusCode = 401 };
                return;
            }
        }
    }
}
