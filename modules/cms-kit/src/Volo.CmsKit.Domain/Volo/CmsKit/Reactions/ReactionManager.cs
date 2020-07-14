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
            [CanBeNull] string entityType = null,
            [CanBeNull] Guid? userId = null)
        {
            return await ReactionDefinitionStore.GetAvailableReactionsAsync(entityType, userId);
        }

        public virtual Task<ReactionSummary> GetSummariesAsync(
            [NotNull] string entityType,
            [NotNull] string entityId)
        {
            //TODO: ...
            throw new NotImplementedException();
        }

        public virtual Task<ReactionDefinition> GetUserReactionsAsync(
            Guid userId,
            [NotNull] string entityType,
            [NotNull] string entityId)
        {
            //TODO: ...
            throw new NotImplementedException();
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

    public class ReactionSummary
    {
        public ReactionDefinition Reaction { get; set; }

        public int Count { get; set; }
    }
}
