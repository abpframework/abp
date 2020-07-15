using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Reactions;

namespace Volo.CmsKit.Web.Pages.CmsKit.Shared.Components.ReactionSelection
{
    [Widget(
        ScriptTypes = new[] {typeof(ReactionSelectionScriptBundleContributor)}
    )]
    public class ReactionSelectionViewComponent : AbpViewComponent
    {
        protected IReactionPublicAppService ReactionPublicAppService { get; }

        protected CmsKitUiOptions Options { get; }

        public ReactionSelectionViewComponent(
            IReactionPublicAppService reactionPublicAppService,
            IOptions<CmsKitUiOptions> options)
        {
            ReactionPublicAppService = reactionPublicAppService;
            Options = options.Value;
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(
            string entityType = null)
        {
            var result = await ReactionPublicAppService.GetAvailableReactions(
                new GetAvailableReactionsDto
                {
                    EntityType = entityType
                }
            );

            var viewModel = new ReactionSelectionViewModel
            {
                EntityType = entityType,
                Reactions = new List<ReactionViewModel>()
            };

            foreach (var reactionDto in result.Items)
            {
                viewModel.Reactions.Add(
                    new ReactionViewModel
                    {
                        Name = reactionDto.Name,
                        DisplayName = reactionDto.DisplayName,
                        Icon = Options.ReactionIcons.GetLocalizedIcon(reactionDto.Name)
                    });
            }

            return View("~/Pages/CmsKit/Shared/Components/ReactionSelection/Default.cshtml", viewModel);
        }
    }
}
