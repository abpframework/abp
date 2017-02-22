using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Ui.Navigation;

namespace AbpDesk.Web.Mvc.Views.Shared.Components.HorizontalMenu
{
    public class HorizontalMenuViewComponent : ViewComponent
    {
        private readonly IMenuManager _menuManager;

        public HorizontalMenuViewComponent(IMenuManager menuManager)
        {
            //TODO: Create a INavigationAppService that can also be used remotely, instead of directly using IMenuManager!

            _menuManager = menuManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string menuName = StandardMenus.Main)
        {
            var menu = await _menuManager.GetAsync(StandardMenus.Main);
            return View(menu);
        }
    }
}
