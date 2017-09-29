using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity.HttpApi.Host.Controllers
{
    public class HomeController : AbpController
    {
        [HttpPost]
        [Route("/api/v1/users/create")]
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
