using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization
{
    public class LocalizableString : ILocalizableString
    {
        [CanBeNull]
        public Type ResourceType { get; }

        [NotNull]
        public string Name { get; }

        public LocalizableString(Type resourceType, [NotNull] string name)
        {
            Name = Check.NotNullOrEmpty(name, nameof(name));
            ResourceType = resourceType;
        }

        public LocalizedString Localize(IStringLocalizerFactory stringLocalizerFactory)
        {
            return stringLocalizerFactory.Create(ResourceType)[Name];
        }

        public static LocalizableString Create<TResource>([NotNull] string name)
        {
            return new LocalizableString(typeof(TResource), name);
        }
    }
}