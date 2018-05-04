using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Localization.VirtualFiles.Json;

namespace Volo.Abp.Localization
{
    public static class LocalizationResourceExtensions
    {
        public static LocalizationResource AddVirtualJson(
            [NotNull] this LocalizationResource localizationResource,
            [NotNull] string virtualPath)
        {
            Check.NotNull(localizationResource, nameof(localizationResource));
            Check.NotNull(virtualPath, nameof(virtualPath));

            localizationResource.Contributors.Add(new JsonVirtualFileLocalizationResourceContributor(
                virtualPath.EnsureStartsWith('/')
            ));

            return localizationResource;
        }

        public static LocalizationResource AddBaseTypes(
            [NotNull] this LocalizationResource localizationResource,
            [NotNull] params Type[] types)
        {
            Check.NotNull(localizationResource, nameof(localizationResource));
            Check.NotNull(types, nameof(types));

            foreach (var type in types)
            {
                localizationResource.BaseResourceTypes.AddIfNotContains(type);
            }

            return localizationResource;
        }

        //public static LocalizationResource AddVirtualJson<TResource>(
        //    [NotNull] this LocalizationResourceDictionary resourceDictionary,
        //    [CanBeNull] string defaultCultureName,
        //    [NotNull] string virtualPath)
        //{
        //    Check.NotNull(resourceDictionary, nameof(resourceDictionary));
        //    Check.NotNull(virtualPath, nameof(virtualPath));

        //    return resourceDictionary
        //        .Add<TResource>(defaultCultureName)
        //        .AddVirtualJson(virtualPath);
        //}

        //public static LocalizationResource ExtendWithVirtualJson<TResource>(
        //    [NotNull] this LocalizationResourceDictionary resourceDictionary,
        //    [NotNull] string virtualPath)
        //{
        //    Check.NotNull(resourceDictionary, nameof(resourceDictionary));

        //    return resourceDictionary
        //        .Get<TResource>()
        //        .AddVirtualJson(virtualPath);
        //}

        //public static LocalizationResource AddBaseTypes<TResource>(
        //    [NotNull] this LocalizationResourceDictionary resourceDictionary,
        //    [NotNull] params Type[] types)
        //{
        //    Check.NotNull(resourceDictionary, nameof(resourceDictionary));
        //    Check.NotNull(types, nameof(types));

        //    var localizationResource = resourceDictionary.Get<TResource>();

        //    foreach (var type in types)
        //    {
        //        localizationResource.BaseResourceTypes.AddIfNotContains(type);
        //    }

        //    return localizationResource;
        //}
    }
}