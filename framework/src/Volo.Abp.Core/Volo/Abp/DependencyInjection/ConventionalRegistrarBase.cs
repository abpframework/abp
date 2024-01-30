using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Reflection;

namespace Volo.Abp.DependencyInjection;

public abstract class ConventionalRegistrarBase : IConventionalRegistrar
{
    public virtual void AddAssembly(IServiceCollection services, Assembly assembly)
    {
        var types = AssemblyHelper
            .GetAllTypes(assembly)
            .Where(
                type => type != null &&
                        type.IsClass &&
                        !type.IsAbstract &&
                        !type.IsGenericType
            ).ToArray();

        AddTypes(services, types);
    }

    public virtual void AddTypes(IServiceCollection services, params Type[] types)
    {
        foreach (var type in types)
        {
            AddType(services, type);
        }
    }

    public abstract void AddType(IServiceCollection services, Type type);

    protected virtual bool IsConventionalRegistrationDisabled(Type type)
    {
        return type.IsDefined(typeof(DisableConventionalRegistrationAttribute), true);
    }

    protected virtual void TriggerServiceExposing(IServiceCollection services, Type implementationType, List<Type> serviceTypes)
    {
        TriggerServiceExposing(services, implementationType, serviceTypes.ConvertAll(t => new ServiceIdentifier(t)));
    }

    protected virtual void TriggerServiceExposing(IServiceCollection services, Type implementationType, List<ServiceIdentifier> serviceTypes)
    {
        var exposeActions = services.GetExposingActionList();
        if (exposeActions.Any())
        {
            var args = new OnServiceExposingContext(implementationType, serviceTypes);
            foreach (var action in exposeActions)
            {
                action(args);
            }
        }
    }

    protected virtual DependencyAttribute? GetDependencyAttributeOrNull(Type type)
    {
        return type.GetCustomAttribute<DependencyAttribute>(true);
    }

    protected virtual ServiceLifetime? GetLifeTimeOrNull(Type type, DependencyAttribute? dependencyAttribute)
    {
        return dependencyAttribute?.Lifetime ?? GetServiceLifetimeFromClassHierarchy(type) ?? GetDefaultLifeTimeOrNull(type);
    }

    protected virtual ServiceLifetime? GetServiceLifetimeFromClassHierarchy(Type type)
    {
        if (typeof(ITransientDependency).GetTypeInfo().IsAssignableFrom(type))
        {
            return ServiceLifetime.Transient;
        }

        if (typeof(ISingletonDependency).GetTypeInfo().IsAssignableFrom(type))
        {
            return ServiceLifetime.Singleton;
        }

        if (typeof(IScopedDependency).GetTypeInfo().IsAssignableFrom(type))
        {
            return ServiceLifetime.Scoped;
        }

        return null;
    }

    protected virtual ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
    {
        return null;
    }

    protected virtual List<Type> GetExposedServiceTypes(Type type)
    {
        return ExposedServiceExplorer.GetExposedServices(type);
    }

    protected virtual List<ServiceIdentifier> GetExposedKeyedServiceTypes(Type type)
    {
        return ExposedServiceExplorer.GetExposedKeyedServices(type);
    }

    protected virtual ServiceDescriptor CreateServiceDescriptor(
        Type implementationType,
        object? serviceKey,
        Type exposingServiceType,
        List<ServiceIdentifier> allExposingServiceTypes,
        ServiceLifetime lifeTime)
    {
        if (lifeTime.IsIn(ServiceLifetime.Singleton, ServiceLifetime.Scoped))
        {
            var redirectedType = GetRedirectedTypeOrNull(
                implementationType,
                exposingServiceType,
                allExposingServiceTypes
            );

            if (redirectedType != null)
            {
                return serviceKey == null
                    ? ServiceDescriptor.Describe(
                        exposingServiceType,
                        provider => provider.GetService(redirectedType)!,
                        lifeTime
                    )
                    : ServiceDescriptor.DescribeKeyed(
                        exposingServiceType,
                        serviceKey,
                        (provider, key) => provider.GetKeyedService(redirectedType, key)!,
                        lifeTime
                    );
            }
        }

        return serviceKey == null
            ? ServiceDescriptor.Describe(
                exposingServiceType,
                implementationType,
                lifeTime
            )
            : ServiceDescriptor.DescribeKeyed(
                exposingServiceType,
                serviceKey,
                implementationType,
                lifeTime
            );
    }

    protected virtual Type? GetRedirectedTypeOrNull(
        Type implementationType,
        Type exposingServiceType,
        List<ServiceIdentifier> allExposingKeyedServiceTypes)
    {
        if (allExposingKeyedServiceTypes.Count < 2)
        {
            return null;
        }

        if (exposingServiceType == implementationType)
        {
            return null;
        }

        if (allExposingKeyedServiceTypes.Any(t => t.ServiceType == implementationType))
        {
            return implementationType;
        }

        return allExposingKeyedServiceTypes.FirstOrDefault(
            t => t.ServiceType != exposingServiceType && exposingServiceType.IsAssignableFrom(t.ServiceType)
        ).ServiceType;
    }
}
