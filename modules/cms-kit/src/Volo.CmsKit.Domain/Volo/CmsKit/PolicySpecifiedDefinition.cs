using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace Volo.CmsKit
{
    public abstract class PolicySpecifiedDefinition : IEquatable<PolicySpecifiedDefinition>
    {
        protected PolicySpecifiedDefinition()
        {
        }

        public PolicySpecifiedDefinition(
            [NotNull] string entityType,
            [CanBeNull] string createPolicy = null,
            [CanBeNull] string updatePolicy = null,
            [CanBeNull] string deletePolicy = null)
        {
            EntityType = Check.NotNullOrEmpty(entityType, nameof(entityType));
            CreatePolicy = createPolicy;
            DeletePolicy = deletePolicy;
            UpdatePolicy = updatePolicy;
        }

        [NotNull]
        public string EntityType { get; set; }

        [CanBeNull]
        public virtual string CreatePolicy { get; set; }

        [CanBeNull]
        public virtual string UpdatePolicy { get; set; }

        [CanBeNull]
        public virtual string DeletePolicy { get; set; }

        public bool Equals(PolicySpecifiedDefinition other)
        {
            return other?.EntityType == EntityType;
        }
    }
}
