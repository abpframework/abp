using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionCommonExtensions
{
    public static bool IsAdded<T>(this IServiceCollection services)
    {
        return services.IsAdded(typeof(T));
    }

    public static bool IsAdded(this IServiceCollection services, Type type)
    {
        return services.Any(d => d.ServiceType == type);
    }

    public static ObjectAccessorCollection GetObjectAccessorCollection(this IServiceCollection services)
    {
        var implementationInstance = (ObjectAccessorCollection)services
            .FirstOrDefault(d => d.ServiceType == typeof(ObjectAccessorCollection))
            ?.ImplementationInstance;

        if (implementationInstance != null)
        {
            return implementationInstance;
        }

        if (services.IsReadOnly)
        {
            throw new AbpException("The ObjectAccessorCollection does not exist and the IServiceCollection cannot be modified because it is read-only.");
        }

        implementationInstance = new ObjectAccessorCollection();
        services.AddSingleton(implementationInstance);
        return implementationInstance;
    }

    public static T GetSingletonInstanceOrNull<T>(this IServiceCollection services)
    {
        return (T)services
            .FirstOrDefault(d => d.ServiceType == typeof(T))
            ?.ImplementationInstance;
    }

    public static T GetSingletonInstance<T>(this IServiceCollection services)
    {
        var service = services.GetSingletonInstanceOrNull<T>();
        if (service == null)
        {
            throw new InvalidOperationException("Could not find singleton service: " + typeof(T).AssemblyQualifiedName);
        }

        return service;
    }

    public static IServiceProvider BuildServiceProviderFromFactory([NotNull] this IServiceCollection services)
    {
        Check.NotNull(services, nameof(services));

        foreach (var service in services)
        {
            var factoryInterface = service.ImplementationInstance?.GetType()
                .GetTypeInfo()
                .GetInterfaces()
                .FirstOrDefault(i => i.GetTypeInfo().IsGenericType &&
                                     i.GetGenericTypeDefinition() == typeof(IServiceProviderFactory<>));

            if (factoryInterface == null)
            {
                continue;
            }

            var containerBuilderType = factoryInterface.GenericTypeArguments[0];
            return (IServiceProvider)typeof(ServiceCollectionCommonExtensions)
                .GetTypeInfo()
                .GetMethods()
                .Single(m => m.Name == nameof(BuildServiceProviderFromFactory) && m.IsGenericMethod)
                .MakeGenericMethod(containerBuilderType)
                .Invoke(null, new object[] { services, null });
        }

        return services.BuildServiceProvider();
    }

    public static IServiceProvider BuildServiceProviderFromFactory<TContainerBuilder>([NotNull] this IServiceCollection services, Action<TContainerBuilder> builderAction = null)
    {
        Check.NotNull(services, nameof(services));

        var serviceProviderFactory = services.GetSingletonInstanceOrNull<IServiceProviderFactory<TContainerBuilder>>();
        if (serviceProviderFactory == null)
        {
            throw new AbpException($"Could not find {typeof(IServiceProviderFactory<TContainerBuilder>).FullName} in {services}.");
        }

        var builder = serviceProviderFactory.CreateBuilder(services);
        builderAction?.Invoke(builder);
        return serviceProviderFactory.CreateServiceProvider(builder);
    }

    /// <summary>
    /// Resolves a dependency using given <see cref="IServiceCollection"/>.
    /// This method should be used only after dependency injection registration phase completed.
    /// </summary>
    internal static T GetService<T>(this IServiceCollection services)
    {
        return services
            .GetSingletonInstance<IAbpApplication>()
            .ServiceProvider
            .GetService<T>();
    }

    /// <summary>
    /// Resolves a dependency using given <see cref="IServiceCollection"/>.
    /// This method should be used only after dependency injection registration phase completed.
    /// </summary>
    internal static object GetService(this IServiceCollection services, Type type)
    {
        return services
            .GetSingletonInstance<IAbpApplication>()
            .ServiceProvider
            .GetService(type);
    }

    /// <summary>
    /// Resolves a dependency using given <see cref="IServiceCollection"/>.
    /// Throws exception if service is not registered.
    /// This method should be used only after dependency injection registration phase completed.
    /// </summary>
    public static T GetRequiredService<T>(this IServiceCollection services)
    {
        return services
            .GetSingletonInstance<IAbpApplication>()
            .ServiceProvider
            .GetRequiredService<T>();
    }

    /// <summary>
    /// Resolves a dependency using given <see cref="IServiceCollection"/>.
    /// Throws exception if service is not registered.
    /// This method should be used only after dependency injection registration phase completed.
    /// </summary>
    public static object GetRequiredService(this IServiceCollection services, Type type)
    {
        return services
            .GetSingletonInstance<IAbpApplication>()
            .ServiceProvider
            .GetRequiredService(type);
    }

    /// <summary>
    /// Returns a <see cref="Lazy{T}"/> to resolve a service from given <see cref="IServiceCollection"/>
    /// once dependency injection registration phase completed.
    /// </summary>
    public static Lazy<T> GetServiceLazy<T>(this IServiceCollection services)
    {
        return new Lazy<T>(services.GetService<T>, true);
    }

    /// <summary>
    /// Returns a <see cref="Lazy{T}"/> to resolve a service from given <see cref="IServiceCollection"/>
    /// once dependency injection registration phase completed.
    /// </summary>
    public static Lazy<object> GetServiceLazy(this IServiceCollection services, Type type)
    {
        return new Lazy<object>(() => services.GetService(type), true);
    }

    /// <summary>
    /// Returns a <see cref="Lazy{T}"/> to resolve a service from given <see cref="IServiceCollection"/>
    /// once dependency injection registration phase completed.
    /// </summary>
    public static Lazy<T> GetRequiredServiceLazy<T>(this IServiceCollection services)
    {
        return new Lazy<T>(services.GetRequiredService<T>, true);
    }

    /// <summary>
    /// Returns a <see cref="Lazy{T}"/> to resolve a service from given <see cref="IServiceCollection"/>
    /// once dependency injection registration phase completed.
    /// </summary>
    public static Lazy<object> GetRequiredServiceLazy(this IServiceCollection services, Type type)
    {
        return new Lazy<object>(() => services.GetRequiredService(type), true);
    }

    public static IServiceProvider GetServiceProviderOrNull(this IServiceCollection services)
    {
        return services.GetObjectOrNull<IServiceProvider>();
    }
}
