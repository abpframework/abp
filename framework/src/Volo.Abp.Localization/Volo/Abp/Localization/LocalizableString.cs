using System;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization
{
    public class LocalizableString : ILocalizableString
    {
        public Type ResourceType { get; }

        public string Name { get; }

        public LocalizableString(Type resourceType, string name)
        {
            ResourceType = resourceType;
            Name = name;
        }

        public LocalizedString Localize(IStringLocalizerFactory stringLocalizerFactory)
        {
            return stringLocalizerFactory.Create(ResourceType)[Name];
        }

        public static LocalizableString Create<TResource>(string name)
        {
            return new LocalizableString(typeof(TResource), name);
        }
    }
}