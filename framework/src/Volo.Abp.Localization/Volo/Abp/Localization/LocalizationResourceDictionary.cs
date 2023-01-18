using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Localization;

public class LocalizationResourceDictionary : Dictionary<string, LocalizationResourceBase>
{
    private readonly Dictionary<Type, LocalizationResourceBase> _resourcesByTypes = new();

    public LocalizationResource Add<TResouce>([CanBeNull] string defaultCultureName = null)
    {
        return Add(typeof(TResouce), defaultCultureName);
    }

    public LocalizationResource Add(Type resourceType, [CanBeNull] string defaultCultureName = null)
    {
        var resourceName = LocalizationResourceNameAttribute.GetName(resourceType);
        if (ContainsKey(resourceName))
        {
            throw new AbpException("This resource is already added before: " + resourceType.AssemblyQualifiedName);
        }

        var resource = new LocalizationResource(resourceType, defaultCultureName);

        this[resourceName] = resource;
        _resourcesByTypes[resourceType] = resource;

        return resource;
    }
    
    public NonTypedLocalizationResource Add([NotNull] string resourceName, [CanBeNull] string defaultCultureName = null)
    {
        Check.NotNullOrWhiteSpace(resourceName, nameof(resourceName));
        
        if (ContainsKey(resourceName))
        {
            throw new AbpException("This resource is already added before: " + resourceName);
        }

        var resource = new NonTypedLocalizationResource(resourceName, defaultCultureName);

        this[resourceName] = resource;

        return resource;
    }

    public LocalizationResourceBase Get<TResource>()
    {
        var resourceType = typeof(TResource);

        var resource = _resourcesByTypes.GetOrDefault(resourceType);
        if (resource == null)
        {
            throw new AbpException("Can not find a resource with given type: " + resourceType.AssemblyQualifiedName);
        }

        return resource;
    }

    public LocalizationResourceBase Get(string resourceName)
    {
        var resource = this.GetOrDefault(resourceName);
        if (resource == null)
        {
            throw new AbpException("Can not find a resource with given name: " + resourceName);
        }

        return resource;
    }
    
    public LocalizationResourceBase Get(Type resourceType)
    {
        var resource = GetOrNull(resourceType);
        if (resource == null)
        {
            throw new AbpException("Can not find a resource with given type: " + resourceType);
        }

        return resource;
    }

    public LocalizationResourceBase GetOrNull(Type resourceType)
    {
        return _resourcesByTypes.GetOrDefault(resourceType);
    }
    
    public bool ContainsResource(Type resourceType)
    {
        return _resourcesByTypes.ContainsKey(resourceType);
    }
}