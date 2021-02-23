using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Contents
{
    public class Content : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        [CanBeNull] public virtual Guid? TenantId { get; protected set; }

        [NotNull] public virtual string EntityType { get; protected set; }

        [NotNull] public virtual string EntityId { get; protected set; }

        [NotNull] public virtual string Value { get; protected set; }

        protected Content()
        {
        }

        public Content(Guid id, [NotNull] string entityType, [NotNull] string entityId, [NotNull] string value,
            Guid? tenantId = null) : base(id)
        {
            EntityId = Check.NotNullOrWhiteSpace(entityId, nameof(entityId), ContentConsts.MaxEntityIdLength);
            EntityType = Check.NotNullOrEmpty(entityType, nameof(entityType), ContentConsts.MaxEntityTypeLength);
            Value = Check.NotNullOrEmpty(value, nameof(value), ContentConsts.MaxValueLength);

            TenantId = tenantId;
        }

        public virtual void SetValue([NotNull] string value)
        {
            Value = Check.NotNullOrEmpty(value, nameof(value), ContentConsts.MaxValueLength);
        }

        public virtual void SetEntity([NotNull] string entityType, [NotNull] string entityId)
        {
            EntityType = Check.NotNullOrEmpty(entityType, nameof(entityType), ContentConsts.MaxEntityTypeLength);
            EntityId = Check.NotNullOrWhiteSpace(entityId, nameof(entityId), ContentConsts.MaxEntityIdLength);
        }
    }
}