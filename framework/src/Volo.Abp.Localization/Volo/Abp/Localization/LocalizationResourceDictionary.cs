using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Localization;

public class LocalizationResourceDictionary : Dictionary<Type, LocalizationResource>
{
    private readonly Dictionary<string, LocalizationResource> _resourcesByNames = new();

    public LocalizationResource Add<TResouce>([CanBeNull] string defaultCultureName = null)
    {
        return Add(typeof(TResouce), defaultCultureName);
    }

    public LocalizationResource Add(Type resourceType, [CanBeNull] string defaultCultureName = null)
    {
        if (ContainsKey(resourceType))
        {
            throw new AbpException("This resource is already added before: " + resourceType.AssemblyQualifiedName);
        }

        var resource = new LocalizationResource(resourceType, defaultCultureName);

        this[resourceType] = resource;
        _resourcesByNames[resource.ResourceName] = resource;

        return resource;
    }

    public LocalizationResource Get<TResource>()
    {
        var resourceType = typeof(TResource);

        var resource = this.GetOrDefault(resourceType);
        if (resource == null)
        {
            throw new AbpException("Can not find a resource with given type: " + resourceType.AssemblyQualifiedName);
        }

        return resource;
    }

    public LocalizationResource Get(string resourceName)
    {
        var resource = GetOrNull(resourceName);
        if (resource == null)
        {
            throw new AbpException("Can not find a resource with given name: " + resourceName);
        }

        return resource;
    }

    public LocalizationResource GetOrNull(string resourceName)
    {
        return _resourcesByNames.GetOrDefault(resourceName);
    }
}