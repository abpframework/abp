using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Account.Web.Areas.Account.Controllers
{
    [Area("Account")]
    public class LoginController : AbpController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
