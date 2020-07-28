using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Reactions;

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
        protected IReactionPublicAppService ReactionPublicAppService { get; }

        protected CmsKitUiOptions Options { get; }

        public CommentingViewComponent(
            IReactionPublicAppService reactionPublicAppService,
            IOptions<CmsKitUiOptions> options)
        {
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(
            string entityType,
            string entityId)
        {

            return View("~/Pages/CmsKit/Shared/Components/Commenting/Default.cshtml", new CommentingViewModel
            {
                EntityType = entityType,
                EntityId = entityId,
                Reactions = new List<CommentViewModel>()
            });

            var result = await ReactionPublicAppService.GetForSelectionAsync(entityType, entityId);

            var viewModel = new CommentingViewModel
            {
                EntityType = entityType,
                EntityId = entityId,
                Reactions = new List<CommentViewModel>()
            };

            foreach (var reactionDto in result.Items)
            {
                viewModel.Reactions.Add(
                    new CommentViewModel //TODO: AutoMap
                    {
                        Name = reactionDto.Reaction.Name,
                        DisplayName = reactionDto.Reaction.DisplayName,
                        Icon = Options.ReactionIcons.GetLocalizedIcon(reactionDto.Reaction.Name),
                        Count = reactionDto.Count,
                        IsSelectedByCurrentUser = reactionDto.IsSelectedByCurrentUser
                    });
            }

            return View("~/Pages/CmsKit/Shared/Components/Commenting/Default.cshtml", viewModel);
        }

        public class CommentingViewModel
        {
            public string EntityType { get; set; }

            public string EntityId { get; set; }

            public List<CommentViewModel> Reactions { get; set; }
        }

        public class CommentViewModel
        {
            [NotNull]
            public string Name { get; set; }

            [CanBeNull]
            public string DisplayName { get; set; }

            [NotNull]
            public string Icon { get; set; }

            public int Count { get; set; }

            public bool IsSelectedByCurrentUser { get; set; }
        }
    }
}
