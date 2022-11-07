using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Localization.VirtualFiles.Json;

namespace Volo.Abp.Localization;

public static class LocalizationResourceExtensions
{
    public static TLocalizationResource AddVirtualJson<TLocalizationResource>(
        [NotNull] this TLocalizationResource localizationResource,
        [NotNull] string virtualPath)
        where TLocalizationResource : LocalizationResourceBase
    {
        Check.NotNull(localizationResource, nameof(localizationResource));
        Check.NotNull(virtualPath, nameof(virtualPath));

        localizationResource.Contributors.Add(new JsonVirtualFileLocalizationResourceContributor(
            virtualPath.EnsureStartsWith('/')
        ));

        return localizationResource;
    }

    public static TLocalizationResource AddBaseTypes<TLocalizationResource>(
        [NotNull] this TLocalizationResource localizationResource,
        [NotNull] params Type[] types)
        where TLocalizationResource : LocalizationResourceBase
    {
        Check.NotNull(localizationResource, nameof(localizationResource));
        Check.NotNull(types, nameof(types));

        foreach (var type in types)
        {
            localizationResource
                .BaseResourceNames
                .AddIfNotContains(LocalizationResourceNameAttribute.GetName(type));
        }

        return localizationResource;
    }
    
    public static TLocalizationResource AddBaseResources<TLocalizationResource>(
        [NotNull] this TLocalizationResource localizationResource,
        [NotNull] params string[] baseResourceNames)
        where TLocalizationResource : LocalizationResourceBase
    {
        Check.NotNull(localizationResource, nameof(localizationResource));
        Check.NotNull(baseResourceNames, nameof(baseResourceNames));

        foreach (var baseResourceName in baseResourceNames)
        {
            localizationResource.BaseResourceNames.AddIfNotContains(baseResourceName);
        }

        return localizationResource;
    }
}
