using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthServer.Host.Controllers
{
    [ApiController]
    [Route("/api/account")]
    public class AccountController : Controller
    {

        [HttpPost("token")]
        public async Task<IActionResult> Index()
        {
            var dico = await DiscoveryClient.GetAsync("http://localhost:64999");

            var tokenClient = new TokenClient(dico.TokenEndpoint, "backend-admin-app-client", "1q2w3e*");
            TokenResponse tokenresp = await tokenClient.RequestResourceOwnerPasswordAsync("admin", "1q2w3E*", "IdentityService BackendAdminAppGateway");
            if (tokenresp.IsError)
            {
                Console.WriteLine(tokenresp.Error);
                return Json(new { code = 0 });
            }

            return Json(new { code = 1, data = tokenresp.Json });
        }
    }
}
