using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity.Web.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Authorize]
    public class UsersController : AbpController
    {
        private readonly IIdentityUserAppService _identityUserAppService;

        public UsersController(IIdentityUserAppService identityUserAppService)
        {
            _identityUserAppService = identityUserAppService;
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> _Create()
        {
            var identityUser = await _identityUserAppService.GetUserForCreateOrUpdateAsync(null);

            return PartialView(identityUser);
        }

        public async Task<ActionResult> _Update(Guid id)
        {
            var identityUser = await _identityUserAppService.GetUserForCreateOrUpdateAsync(id);

            return PartialView(identityUser);
        }
    }
}
