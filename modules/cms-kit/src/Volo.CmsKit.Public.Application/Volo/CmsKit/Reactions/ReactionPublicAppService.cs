using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Volo.CmsKit.Reactions
{
    //TODO: Authorization
    public class ReactionPublicAppService : CmsKitPublicAppService, IReactionPublicAppService
    {
        protected IReactionDefinitionStore ReactionDefinitionStore { get; }

        protected IUserReactionRepository UserReactionRepository { get; }

        protected ReactionManager ReactionManager { get; }

        public ReactionPublicAppService(
            IReactionDefinitionStore reactionDefinitionStore,
            IUserReactionRepository userReactionRepository,
            ReactionManager reactionManager)
        {
            ReactionDefinitionStore = reactionDefinitionStore;
            UserReactionRepository = userReactionRepository;
            ReactionManager = reactionManager;
        }

        public virtual async Task<ListResultDto<ReactionWithSelectionDto>> GetForSelectionAsync(string entityType, string entityId)
        {
            var reactionDefinitions = await ReactionManager
                .GetAvailableReactionsAsync(
                    entityType
                );

            var summaries =
                (await ReactionManager.GetSummariesAsync(entityType, entityId))
                .ToDictionary(x => x.Reaction.Name, x => x.Count);

            var userReactions = await ReactionManager.GetUserReactionsAsync(
                CurrentUser.GetId(),
                entityType,
                entityId
            );

            var reactionDtos = new List<ReactionWithSelectionDto>();

            foreach (var reactionDefinition in reactionDefinitions)
            {
                reactionDtos.Add(
                    new ReactionWithSelectionDto
                    {
                        Reaction = ConvertToReactionDto(reactionDefinition),
                        Count = summaries.GetOrDefault(reactionDefinition.Name),
                        IsSelectedByCurrentUser = userReactions.Any(x => x.Name == reactionDefinition.Name)
                    }
                );
            }

            return new ListResultDto<ReactionWithSelectionDto>(reactionDtos);
        }

        public virtual async Task CreateAsync(CreateReactionDto input)
        {
            await ReactionManager.CreateAsync(
                CurrentUser.GetId(),
                input.EntityType,
                input.EntityId,
                input.ReactionName
            );
        }

        public virtual async Task DeleteAsync(DeleteReactionDto input)
        {
            await ReactionManager.DeleteAsync(
                CurrentUser.GetId(),
                input.EntityType,
                input.EntityId,
                input.ReactionName
            );
        }

        private ReactionDto ConvertToReactionDto(ReactionDefinition reactionDefinition)
        {
            return new ReactionDto
            {
                Name = reactionDefinition.Name,
                DisplayName = reactionDefinition.DisplayName?.Localize(StringLocalizerFactory)
            };
        }
    }
}
