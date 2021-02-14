using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.CmsKit.Domain.Volo.CmsKit;

namespace Volo.CmsKit.Tags
{
    public class TagEntityTypeDefiniton : PolicySpecifiedDefinition, IEquatable<TagEntityTypeDefiniton>
    {
        public string EntityType { get; }

        [CanBeNull]
        public virtual ILocalizableString DisplayName { get; }

        protected TagEntityTypeDefiniton()
        {
        }

        public TagEntityTypeDefiniton(
            [NotNull] string entityType,
            [CanBeNull] ILocalizableString displayName = null,
            [CanBeNull] string createPolicy = null,
            [CanBeNull] string updatePolicy = null,
            [CanBeNull] string deletePolicy = null) : base(createPolicy, updatePolicy, deletePolicy)
        {
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

            DisplayName = displayName;
        }

        public bool Equals(TagEntityTypeDefiniton other)
        {
            return EntityType == other.EntityType;
        }
    }
}
