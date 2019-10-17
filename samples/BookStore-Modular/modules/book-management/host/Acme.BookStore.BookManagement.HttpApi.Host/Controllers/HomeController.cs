using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Acme.BookStore.BookManagement.Controllers
{
    public class HomeController : AbpController
    {
        public ActionResult Index()
        {
            //TODO: Enable when Swagger supports ASP.NET Core 3.x
            //return Redirect("/swagger");

            return Content("OK: Acme.BookStore.BookManagement.HttpApi.Host is running...");
        }
    }
}
