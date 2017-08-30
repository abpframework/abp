using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.App
{
    public class SimpleController : AbpController
    {
        public ActionResult Index()
        {
            return Content("Index-Result");
        }
    }
}
