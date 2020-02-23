using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Account.Web.Areas.Account.Controllers.Models;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.Identity;
using Volo.Abp.Validation;
using Volo.Abp.IdentityModel;
using Volo.Abp.MultiTenancy;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthServer.Host.Controllers
{
    [ApiController]
    [Route("/api/account")]
    public class AccountController : Controller
    {
        private readonly IdentityUserManager _userManager;
        private readonly IConfiguration _configuration;
        private readonly ICurrentTenant _currentTenant;
        private readonly AbpAspNetCoreMultiTenancyOptions _aspNetCoreMultiTenancyOptions;
        private readonly IIdentityModelAuthenticationService _authenticator;

        public AccountController(IdentityUserManager userManager,
            IConfiguration configuration,
            ICurrentTenant currentTenant,
            IOptions<AbpAspNetCoreMultiTenancyOptions> options, IIdentityModelAuthenticationService authenticator)
        {
            _userManager = userManager;
            _currentTenant = currentTenant;
            _authenticator = authenticator;
            _aspNetCoreMultiTenancyOptions = options.Value;
            _configuration = configuration;
        }

        /// <summary>
        /// DiscoveryClient方法提示在下一个版本被弃用，scope传递offline_access，可得到refresh_token值
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost("token")]
        //[Obsolete]
        public async Task<IActionResult> Index(UserLoginInfo login)
        {
            HttpClient client = new HttpClient();

            await ReplaceEmailToUsernameOfInputIfNeeds(login);

            var disco = await client.GetDiscoveryDocumentAsync(_configuration["AuthServer:Authority"]);
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return Json(new
                {
                    code = 0,
                    data = disco.HttpErrorReason,
                    message = disco.Error
                });
            }

            TokenResponse response = await client.RequestTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                GrantType = GrantType.ResourceOwnerPassword,

                ClientId = _configuration["AuthServer:ClientId"],
                ClientSecret = _configuration["AuthServer:ClientSecret"],
                //Parameters =
                //{
                //    {_aspNetCoreMultiTenancyOptions.TenantKey,login.TenanId?.ToString()}
                //},
                Parameters =
                {
                    { "UserName",login.UserNameOrEmailAddress},
                    { "Password",login.Password}
                }
                //UserName = login.UserNameOrEmailAddress,
                //Password = login.Password,
                //Scope = "offline_access IdentityService BackendAdminAppGateway AuditLogging BaseManagement OrganizationService"
            });

            if (response.IsError)
            {
                Console.WriteLine(response.Error);
                return Json(new
                {
                    code = 0,
                    data = response.ErrorDescription,
                    message = response.Error
                });
            }

            return Json(new { code = 1, data = response.Json });
        }

        protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds(UserLoginInfo login)
        {
            if (!ValidationHelper.IsValidEmailAddress(login.UserNameOrEmailAddress))
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
        /// 官方支持获取AccessToken的写法，传递offline_access也得不到refresh_token,除了改类库源码GetAccessTokenAsync，直接返回tokenResponse
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
                Scope = "offline_access IdentityService BackendAdminAppGateway AuditLogging BaseManagement OrganizationService"
            };

            string token = await _authenticator.GetAccessTokenAsync(config);

            return Json(new { code = 1, data = token });
        }
    }
}


//{
//"userNameOrEmailAddress": "710277267",
//"password": "1q2w3E*",
//"rememberMe": true,
//"tenanId": "446A5211-3D72-4339-9ADC-845151F8ADA0"
//}