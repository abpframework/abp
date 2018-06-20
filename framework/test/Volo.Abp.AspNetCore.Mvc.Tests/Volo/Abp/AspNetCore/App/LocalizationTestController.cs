using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.App
{
    public class LocalizationTestController : AbpController
    {
        public IActionResult Index1()
        {
            return View();
        }
    }
}
