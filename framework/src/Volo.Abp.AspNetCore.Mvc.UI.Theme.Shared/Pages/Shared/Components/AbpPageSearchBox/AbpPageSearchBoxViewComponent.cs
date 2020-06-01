using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Pages.Shared.Components.AbpPageSearchBox
{
    public class AbpPageSearchBoxViewComponent : AbpViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string placeHolder)
        {
            return await Task.FromResult(
                View("~/Pages/Shared/Components/AbpPageSearchBox/Default.cshtml",
                new AbpSearchBoxViewModel {PlaceHolder = placeHolder}));
        }
    }
}