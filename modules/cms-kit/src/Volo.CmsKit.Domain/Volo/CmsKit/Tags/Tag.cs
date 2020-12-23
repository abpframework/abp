using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Tags
{
    public class Tag : BasicAggregateRoot<Guid>, IMultiTenant, IHasCreationTime
    {
        protected Tag()
        {
        }

        public Tag(
            [NotNull] string name,
            [CanBeNull] string colorHex,
            [NotNull] Guid? tenantId = null)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), TagConsts.MaxNameLength);
            ColorHex = Check.Length(colorHex, nameof(colorHex), TagConsts.MaxColorHexLength);
            TenantId = tenantId;
        }

        public virtual string Name { get; set; }
        public virtual string ColorHex { get; set; }
        public Guid? TenantId { get; }
        public DateTime CreationTime { get; }
    }
}
