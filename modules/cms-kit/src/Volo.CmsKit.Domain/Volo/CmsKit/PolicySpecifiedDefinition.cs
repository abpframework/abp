using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Localization;

namespace Volo.CmsKit.Domain.Volo.CmsKit
{
    public abstract class PolicySpecifiedDefinition
    {
        protected PolicySpecifiedDefinition()
        {
        }

        public PolicySpecifiedDefinition(
            [CanBeNull] ILocalizableString displayName = null,
            [CanBeNull] string createPolicy = null,
            [CanBeNull] string updatePolicy = null,
            [CanBeNull] string deletePolicy = null)
        {
            DisplayName = displayName;
            CreatePolicy = createPolicy;
            DeletePolicy = deletePolicy;
            UpdatePolicy = updatePolicy;
        }

        [CanBeNull]
        public virtual ILocalizableString DisplayName { get; }

        [CanBeNull]
        public virtual string CreatePolicy { get; set; }

        [CanBeNull]
        public virtual string UpdatePolicy { get; set; }

        [CanBeNull]
        public virtual string DeletePolicy { get; set; }
    }
}
