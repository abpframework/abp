using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Authorization;

namespace AbpDesk.Web.Mvc.Controllers
{
    public class AuthTestController : AbpController
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthTestController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [Authorize(Policy = "RequirePhoneNumber")]
        public ContentResult PhoneNumberIsRequired()
        {
            return Content("OK: " + User.Claims.First(c => c.Type == "phone_number")?.Value);
        }

        //[RequirePermission("AllowedPermission")]
        public async Task<ContentResult> AllowedPermissionTest()
        {

            var a = "..";
            var result = await _authorizationService.AuthorizeAsync(User, a);
            if (result.Succeeded)
            {
                //...
            }

            return Content("OK: AllowedPermission");
        }
        
        [RequiresPermission("NotAllowedPermission")]
        public ContentResult NotAllowedPermissionTest()
        {
            return Content("OK: NotAllowedPermission");
        }
    }
}
