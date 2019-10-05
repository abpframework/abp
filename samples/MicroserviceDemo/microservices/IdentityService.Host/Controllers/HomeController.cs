using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace IdentityService.Host.Controllers
{
    public class HomeController : AbpController
    {
        public ActionResult Index()
        {
            //TODO: Enable swagger UI once it supports asp.net core 3.x
            //return Redirect("/swagger");

            return Content("OK: IdentityService.Host is working...");
        }
    }
}
