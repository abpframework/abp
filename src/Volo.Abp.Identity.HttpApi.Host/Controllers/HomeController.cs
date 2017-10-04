using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity.HttpApi.Host.Controllers
{
    public class HomeController : AbpController
    {
        [HttpPost]
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
