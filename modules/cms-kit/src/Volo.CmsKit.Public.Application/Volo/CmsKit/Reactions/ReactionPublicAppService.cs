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
            var summaries = await ReactionManager.GetSummariesAsync(entityType, entityId);

            var userReactions = (await UserReactionRepository
                .GetListForUserAsync(
                    CurrentUser.GetId(),
                    entityType,
                    entityId
                )).ToDictionary(x => x.ReactionName, x => x);

            var reactionWithSelectionDtos = new List<ReactionWithSelectionDto>();

            foreach (var summary in summaries)
            {
                reactionWithSelectionDtos.Add(
                    new ReactionWithSelectionDto
                    {
                        Reaction = ConvertToReactionDto(summary.Reaction),
                        Count = summary.Count,
                        IsSelectedByCurrentUser = userReactions.ContainsKey(summary.Reaction.Name)
                    }
                );
            }

            return new ListResultDto<ReactionWithSelectionDto>(reactionWithSelectionDtos);
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
