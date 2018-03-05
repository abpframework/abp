using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.AspNetCore.Mvc;

namespace MicroserviceDemo.Web.Controllers
{
    public class AccountController : AbpController
    {
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await HttpContext.SignOutAsync("oidc");
        }
    }
}
