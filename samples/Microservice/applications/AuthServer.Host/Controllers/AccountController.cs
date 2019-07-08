using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Volo.Abp.Account.Web.Areas.Account.Controllers.Models;
using Volo.Abp.Configuration;
using Volo.Abp.Identity;
using Volo.Abp.Validation;
using Volo.Abp.IdentityModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthServer.Host.Controllers
{
    [ApiController]
    [Route("/api/account")]
    public class AccountController : Controller
    {
        private readonly IdentityUserManager _userManager;
        private readonly IConfiguration _configuration;
        public IIdentityModelAuthenticationService _authenticator { get; set; }

        public AccountController(IdentityUserManager userManager, IConfigurationAccessor configurationAccessor)
        {
            _userManager = userManager;
            _configuration = configurationAccessor.Configuration;
        }

        /// <summary>
        /// 该方法提示在下一个版本被弃用
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost("token")]
        //[Obsolete]
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
                return Json(new { code = 0, data = tokenresp.ErrorDescription, message = tokenresp.Error });
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

            if (userByEmail.EmailConfirmed == false)
            {
                throw new UserFriendlyException("邮件未激活确认,无法使用邮件进行登录!");
            }

            login.UserNameOrEmailAddress = userByEmail.UserName;
        }

        /// <summary>
        /// 官方支持获取AccessToken的写法
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost("getAccessToken")]
        public async Task<IActionResult> GetAccessToken(UserLoginInfo login)
        {
            await ReplaceEmailToUsernameOfInputIfNeeds(login);

            var config = new IdentityClientConfiguration
            {
                Authority = _configuration["AuthServer:Authority"],
                ClientId = _configuration["AuthServer:ClientId"],
                ClientSecret = _configuration["AuthServer:ClientSecret"],
                GrantType = OidcConstants.GrantTypes.Password,
                UserName = login.UserNameOrEmailAddress,
                UserPassword = login.Password,
                Scope = "IdentityService BackendAdminAppGateway AuditLogging BaseManagement OrganizationService"
            };

            string token = await _authenticator.GetAccessTokenAsync(config);

            return Json(new { code = 1, data = token });
        }


    }
}
