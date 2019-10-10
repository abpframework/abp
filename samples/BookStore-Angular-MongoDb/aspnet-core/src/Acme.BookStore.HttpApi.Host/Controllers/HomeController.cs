using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Acme.BookStore.Controllers
{
    public class HomeController : AbpController
    {
        public ActionResult Index()
        {
            //TODO: Enabled once Swagger supports ASP.NET Core 3.x
            //return Redirect("/swagger");
            return Content("OK: Acme.BookStore.HttpApi.Host is running...");
        }
    }
}
