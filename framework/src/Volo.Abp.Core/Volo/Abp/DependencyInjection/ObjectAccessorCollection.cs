using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.DependencyInjection;

public class ObjectAccessorCollection
{
    private readonly List<ServiceDescriptor> _serviceDescriptors;

    public ObjectAccessorCollection()
    {
        _serviceDescriptors = new List<ServiceDescriptor>();
    }

    public void Insert(int index, ServiceDescriptor serviceDescriptor)
    {
        _serviceDescriptors.Insert(index, serviceDescriptor);
    }

    public void Add(ServiceDescriptor serviceDescriptor)
    {
        _serviceDescriptors.Add(serviceDescriptor);
    }

    public void Remove(ServiceDescriptor serviceDescriptor)
    {
        _serviceDescriptors.Remove(serviceDescriptor);
    }

    public void RemoveAll(Predicate<ServiceDescriptor> predicate)
    {
        _serviceDescriptors.RemoveAll(predicate);
    }

    public IEnumerable<ServiceDescriptor> GetAll()
    {
        return _serviceDescriptors.ToImmutableList();
    }

    public T GetService<T>()
    {
        return (T)GetService(typeof(T));
    }

    public object GetService(Type serviceType)
    {
        return _serviceDescriptors.FirstOrDefault(x => x.ServiceType == serviceType)?.ImplementationInstance;
    }

    public T GetRequiredService<T>()
    {
        return (T)GetRequiredService(typeof(T));
    }

    public object GetRequiredService(Type serviceType)
    {
        return _serviceDescriptors.First(x => x.ServiceType == serviceType).ImplementationInstance ??
               throw new AbpException($"No service for type '{serviceType.FullName}' has been added");
    }
}
