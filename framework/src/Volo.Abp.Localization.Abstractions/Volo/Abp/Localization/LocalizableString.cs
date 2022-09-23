using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization;

public class LocalizableString : ILocalizableString, IAsyncLocalizableString
{
    [CanBeNull]
    public string ResourceName { get; }
    
    [CanBeNull]
    public Type ResourceType { get; }

    [NotNull]
    public string Name { get; }

    public LocalizableString([CanBeNull] Type resourceType, [NotNull] string name)
    {
        Name = Check.NotNullOrEmpty(name, nameof(name));
        ResourceType = resourceType;
        
        if (resourceType != null)
        {
            ResourceName = LocalizationResourceNameAttribute.GetName(resourceType);
        }
    }

    public LocalizableString([NotNull] string name, [CanBeNull] string resourceName = null)
    {
        Name = Check.NotNullOrEmpty(name, nameof(name));
        ResourceName = resourceName;
    }

    public LocalizedString Localize(IStringLocalizerFactory stringLocalizerFactory)
    {
        var localizer = CreateStringLocalizerOrNull(stringLocalizerFactory);
        if (localizer == null)
        {
            return new LocalizedString(Name, Name, resourceNotFound: true);
        }
        
        var result = localizer[Name];
        
        if (result.ResourceNotFound && ResourceName != null)
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
    
    public async Task<LocalizedString> LocalizeAsync(IStringLocalizerFactory stringLocalizerFactory)
    {
        var localizer = await CreateStringLocalizerOrNullAsync(stringLocalizerFactory);
        if (localizer == null)
        {
            throw new AbpException($"Set {nameof(ResourceName)} or configure the default localization resource type (in the AbpLocalizationOptions)!");
        }
        
        var result = localizer[Name];
        
        if (result.ResourceNotFound && ResourceName != null)
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

    private IStringLocalizer CreateStringLocalizerOrNull(IStringLocalizerFactory stringLocalizerFactory)
    {
        if (ResourceType != null)
        {
            return stringLocalizerFactory.Create(ResourceType);
        }
        
        if (ResourceName != null)
        {
            var localizerByName = stringLocalizerFactory.CreateByResourceNameOrNull(ResourceName);
            if (localizerByName != null)
            {
                return localizerByName;
            }
        }

        return stringLocalizerFactory.CreateDefaultOrNull();
    }
    
    private async Task<IStringLocalizer> CreateStringLocalizerOrNullAsync(IStringLocalizerFactory stringLocalizerFactory)
    {
        if (ResourceType != null)
        {
            return stringLocalizerFactory.Create(ResourceType);
        }
        
        if (ResourceName != null)
        {
            var localizerByName = await stringLocalizerFactory.CreateByResourceNameOrNullAsync(ResourceName);
            if (localizerByName != null)
            {
                return localizerByName;
            }
        }

        return stringLocalizerFactory.CreateDefaultOrNull();
    }

    public static LocalizableString Create<TResource>([NotNull] string name)
    {
        return Create(typeof(TResource), name);
    }
    
    public static LocalizableString Create(Type resourceType,[NotNull] string name)
    {
        return new LocalizableString(resourceType, name);
    }
    
    public static LocalizableString Create([NotNull] string name, [CanBeNull] string resourceName = null)
    {
        return new LocalizableString(name, resourceName);
    }
}
