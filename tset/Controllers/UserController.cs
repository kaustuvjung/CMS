using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace tset.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        public UserController() { 

        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}
