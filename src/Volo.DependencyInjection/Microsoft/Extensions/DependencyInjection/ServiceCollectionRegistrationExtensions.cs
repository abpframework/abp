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
        //TODO: Check if assembly/type is added before or add TryAdd versions of them?
        //TODO: When to use Add, when to use TryAdd? We may think to add conventional interfaces to indicate and attributes to override them.
        //TODO: Make this code extensible, so we can add other conventions!

        public static IServiceCollection AddAssemblyOf<T>(this IServiceCollection services)
        {
            return services.AddAssemblyOf<T>(false);
        }

        public static IServiceCollection TryAddAssemblyOf<T>(this IServiceCollection services)
        {
            return services.AddAssemblyOf<T>(true);
        }

        public static IServiceCollection AddAssembly(this IServiceCollection services, Assembly assembly)
        {
            return services.AddAssembly(assembly, false);
        }

        public static IServiceCollection TryAddAssembly(this IServiceCollection services, Assembly assembly)
        {
            return services.AddAssembly(assembly, true);
        }

        public static IServiceCollection AddTypes(this IServiceCollection services, params Type[] types)
        {
            return services.AddTypes(false, types);
        }

        public static IServiceCollection TryAddTypes(this IServiceCollection services, params Type[] types)
        {
            return services.AddTypes(true, types);
        }

        public static IServiceCollection AddType(this IServiceCollection services, Type type)
        {
            return services.AddType(type, false);
        }

        public static IServiceCollection TryAddType(this IServiceCollection services, Type type)
        {
            return services.AddType(type, true);
        }

        internal static IServiceCollection AddAssemblyOf<T>(this IServiceCollection services, bool tryAdd)
        {
            return services.AddAssembly(typeof(T).GetTypeInfo().Assembly, tryAdd);
        }

        internal static IServiceCollection AddAssembly(this IServiceCollection services, Assembly assembly, bool tryAdd)
        {
            var types = AssemblyHelper
                .GetAllTypes(assembly)
                .Where(t =>
                {
                    var typeInfo = t.GetTypeInfo();
                    return typeInfo.IsClass &&
                           !typeInfo.IsAbstract &&
                           !typeInfo.IsGenericType &&
                           !typeInfo.IsDefined(typeof(SkipAutoRegistrationAttribute));
                });

            return services.AddTypes(tryAdd, types.ToArray());
        }

        internal static IServiceCollection AddTypes(this IServiceCollection services, bool tryAdd, params Type[] types)
        {
            foreach (var type in types)
            {
                services.AddType(type, tryAdd);
            }

            return services;
        }

        internal static IServiceCollection AddType(this IServiceCollection services, Type type, bool tryAdd)
        {
            var lifeTime = GetServiceLifeTime(type);
            if (lifeTime == null)
            {
                return services;
            }
            
            foreach (var serviceType in AutoRegistrationHelper.GetExposedServices(type))
            {
                var serviceDescriptor = ServiceDescriptor.Describe(serviceType, type, lifeTime.Value);
                if (tryAdd)
                {
                    services.Add(serviceDescriptor);
                }
                else
                {
                    services.TryAdd(serviceDescriptor);
                }
            }

            return services;
        }

        [CanBeNull]
        internal static ServiceLifetime? GetServiceLifeTime(Type type)
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
