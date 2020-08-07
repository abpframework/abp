using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Commenting;
using Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.ReactionSelection;

namespace Volo.CmsKit.Public.Web.Controllers
{
    //TODO: Consider to move to an area, but also consider to not have the same prefix with API Controllers which can be problem in case of a tiered architecture
    public class CmsKitPublicWidgetsController : CmsKitPublicControllerBase
    {
        public async Task<IActionResult> ReactionSelection(string entityType, string entityId)
        {
            return ViewComponent(typeof(ReactionSelectionViewComponent), new {entityType, entityId});
        }

        public async Task<IActionResult> Commenting(string entityType, string entityId)
        {
            return ViewComponent(typeof(CommentingViewComponent), new {entityType, entityId});
        }
    }
}
