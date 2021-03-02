using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Localization;

namespace Volo.CmsKit.Tags
{
    public class TagEntityTypeDefiniton : PolicySpecifiedDefinition, IEquatable<TagEntityTypeDefiniton>
    {
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
            [CanBeNull] string deletePolicy = null) : base(entityType, createPolicy, updatePolicy, deletePolicy)
        {
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            DisplayName = displayName;
        }

        public bool Equals(TagEntityTypeDefiniton other)
        {
            return EntityType == other?.EntityType;
        }
    }
}
