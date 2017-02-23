using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace AbpDesk.Web.Mvc.Controllers
{
    //TODO: Move to Volo.Abp.Identity.AspNetCore.Mvc or a similar module as a seperated area!
    public class UsersController : AbpController
    {
        private readonly IUserAppService _userAppService;

        public UsersController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public async Task<ActionResult> Index()
        {
            var result = await _userAppService.GetAll();
            return View(result.Items);
        }
    }
}
