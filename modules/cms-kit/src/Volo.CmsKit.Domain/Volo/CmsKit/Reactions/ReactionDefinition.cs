using JetBrains.Annotations;
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

        [NotNull]
        public LocalizableIconDictionary Icons { get; }

        public ReactionDefinition(
            [NotNull] string name,
            [NotNull] string defaultIcon,
            [CanBeNull] ILocalizableString displayName = null)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            DisplayName = displayName;

            Icons = new LocalizableIconDictionary
            {
                Default = Check.NotNullOrWhiteSpace(defaultIcon, nameof(defaultIcon))
            };
        }
    }
}