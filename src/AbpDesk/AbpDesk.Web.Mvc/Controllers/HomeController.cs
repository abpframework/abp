using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpDesk.Web.Mvc.Controllers
{
    public class HomeController : AbpController
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Tickets");
        }
    }
}
