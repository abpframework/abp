using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Reactions;

namespace Volo.CmsKit.Web.Pages.CmsKit.Shared.Components
{
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
            string entityType = null,
            string entityId = null)
        {
            var result = await ReactionPublicAppService.GetAvailableReactions(
                new GetAvailableReactionsDto
                {
                    EntityType = entityType,
                    EntityId = entityId
                }
            );

            var viewModel = new ReactionSelectionViewModel
            {
                EntityType = entityType,
                EntityId = entityId,
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

            return View("~/Pages/CmsKit/Shared/Components/Default.cshtml", viewModel);
        }
    }
}
