using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Themes.Basic.Components.Toolbar
{
    public class MainNavbarToolbarViewComponent : AbpViewComponent
    {
        protected IToolbarManager ToolbarManager { get; }

        public MainNavbarToolbarViewComponent(IToolbarManager toolbarManager)
        {
            ToolbarManager = toolbarManager;
        }

        public async virtual Task<IViewComponentResult> InvokeAsync()
        {
            var toolbar = await ToolbarManager.GetAsync(StandardToolbars.Main);
            return View("~/Themes/Basic/Components/Toolbar/Default.cshtml", toolbar);
        }
    }
}
