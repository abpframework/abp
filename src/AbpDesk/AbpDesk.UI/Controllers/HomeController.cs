using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpDesk.Controllers
{
    public class HomeController : AbpController
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Tickets");
        }
    }
}
