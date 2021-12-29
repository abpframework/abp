using System;
using Volo.Abp.Localization;

namespace Volo.Abp.ObjectExtending.Modularity;

public static class ExtensionPropertyConfigurationExtensions
{
    public static string GetLocalizationResourceNameOrNull(
        this ExtensionPropertyConfiguration property)
    {
        var resourceType = property.GetLocalizationResourceTypeOrNull();
        if (resourceType == null)
        {
            return null;
        }

        return LocalizationResourceNameAttribute.GetName(resourceType);
    }

    public static Type GetLocalizationResourceTypeOrNull(
        this ExtensionPropertyConfiguration property)
    {
        if (property.DisplayName != null &&
            property.DisplayName is LocalizableString localizableString)
        {
            return localizableString.ResourceType;
        }

        return null;
    }
}
