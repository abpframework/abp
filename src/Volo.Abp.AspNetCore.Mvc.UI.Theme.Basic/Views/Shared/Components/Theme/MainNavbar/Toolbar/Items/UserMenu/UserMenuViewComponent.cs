using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Views.Shared.Components.Theme.MainNavbar.Toolbar.Items.UserMenu
{
    public class UserMenuViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Views/Shared/Components/Theme/MainNavbar/Toolbar/Items/UserMenu/Default.cshtml");
        }
    }
}
