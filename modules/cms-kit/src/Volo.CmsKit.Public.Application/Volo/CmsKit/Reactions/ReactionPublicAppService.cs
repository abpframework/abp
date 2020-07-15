using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Reactions
{
    public class ReactionPublicAppService : CmsKitPublicAppService, IReactionPublicAppService
    {
        protected IReactionDefinitionStore ReactionDefinitionStore { get; }

        public ReactionPublicAppService(IReactionDefinitionStore reactionDefinitionStore)
        {
            ReactionDefinitionStore = reactionDefinitionStore;
        }

        public virtual async Task<ListResultDto<ReactionDto>> GetAvailableReactions(
            GetAvailableReactionsDto input)
        {
            var reactionDefinitions = await ReactionDefinitionStore.GetAvailableReactionsAsync(input.EntityType, CurrentUser.Id);

            var reactionDtos = reactionDefinitions
                .Select(reactionDefinition => new ReactionDto
                {
                    Name = reactionDefinition.Name,
                    DisplayName = reactionDefinition.DisplayName?.Localize(StringLocalizerFactory)
                }).ToList();

            return new ListResultDto<ReactionDto>(reactionDtos);
        }
    }
}
