using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Public.Comments;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Commenting
{
    [ViewComponent(Name = "CmsCommenting")]
    [Widget(
        ScriptTypes = new[] {typeof(CommentingScriptBundleContributor)},
        StyleTypes = new[] {typeof(CommentingStyleBundleContributor)},
        RefreshUrl = "/CmsKitPublicWidgets/Commenting",
        AutoInitialize = true
    )]
    public class CommentingViewComponent : AbpViewComponent
    {
        public ICommentPublicAppService CommentPublicAppService { get; }
        public AbpMvcUiOptions AbpMvcUiOptions { get; }

        public CommentingViewComponent(
            ICommentPublicAppService commentPublicAppService,
            IOptions<AbpMvcUiOptions> options)
        {
            CommentPublicAppService = commentPublicAppService;
            AbpMvcUiOptions = options.Value;
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(
            string entityType,
            string entityId)
        {
            var result = await CommentPublicAppService
                .GetListAsync(entityType, entityId);

            var loginUrl = $"{AbpMvcUiOptions.LoginUrl}?returnUrl={HttpContext.Request.Path.ToString()}&returnUrlHash=#cms-comment_{entityType}_{entityId}";

            var viewModel = new CommentingViewModel
            {
                EntityId = entityId,
                EntityType = entityType,
                LoginUrl = loginUrl,
                Comments = result.Items.OrderByDescending(i=> i.CreationTime).ToList()
            };

            return View("~/Pages/CmsKit/Shared/Components/Commenting/Default.cshtml", viewModel);
        }

        public class CommentingViewModel
        {
            public string EntityType { get; set; }

            public string EntityId { get; set; }

            public string LoginUrl { get; set; }

            public IReadOnlyList<CommentWithDetailsDto> Comments { get; set; }
        }
    }
}
