using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Localization;

namespace Volo.CmsKit.Reactions
{
    public class ReactionDefinition
    {
        [NotNull]
        public string Name { get; }

        [CanBeNull]
        public ILocalizableString DisplayName { get; set; }

        public ReactionDefinition(
            [NotNull] string name,
            [CanBeNull] ILocalizableString displayName = null)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            DisplayName = displayName;
        }
    }
}
