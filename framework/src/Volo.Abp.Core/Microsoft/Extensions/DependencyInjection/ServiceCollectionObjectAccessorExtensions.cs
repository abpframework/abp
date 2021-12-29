using System;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionObjectAccessorExtensions
{
    public static ObjectAccessor<T> TryAddObjectAccessor<T>(this IServiceCollection services)
    {
        var service = services.GetObjectOrNull<ObjectAccessor<T>>();
        return service ?? services.AddObjectAccessor<T>();
    }

    public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services)
    {
        return services.AddObjectAccessor(new ObjectAccessor<T>());
    }

    public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services, T obj)
    {
        return services.AddObjectAccessor(new ObjectAccessor<T>(obj));
    }

    public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services, ObjectAccessor<T> accessor)
    {
        var service = services.GetObjectOrNull<ObjectAccessor<T>>();
        if (service != null)
        {
            throw new Exception("An object accessor is registered before for type: " + typeof(T).AssemblyQualifiedName);
        }

        services.GetObjectAccessorCollection().Add(ServiceDescriptor.Singleton(typeof(ObjectAccessor<T>), accessor));
        services.GetObjectAccessorCollection().Add(ServiceDescriptor.Singleton(typeof(IObjectAccessor<T>), accessor));

        return accessor;
    }

    public static T GetObjectOrNull<T>(this IServiceCollection services)
        where T : class
    {
        return services.GetObjectAccessorCollection().GetService<T>();
    }

    public static T GetObject<T>(this IServiceCollection services)
        where T : class
    {
        return services.GetObjectOrNull<T>() ?? throw new Exception($"Could not find an object of {typeof(T).AssemblyQualifiedName} in services. Be sure that you have used AddObjectAccessor before!");
    }
}
