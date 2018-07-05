using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Account.Web.Modules.Account.Components.Toolbar.UserLoginLink
{
    public class UserLoginLinkViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Modules/Account/Components/Toolbar/UserLoginLink/Default.cshtml");
        }
    }
}
