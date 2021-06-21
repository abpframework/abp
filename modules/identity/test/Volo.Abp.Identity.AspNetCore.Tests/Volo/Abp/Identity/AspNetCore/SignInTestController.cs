using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity.AspNetCore
{
    [Route("api/signin-test")]
    public class SignInTestController : AbpController
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public SignInTestController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [Route("password")]
        public async Task<ActionResult> PasswordLogin(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(
                userName,
                password,
                false,
                false
            );

            return Content(result.ToString());
        }
    }
}
