using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Localization;

namespace Volo.CmsKit.Tags
{
    public class TagEntityTypeDefiniton : PolicySpecifiedDefinition, IEquatable<TagEntityTypeDefiniton>
    {
        [CanBeNull]
        public virtual ILocalizableString DisplayName { get; }

        public TagEntityTypeDefiniton(
            [NotNull] string entityType,
            [CanBeNull] ILocalizableString displayName = null,
            IEnumerable<string> createPolicies = null,
            IEnumerable<string> updatePolicies = null,
            IEnumerable<string> deletePolicies = null) : base(entityType, createPolicies, updatePolicies, deletePolicies)
        {
            DisplayName = displayName;
        }

        public bool Equals(TagEntityTypeDefiniton other)
        {
            return EntityType == other?.EntityType;
        }
    }
}
