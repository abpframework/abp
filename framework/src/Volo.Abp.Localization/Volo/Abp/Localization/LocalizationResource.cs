using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp.Localization;

public class LocalizationResource : LocalizationResourceBase
{
    [NotNull]
    public Type ResourceType { get; }

    public LocalizationResource(
        [NotNull] Type resourceType,
        string? defaultCultureName = null,
        ILocalizationResourceContributor? initialContributor = null)
        : base(
            LocalizationResourceNameAttribute.GetName(resourceType),
            defaultCultureName,
            initialContributor)
    {
        ResourceType = Check.NotNull(resourceType, nameof(resourceType));
        AddBaseResourceTypes();
    }

    protected virtual void AddBaseResourceTypes()
    {
        var descriptors = ResourceType
            .GetCustomAttributes(true)
            .OfType<IInheritedResourceTypesProvider>();

        foreach (var descriptor in descriptors)
        {
            foreach (var baseResourceType in descriptor.GetInheritedResourceTypes())
            {
                BaseResourceNames.AddIfNotContains(LocalizationResourceNameAttribute.GetName(baseResourceType));
            }
        }
    }
}