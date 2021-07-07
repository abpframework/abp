using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Themes.Basic.Components.Menu
{
    public class MainNavbarMenuViewComponent : AbpViewComponent
    {
        private readonly IMenuManager _menuManager;

        public MainNavbarMenuViewComponent(IMenuManager menuManager)
        {
            _menuManager = menuManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menu = await _menuManager.GetMainMenuAsync();
            return View("~/Themes/Basic/Components/Menu/Default.cshtml", menu);
        }
    }
}
