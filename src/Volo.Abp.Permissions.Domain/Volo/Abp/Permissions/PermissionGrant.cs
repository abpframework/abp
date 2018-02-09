using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Permissions
{
    public class PermissionGrant : Entity<Guid>
    {
        [NotNull]
        public virtual string Name { get; protected set; }

        public virtual bool IsGranted { get; internal set; }

        [CanBeNull]
        public virtual string ProviderName { get; protected set; }

        [CanBeNull]
        public virtual string ProviderKey { get; protected set; }

        protected PermissionGrant()
        {

        }

        public PermissionGrant(
            Guid id,
            [NotNull] string name,
            bool isGranted,
            [CanBeNull] string providerName = null,
            [CanBeNull] string providerKey = null)
        {
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = name;
            IsGranted = isGranted;
            ProviderName = providerName;
            ProviderKey = providerKey;
        }
    }
}
