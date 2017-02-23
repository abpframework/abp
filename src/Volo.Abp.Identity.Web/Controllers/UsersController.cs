using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity.Web.Controllers
{
    //TODO: to a seperated area, named Identity!
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
