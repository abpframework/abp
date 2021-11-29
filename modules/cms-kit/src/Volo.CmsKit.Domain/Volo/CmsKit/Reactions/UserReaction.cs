using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Reactions
{
    public class UserReaction : BasicAggregateRoot<Guid>, IHasCreationTime, IMustHaveCreator, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        public virtual string EntityType { get; protected set; }

        public virtual string EntityId { get; protected set; }

        public virtual string ReactionName { get; protected set; }

        public virtual Guid CreatorId { get; set; }

        public virtual DateTime CreationTime { get; set; }

        protected UserReaction()
        {

        }

        internal UserReaction(
            Guid id,
            [NotNull] string entityType,
            [NotNull] string entityId,
            [NotNull] string reactionName,
            Guid creatorId,
            Guid? tenantId = null)
            : base(id)
        {
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            EntityId = Check.NotNullOrWhiteSpace(entityId, nameof(entityId));
            ReactionName = Check.NotNullOrWhiteSpace(reactionName, nameof(reactionName));
            CreatorId = creatorId;
            TenantId = tenantId;
        }
    }
}
