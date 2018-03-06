using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Ui;

namespace MicroserviceDemo.Web.Controllers
{
    public class AccountController : AbpController
    {
        private readonly ITenantStore _tenantStore;
        private readonly AspNetCoreMultiTenancyOptions _options;

        public AccountController(ITenantStore tenantStore, IOptions<AspNetCoreMultiTenancyOptions> options)
        {
            _tenantStore = tenantStore;
            _options = options.Value;
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await HttpContext.SignOutAsync("oidc");
        }

        public async Task<IActionResult> CallApiUsingUserAccessToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            var content = await client.GetStringAsync("http://abp-test-tenancy.com:63877/identity");

            ViewBag.Json = JObject.Parse(content).ToString();
            return View("json");
        }

        public async Task<IActionResult> CallApiUsingClientCredentials()
        {
            var tokenClient = new TokenClient("http://abp-test-authserver.com:54307/connect/token", "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("multi-tenancy-api");

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var content = await client.GetStringAsync("http://abp-test-tenancy.com:63877/identity");

            ViewBag.Json = JObject.Parse(content).ToString();
            return View("json");
        }

        public async Task<ActionResult> SwitchTenant(string tenant = "")
        {
            if (tenant.IsNullOrEmpty())
            {
                HttpContext.Response.Cookies.Delete(_options.TenantKey);
            }
            else
            {
                var tenantInfo = await FindTenantAsync(tenant);
                if (tenantInfo == null)
                {
                    throw new UserFriendlyException("Unknown tenant: " + tenant);
                }

                HttpContext.Response.Cookies.Append(
                    _options.TenantKey,
                    tenantInfo.Id.ToString(),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddYears(1)
                    }
                );
            }

            return Redirect("/");
        }

        private async Task<TenantInfo> FindTenantAsync(string tenantIdOrName)
        {
            if (Guid.TryParse(tenantIdOrName, out var parsedTenantId))
            {
                return await _tenantStore.FindAsync(parsedTenantId);
            }
            else
            {
                return await _tenantStore.FindAsync(tenantIdOrName);
            }
        }
    }
}
