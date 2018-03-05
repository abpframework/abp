using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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

        public async Task<IActionResult> CallApiUsingUserAccessToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            var content = await client.GetStringAsync("http://localhost:63877/identity");

            ViewBag.Json = JObject.Parse(content).ToString();
            return View("json");
        }

        public async Task<IActionResult> CallApiUsingClientCredentials()
        {
            var tokenClient = new TokenClient("http://localhost:54307/connect/token", "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("multi-tenancy-api");

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var content = await client.GetStringAsync("http://localhost:63877/identity");

            ViewBag.Json = JObject.Parse(content).ToString();
            return View("json");
        }
    }
}
