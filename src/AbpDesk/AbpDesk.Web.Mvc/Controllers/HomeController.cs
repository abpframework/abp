using AbpDesk.Web.Mvc.Temp;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpDesk.Web.Mvc.Controllers
{
    public class HomeController : AbpController
    {
        public IActionResult Index(MyClassToTestAutofacCustomRegistration obj)
        {
            return View();
        }
    }
}
