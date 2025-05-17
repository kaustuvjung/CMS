using System.Data;
using System.Threading.Tasks;
using DataAccess.InterFaces;
using DataAccess.Model.setup;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tset.Models;

namespace tset.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class SetupController : Controller
    {
        private ISetupRepository _setuprepo;
        public SetupController(ISetupRepository setuprepo)
        {
            _setuprepo = setuprepo;
        }
        [Route("software")]
        [AllowAnonymous]
        public async Task<DataTable> Software(int skip, int take, string filter)
        {
            var model = await _setuprepo.GetSoftware(skip, take, filter);
            return model;
        }
        [Route("savesoftware")]
        [HttpPost]
        public async Task<Software> SaveSoftware(Software model)
        {
            var currentUser = SessionData.CurrentUser;
            var response =  await _setuprepo.SaveSoftware(model);
            return model;
        }

        [HttpDelete]
        [Route("deletesoftware")]
        public async Task<IActionResult> Deletesoftware(int id)
        {
            var response = await _setuprepo.DeleteSoftware(id);
            return Ok();
        }
    }
}
