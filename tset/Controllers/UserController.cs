using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccess.InterFaces;
using DataAccess.Model.Setup;
using Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Mvc;
using tset.Models;

namespace tset.Controllers
{

    public class UserController : Controller
    {
        private IUserRepository _userRepo;
        public UserController(IUserRepository userRepo) {
            _userRepo = userRepo;

        }

        #region User Login 
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return PartialView();
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                var enc = new EncryptionService();
                var userSalt = await _userRepo.GetUserSaltByUsernameAsync(model.Username);
                if (model == null || userSalt == null)
                {
                    ModelState.AddModelError("Summary", "Inavalid username or password");
                    return PartialView(model);
                }
                else
                {
                    model.Password = enc.CreatePasswordHash(model.Password, userSalt);
                    var signedUser = await _userRepo.Login(model);
                    if(signedUser.Rows.Count ==0)
                    {
                        ModelState.AddModelError("Summary", "Inavalid username or password");
                        return PartialView(model);
                    }
                    else
                    {
                        //if (!string.IsNullOrEmpty(signedUser.Rows[0]["ValidDate"].ToString())
                        //    && DateTime.Today > Convert.ToDateTime(signedUser.Rows[0]["ValidDate"].ToString()))
                        //{
                        //    ModelState.AddModelError("Summary", "User validatity Expired!!");
                        //    return PartialView(model);
                        //}
                        await AddCredentialAsync(signedUser);
                        if (Url.IsLocalUrl(""))
                            return Redirect("");
                        else
                            return RedirectToAction("Privacy", "Home");

                    }
                }
            }
            else
            {
                ModelState.AddModelError("Summary", "Invalid login");
                return PartialView(model);
            }
        }

        [HttpGet]
        [Route("isloggedin")]
        public IActionResult IsLoggedIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = SessionData.CurrentUser;
                return Ok(new { isAuthenticated = true, user = currentUser.Name, permission = currentUser.PermissionId });
            }
            else
            {
                return Ok(new {isAuthenticated= false });
            }
        }




        private async Task AddCredentialAsync(DataTable user)
        {
            var claims = new List<Claim>
            {
                new Claim (ClaimTypes.Name, user.Rows[0]["Name"].ToString()),
                new Claim("Id",  user.Rows[0]["Id"].ToString()),
                new Claim("Username", user.Rows[0]["Username"].ToString()),
                new Claim("Department", user.Rows[0]["Department"].ToString()),
               
                new Claim("Permission",  user.Rows[0]["PermissionId"].ToString())
            };
            var claimsIdenntity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = false
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdenntity), authProperties);

        }

        #endregion
    }
}
