using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace tset.Filters
{
    public class AuthorizationTokenFilter: Attribute, IAuthorizationFilter
    {
        private readonly string _authorizationToken;

        public AuthorizationTokenFilter(string authorizationToken)
        {
            _authorizationToken = authorizationToken;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string parsedAuthorizedToken;
            string authorization = context.HttpContext.Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.Authorization];
            if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer"))
            {
                parsedAuthorizedToken = authorization.Split(" ")[1];
            }
            else
            {
                context.Result = new ObjectResult(new { error = "Authorization  Bearer  Token is Mising." }) { StatusCode = 403 };
                return;
            }

            if(parsedAuthorizedToken != _authorizationToken)
            {
                context.Result = new ObjectResult(new { error = "Invalid Authorization Token." }) { StatusCode = 401 };
                return;
            }
        }


    }
}
