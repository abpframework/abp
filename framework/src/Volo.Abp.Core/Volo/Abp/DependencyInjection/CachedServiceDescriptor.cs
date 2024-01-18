using System;

namespace Volo.Abp.DependencyInjection;

public class CachedServiceDescriptor
{
    private object? Key { get; }

    private Type ServiceType { get; }

    public CachedServiceDescriptor(object? key, Type serviceType)
    {
        Key = key;
        ServiceType = serviceType;
    }

    public override bool Equals(object? obj)
    {
        return obj is CachedServiceDescriptor descriptor &&
               Key == descriptor.Key &&
               ServiceType == descriptor.ServiceType;
    }

    public override int GetHashCode()
    {
        var keyHashCode = Key?.GetHashCode() ?? 0;
        return keyHashCode ^ ServiceType.GetHashCode();
    }
}
