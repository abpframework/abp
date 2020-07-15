using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.Reactions
{
    public class ReactionManager : CmsKitDomainServiceBase
    {
        protected IReactionDefinitionStore ReactionDefinitionStore { get; }
        protected IUserReactionRepository UserReactionRepository { get; }

        public ReactionManager(
            IUserReactionRepository userReactionRepository,
            IReactionDefinitionStore reactionDefinitionStore)
        {
            UserReactionRepository = userReactionRepository;
            ReactionDefinitionStore = reactionDefinitionStore;
        }

        public virtual async Task<List<ReactionDefinition>> GetAvailableReactionsAsync(
            [CanBeNull] string entityType = null)
        {
            return await ReactionDefinitionStore.GetAvailableReactionsAsync(entityType);
        }

        public virtual async Task<List<ReactionSummary>> GetSummariesAsync(
            [NotNull] string entityType,
            [NotNull] string entityId)
        {
            var summaries = await UserReactionRepository.GetSummariesAsync(entityType, entityId);

            var summaryDtos = new List<ReactionSummary>();

            foreach (var summary in summaries)
            {
                var summaryDto = new ReactionSummary
                {
                    Count = summary.Count
                };

                //TODO: Get all definitions then filter here?
                var reactionDefinition = await ReactionDefinitionStore
                    .GetReactionOrNullAsync(
                        summary.ReactionName,
                        entityType
                    );

                if (reactionDefinition == null)
                {
                    continue;
                }

                summaryDto.Reaction = reactionDefinition;

                summaryDtos.Add(summaryDto);
            }

            return summaryDtos;
        }

        public virtual async Task<List<ReactionDefinition>> GetUserReactionsAsync(
            Guid userId,
            [NotNull] string entityType,
            [NotNull] string entityId)
        {
            var userReactions = await UserReactionRepository
                .GetListForUserAsync(
                    userId,
                    entityType,
                    entityId
                );

            var reactionDtos = new List<ReactionDefinition>();

            foreach (var userReaction in userReactions)
            {
                //TODO: Get all definitions then filter here?
                var reactionDefinition = await ReactionDefinitionStore
                    .GetReactionOrNullAsync(
                        userReaction.ReactionName,
                        userReaction.EntityType
                    );

                if (reactionDefinition == null)
                {
                    await UserReactionRepository.DeleteAsync(userReaction);
                    continue;
                }

                reactionDtos.Add(reactionDefinition);
            }

            return reactionDtos;
        }

        public virtual async Task<UserReaction> CreateAsync(
            Guid userId,
            [NotNull] string entityType,
            [NotNull] string entityId,
            [NotNull] string reactionName)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            Check.NotNullOrWhiteSpace(entityId, nameof(entityId));
            Check.NotNullOrWhiteSpace(reactionName, nameof(reactionName));

            var existingReaction = await UserReactionRepository.FindAsync(userId, entityType, entityId, reactionName);
            if (existingReaction != null)
            {
                return existingReaction;
            }

            return await UserReactionRepository.InsertAsync(
                new UserReaction(
                    GuidGenerator.Create(),
                    userId,
                    entityType,
                    entityId,
                    reactionName,
                    Clock.Now
                )
            );
        }

        public virtual async Task<bool> DeleteAsync(
            Guid userId,
            [NotNull] string entityType,
            [NotNull] string entityId,
            [NotNull] string reactionName)
        {
            var existingReaction = await UserReactionRepository.FindAsync(userId, entityType, entityId, reactionName);
            if (existingReaction == null)
            {
                return false;
            }

            await UserReactionRepository.DeleteAsync(existingReaction);
            return true;
        }
    }
}
