using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.Views.Shared.Components.AbpMenu
{
    public class AbpMenuViewComponent : AbpViewComponent
    {
        private readonly IMenuManager _menuManager;

        public AbpMenuViewComponent(IMenuManager menuManager)
        {
            _menuManager = menuManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string menuName = StandardMenus.Main, string viewName = "Default")
        {
            var menu = await _menuManager.GetAsync(menuName);
            return View(viewName, menu);
        }
    }
}
