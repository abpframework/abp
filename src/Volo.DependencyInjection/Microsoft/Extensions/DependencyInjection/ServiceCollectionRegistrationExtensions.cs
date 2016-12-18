using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.DependencyInjection;
using Volo.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionRegistrationExtensions
    {
        //TODO: Make this code extensible, so we can add other conventions! Also, extract default convention to a class which implements the convention interface.

        public static IServiceCollection AddAssemblyOf<T>(this IServiceCollection services)
        {
            return services.AddAssembly(typeof(T).GetTypeInfo().Assembly);
        }

        public static IServiceCollection AddAssembly(this IServiceCollection services, Assembly assembly)
        {
            var types = AssemblyHelper
                .GetAllTypes(assembly)
                .Where(t =>
                {
                    var typeInfo = t.GetTypeInfo();
                    return typeInfo.IsClass &&
                           !typeInfo.IsAbstract &&
                           !typeInfo.IsGenericType;
                });

            return services.AddTypes(types.ToArray());
        }

        public static IServiceCollection AddTypes(this IServiceCollection services, params Type[] types)
        {
            foreach (var type in types)
            {
                services.AddType(type);
            }

            return services;
        }

        public static IServiceCollection AddType(this IServiceCollection services, Type type)
        {
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsDefined(typeof(DisableConventionalRegistrationAttribute), true))
            {
                return services;
            }

            var dependencyAttribute = typeInfo.GetCustomAttributes<DependencyAttribute>(true).FirstOrDefault(); //TODO: Use GetCustomAttribute instead?

            var lifeTime = dependencyAttribute?.Lifetime ?? GetServiceLifetimeFromInterfaces(type);
            if (lifeTime == null)
            {
                return services;
            }

            foreach (var serviceType in AutoRegistrationHelper.GetExposedServices(type))
            {
                var serviceDescriptor = ServiceDescriptor.Describe(serviceType, type, lifeTime.Value);

                if (dependencyAttribute?.TryRegister == true)
                {
                    services.TryAdd(serviceDescriptor);
                }
                else
                {
                    services.Add(serviceDescriptor);
                }
            }

            return services;
        }

        [CanBeNull]
        internal static ServiceLifetime? GetServiceLifetimeFromInterfaces(Type type)
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
    }
}
