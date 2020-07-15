using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.CmsKit.Web.Pages.CmsKit.Shared.Components
{
    public class ReactionSelectionViewComponent : AbpViewComponent
    {
        public virtual async Task<IViewComponentResult> InvokeAsync(string entityType = null, string entityId = null)
        {
            return View(
                "~/Pages/CmsKit/Shared/Components/Default.cshtml",
                new ReactionSelectionViewModel()
                );
        }
    }
}
