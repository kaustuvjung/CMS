using DataAccess.InterFaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace tset.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class APIController : ControllerBase
    {
        private const string APP_KEY = "tms";
        private const string AUTHORIZATION_TOKEN = "FA29F20G-127H-7F7H-J6F4-BT564BYT5HH03";

        private IUserRepository _userRepo;
        public APIController(IUserRepository userRepo) {
            _userRepo = userRepo;
        }
        [HttpGet]
        //[AuthorizationTokenFilter(AUTHORIZATION_TOKEN)]
        //[AuthorizationAppKeyFilter(APP_KEY)]

        public async Task<IActionResult> Get(string app_key)
        {
            try
            {
                var user = await _userRepo.API_GetUsers();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { error = "Something Went Wrong. Please Try again Later." }) { StatusCode = 500 };
            }
        }
        [HttpPost]
        [Route("map")]

        public async Task<IActionResult> Map (UserMapRequest model)
        {
            try
            {
                var auth_token = await _userRepo.API_MapUser(model.api_user_Id, model.token);
                if (string.IsNullOrEmpty(auth_token))
                {
                    return new ObjectResult(new { error = "No user found with this api_uiser_id." }) { StatusCode = 404 };
                }
                else
                {
                    return Ok(new { app_key = APP_KEY, auth_token = auth_token });
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { error = "Something went wrong . PLEASE try again Later." }) { StatusCode = 500 };
            }
        }

        [HttpGet]
        [Route("authorize")]
        //[AuthorizationAppKeyFilter(APP_KEY)]
        public async Task<IActionResult> Authorize(string app_key, string auth_token)
        {
            if (string.IsNullOrEmpty(auth_token))
                return new ObjectResult(new { error = "Auth Token is Missing. " }) { StatusCode = 403 };

            try {
                var signedUser = await _userRepo.API_AuthorizeUser(auth_token);
                if(signedUser != null && signedUser.Rows.Count> 0)
                {
                    await AddCredentialAsync(signedUser);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return new ObjectResult(new { error = "Something Went Wrong. Please Try again laterr." }) { StatusCode = 401 };
                }
            }
            catch(Exception ex)
            {
                return new ObjectResult(new { error = "Something went Wrong please Try Again later." }) { StatusCode = 500 };
            }
        }

        private async Task AddCredentialAsync(DataTable user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Rows[0]["Name"].ToString()),
                new Claim("Id", user.Rows[0]["Id"].ToString()),
                new Claim("DepartmentId", user.Rows[0]["DepartmentId"].ToString()),
                new Claim("Permission",  user.Rows[0]["PermissionId"].ToString()),
                new Claim("UserName", user.Rows[0]["UserName"].ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }

    }

    public class UserMapRequest
    {
        public string token {  get; set; }
        public string sso_user_id { get; set; }
        public int api_user_Id {  get; set; }
        public string app_key { get; set; }
    }
}
