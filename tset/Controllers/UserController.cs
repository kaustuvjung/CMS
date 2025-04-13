using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace tset.Controllers
{

    public class UserController : Controller
    {

        public UserController() { 

        }
    
        public IActionResult Index()
        {
            return View();
        }
    }
}
