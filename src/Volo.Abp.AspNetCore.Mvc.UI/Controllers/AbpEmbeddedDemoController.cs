using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.Controllers
{
    public class AbpEmbeddedDemoController : AbpController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
