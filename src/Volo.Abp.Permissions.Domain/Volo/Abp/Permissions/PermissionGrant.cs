using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Permissions
{
    public class PermissionGrant : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        [NotNull]
        public virtual string Name { get; protected set; }

        [NotNull]
        public virtual string ProviderName { get; protected set; }

        [NotNull]
        public virtual string ProviderKey { get; protected set; }

        protected PermissionGrant()
        {

        }

        public PermissionGrant(
            Guid id,
            [NotNull] string name,
            [NotNull] string providerName ,
            [NotNull] string providerKey,
            Guid? tenantId = null)
        {
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = name;
            ProviderName = providerName;
            ProviderKey = providerKey;
            TenantId = tenantId;
        }
    }
}
