using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Comments;

namespace Volo.CmsKit.Web.Pages.CmsKit.Shared.Components.Commenting
{
    [ViewComponent(Name = "CmsCommenting")]
    [Widget(
        ScriptTypes = new[] {typeof(CommentingScriptBundleContributor)},
        StyleTypes = new[] {typeof(CommentingStyleBundleContributor)},
        RefreshUrl = "/CmsKitPublicWidgets/Commenting"
    )]
    public class CommentingViewComponent : AbpViewComponent
    {
        public ICommentPublicAppService CommentPublicAppService { get; }

        public CommentingViewComponent(
            ICommentPublicAppService commentPublicAppService)
        {
            CommentPublicAppService = commentPublicAppService;
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(string entityType, string entityId, string loginUrl = null)
        {
            var result = await CommentPublicAppService.GetAllForEntityAsync(entityType, entityId);

            var viewModel = new CommentingViewModel
            {
                EntityId = entityId,
                EntityType = entityType,
                LoginUrl = loginUrl,
                Comments = result.Items.ToList()
            };

            return View("~/Pages/CmsKit/Shared/Components/Commenting/Default.cshtml", viewModel);
        }

        public class CommentingViewModel
        {
            public string EntityType { get; set; }

            public string EntityId { get; set; }

            public string LoginUrl { get; set; }

            public List<CommentWithDetailsDto> Comments { get; set; }
        }
    }
}
