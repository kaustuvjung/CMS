using DataAccess.InterFaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
        [Route("provinces")]
        public async Task<IActionResult> ProvinceList()
        {
            var data = await _setupRepo.GetProvincesAsync();
            return Ok(data);
        }
        [Route("districts")]
        public async Task<IActionResult> DistrictList()
        {
            var data = await _setupRepo.GetDistrictAsync();
            return Ok(data);
        }

        [Route("localbodies")]
        public  async Task<IActionResult> LocalBodyList()
        {
            var data = await _setupRepo.GetLocalBodyAsync();
            return Ok(data);
        }

        [Route("wards")]
        public async Task<IActionResult> WardList()
        {
            var data = await _setupRepo.GetWards();
            return Ok(data);
        }
        [Route("companyinfo")]
        public async  Task<IActionResult> CompanyInfo()
        {
            var data = await _setupRepo.GetCompanyInfo();
            return Ok(data);
        }

        [Route("departments")]
        public async Task<IActionResult> DepartmentList()
        {
            var data = await _setupRepo.GetDepartments();
            return Ok(data);
        }
        [HttpGet]
        [Route("permissions")]
       public async Task<IActionResult> Permissions()
        {
            var data = await _setupRepo.GetPermissions();
            return Ok(data);

        }
        [Route("currentuser")]
        public IActionResult CurrentUser() {
            if (!User.Identity.IsAuthenticated)
                return Ok();
            var currentUser = Models.SessionData.CurrentUser;
            return Ok(currentUser);
        }

    }
}
