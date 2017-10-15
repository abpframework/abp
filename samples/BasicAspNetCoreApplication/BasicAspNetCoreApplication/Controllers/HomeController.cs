using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace BasicAspNetCoreApplication.Controllers
{
    public class HomeController : AbpController
    {
        public IActionResult Index()
        {
            return Content("Hello World!");
        }
    }
}
