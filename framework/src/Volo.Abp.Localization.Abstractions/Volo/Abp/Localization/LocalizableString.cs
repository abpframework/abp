using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization;

public class LocalizableString : ILocalizableString
{
    [CanBeNull]
    public Type ResourceType { get; }

    [NotNull]
    public string Name { get; }

    public LocalizableString([CanBeNull] Type resourceType, [NotNull] string name)
    {
        Name = Check.NotNullOrEmpty(name, nameof(name));
        ResourceType = resourceType;
    }

    public LocalizedString Localize(IStringLocalizerFactory stringLocalizerFactory)
    {
        var localizer = ResourceType != null
            ? stringLocalizerFactory.Create(ResourceType)
            : stringLocalizerFactory.CreateDefaultOrNull();

        if (localizer == null)
        {
            throw new AbpException($"Set {nameof(ResourceType)} or configure the default localization resource type (in the AbpLocalizationOptions)!");
        }
        
        var result = localizer[Name];
        
        if (result.ResourceNotFound && ResourceType != null)
        {
            /* Search in the default resource if not found in the provided resource */
            localizer = stringLocalizerFactory.CreateDefaultOrNull();
            if (localizer != null)
            {
                result = localizer[Name];
            }
        }

        return result;
    }

    public static LocalizableString Create<TResource>([NotNull] string name)
    {
        return new LocalizableString(typeof(TResource), name);
    }
}
