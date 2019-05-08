using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Volo.Abp.Configuration;
using Volo.Abp.Identity;
using Volo.Abp.Validation;
using UserLoginInfo = Volo.Abp.Account.Web.Areas.Account.Controllers.Models.UserLoginInfo;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthServer.Host.Controllers
{
    [ApiController]
    [Route("/api/account")]
    public class AccountController : Controller
    {
        private readonly IdentityUserManager _userManager;
        private readonly IConfiguration _configuration;
        public AccountController(IdentityUserManager userManager, IConfigurationAccessor configurationAccessor)
        {
            _userManager = userManager;
            _configuration = configurationAccessor.Configuration;
        }

        [HttpPost("token")]
        public async Task<IActionResult> Index(UserLoginInfo login)
        {
            var dico = await DiscoveryClient.GetAsync(_configuration["AuthServer:Authority"]);
            if (dico.IsError)
            {
                Console.WriteLine(dico.Error);
                return Json(new { code = 0, data = dico.Error });
            }

            await ReplaceEmailToUsernameOfInputIfNeeds(login);

            var tokenClient = new TokenClient(dico.TokenEndpoint, _configuration["AuthServer:ClientId"], _configuration["AuthServer:ClientSecret"]);
            TokenResponse tokenresp = await tokenClient.RequestResourceOwnerPasswordAsync(login.UserNameOrEmailAddress, login.Password, "IdentityService BackendAdminAppGateway AuditLogging BaseManagement OrganizationService");
            if (tokenresp.IsError)
            {
                Console.WriteLine(tokenresp.Error);
                return Json(new { code = 0,data=tokenresp.ErrorDescription,message=tokenresp.Error });
            }

            return Json(new { code = 1, data = tokenresp.Json });
        }

        protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds(UserLoginInfo login)
        {
            if (!ValidationHandler.IsValidEmailAddress(login.UserNameOrEmailAddress))
            {
                return;
            }

            var userByUsername = await _userManager.FindByNameAsync(login.UserNameOrEmailAddress);
            if (userByUsername != null)
            {
                return;
            }

            var userByEmail = await _userManager.FindByEmailAsync(login.UserNameOrEmailAddress);
            if (userByEmail == null)
            {
                return;
            }

            if(userByEmail.EmailConfirmed == false)
            {
                throw new UserFriendlyException("邮件未激活确认,无法使用邮件进行登录!");
            }

            login.UserNameOrEmailAddress = userByEmail.UserName;
        }

    }
}
