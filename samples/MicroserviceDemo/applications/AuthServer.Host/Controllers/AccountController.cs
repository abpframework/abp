using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Areas.Account.Controllers.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthServer.Host.Controllers
{
    [ApiController]
    [Route("/api/account")]
    public class AccountController : Controller
    {

        public AccountController()
        {
        }

        [HttpPost("token")]
        public async Task<IActionResult> Index(UserLoginInfo login)
        {
            var dico = await DiscoveryClient.GetAsync("http://localhost:64999");

            var tokenClient = new TokenClient(dico.TokenEndpoint, "backend-admin-app-client", "1q2w3e*");
            TokenResponse tokenresp = await tokenClient.RequestResourceOwnerPasswordAsync(login.UserNameOrEmailAddress, login.Password, "IdentityService BackendAdminAppGateway");
            if (tokenresp.IsError)
            {
                Console.WriteLine(tokenresp.Error);
                return Json(new { code = 0,data=tokenresp.ErrorDescription });
            }

            return Json(new { code = 1, data = tokenresp.Json });
        }
    }
}
