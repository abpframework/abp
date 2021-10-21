using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.PageToolbars;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Pages.Shared.Components.AbpPageToolbar
{
    public class AbpPageToolbarViewComponent : AbpViewComponent
    {
        protected IPageToolbarManager ToolbarManager { get; }

        public AbpPageToolbarViewComponent(IPageToolbarManager toolbarManager)
        {
            ToolbarManager = toolbarManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string pageName)
        {
            var items = await ToolbarManager.GetItemsAsync(pageName);
            return View("~/Pages/Shared/Components/AbpPageToolbar/Default.cshtml", items);
        }
    }
}
