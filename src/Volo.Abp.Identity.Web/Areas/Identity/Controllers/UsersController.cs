using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity.Web.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class UsersController : AbpController
    {
        private readonly IUserAppService _userAppService;

        public UsersController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public async Task<ActionResult> Index()
        {
            var result = await _userAppService.GetListAsync(new PagedAndSortedResultRequestDto());
            return View(result.Items);
        }
    }
}
