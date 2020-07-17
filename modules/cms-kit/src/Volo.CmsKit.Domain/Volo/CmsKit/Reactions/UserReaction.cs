using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Volo.CmsKit.Reactions
{
    public class UserReaction : Entity<Guid>, IAggregateRoot<Guid>
    {
        public virtual string EntityType { get; protected set; }

        public virtual string EntityId { get; protected set; }

        public virtual string ReactionName { get; protected set; }

        public virtual DateTime CreationTime { get; protected set; }

        public virtual Guid UserId { get; protected set; }

        protected UserReaction()
        {

        }

        internal UserReaction(
            Guid id,
            Guid userId,
            [NotNull] string entityType,
            [NotNull] string entityId,
            [NotNull] string reactionName,
            DateTime creationTime)
            : base(id)
        {
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            EntityId = Check.NotNullOrWhiteSpace(entityId, nameof(entityId));
            ReactionName = Check.NotNullOrWhiteSpace(reactionName, nameof(reactionName));
            UserId = userId;
            CreationTime = creationTime;
        }
    }
}
