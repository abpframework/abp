using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.CmsKit.Web.Controllers
{
    //TODO: Consider to move to an area, but also consider to not have the same prefix with API Controllers which can be problem in case of a tiered architecture
    public class CmsKitPublicWidgetsController : CmsKitPublicControllerBase
    {
        public async Task<IActionResult> ReactionSelection(string entityType, string entityId)
        {
            //TODO: Can we change "CmsReactionSelection" to typeof(ReactionSelectionViewComponent)
            return ViewComponent("CmsReactionSelection", new {entityType, entityId});
        }
    }
}
