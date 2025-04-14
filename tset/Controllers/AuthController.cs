using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace tset.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }



    public class AuthRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


    public class Constants
    {
        public const string Issuer = Audiance;
        public const string Audiance = "CMSAPI";
        public const string Secret = "746FA8A1-FCBC-4996-B6DB-B75AC0658AFE";

        public const string UserName = "api-user";
        public const string Password = "AppUser-1";
    }
}
