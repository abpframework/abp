using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace ProductService.Host.Controllers
{
    public class HomeController : AbpController
    {
        public ActionResult Index()
        {
            //TODO: Enable swagger UI once it supports asp.net core 3.x
            //return Redirect("/swagger");

            return Content("OK: ProductService.Host is working...");
        }
    }
}
