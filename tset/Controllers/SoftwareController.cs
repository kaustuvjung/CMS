using DataAccess.InterFaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace tset.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class SoftwareController : Controller
    {
        private  readonly ISoftwareRepository _softwareRepo;
        public SoftwareController(ISoftwareRepository softwareRepo)
        {
            _softwareRepo = softwareRepo;
        }
        public IActionResult Index()
        {
            return View();
        }














    }
}
