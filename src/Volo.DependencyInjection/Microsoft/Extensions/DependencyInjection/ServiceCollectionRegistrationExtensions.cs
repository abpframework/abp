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
            return services.InternalAddAssemblyOf<T>(false);
        }

        public static IServiceCollection TryAddAssemblyOf<T>(this IServiceCollection services)
        {
            return services.InternalAddAssemblyOf<T>(true);
        }

        internal static IServiceCollection InternalAddAssemblyOf<T>(this IServiceCollection services, bool tryAdd)
        {
            return services.InternalAddAssembly(typeof(T).GetTypeInfo().Assembly, tryAdd);
        }

        public static IServiceCollection AddAssembly(this IServiceCollection services, Assembly assembly)
        {
            return services.InternalAddAssembly(assembly, false);
        }

        public static IServiceCollection TryAddAssembly(this IServiceCollection services, Assembly assembly)
        {
            return services.InternalAddAssembly(assembly, true);
        }

        internal static IServiceCollection InternalAddAssembly(this IServiceCollection services, Assembly assembly, bool tryAdd)
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

            return services.InternalAddTypes(tryAdd, types.ToArray());
        }

        public static IServiceCollection AddTypes(this IServiceCollection services, params Type[] types)
        {
            return services.InternalAddTypes(false, types);
        }

        public static IServiceCollection TryAddTypes(this IServiceCollection services, params Type[] types)
        {
            return services.InternalAddTypes(true, types);
        }

        internal static IServiceCollection InternalAddTypes(this IServiceCollection services, bool tryAdd, params Type[] types)
        {
            foreach (var type in types)
            {
                services.AddType(type, tryAdd);
            }

            return services;
        }

        public static IServiceCollection AddType(this IServiceCollection services, Type type)
        {
            return services.AddType(type, false);
        }

        public static IServiceCollection TryAddType(this IServiceCollection services, Type type)
        {
            return services.AddType(type, true);
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
