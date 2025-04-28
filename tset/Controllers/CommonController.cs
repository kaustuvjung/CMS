using DataAccess.InterFaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace tset.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CommonController : Controller
    {

        private ISetupRepository _setupRepo;

        public CommonController(ISetupRepository setupRepo)
        {
            _setupRepo = setupRepo;
        }
        [HttpGet]
        [Route("permissions")]
       public async Task<IActionResult> Permissions()
        {
            var data = await _setupRepo.GetPermissions();
            return Ok(data);

        }
    }
}
