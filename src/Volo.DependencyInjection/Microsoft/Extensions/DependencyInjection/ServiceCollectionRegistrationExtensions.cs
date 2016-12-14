using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Volo.DependencyInjection;
using Volo.ExtensionMethods;
using Volo.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionRegistrationExtensions
    {
        //TODO: Check if assembly/type is added before or add TryAdd versions of them?
        //TODO: When to use Add, when to use TryAdd? We may think to add conventional interfaces to indicate and attributes to override them.

        public static IServiceCollection AddAssemblyOf<T>(this IServiceCollection services)
        {
            return services.AddAssembly(typeof(T).GetTypeInfo().Assembly);
        }

        public static IServiceCollection AddAssembly(this IServiceCollection services, Assembly assembly)
        {
            var types = AssemblyHelper.GetAllTypes(assembly).Where(t =>
            {
                var typeInfo = t.GetTypeInfo();
                return typeInfo.IsClass && !typeInfo.IsAbstract && !typeInfo.IsGenericType && !typeInfo.IsDefined(typeof(SkipAutoRegistrationAttribute));
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
            //TODO: Make this code extensible, so we can add other conventions!

            foreach (var serviceType in FindServiceTypes(type))
            {
                if (typeof(ITransientDependency).GetTypeInfo().IsAssignableFrom(type))
                {
                    services.AddTransient(serviceType, type);
                }

                if (typeof(ISingletonDependency).GetTypeInfo().IsAssignableFrom(type))
                {
                    services.AddSingleton(serviceType, type);
                }

                if (typeof(IScopedDependency).GetTypeInfo().IsAssignableFrom(type))
                {
                    services.AddScoped(serviceType, type);
                }
            }

            return services;
        }

        private static IEnumerable<Type> FindServiceTypes(Type type)
        {
            var customExposedServices = type.GetTypeInfo().GetCustomAttributes().OfType<IExposedServiceTypesProvider>().SelectMany(p => p.GetExposedServiceTypes()).ToList();
            if (customExposedServices.Any())
            {
                return customExposedServices;
            }

            var serviceTypes = new List<Type> { type };

            foreach (var interfaceType in type.GetTypeInfo().GetInterfaces())
            {
                var interfaceName = interfaceType.Name;
                if (interfaceName.StartsWith("I"))
                {
                    interfaceName = interfaceName.Right(interfaceName.Length - 1);
                }

                if (type.Name.EndsWith(interfaceName))
                {
                    serviceTypes.Add(interfaceType);
                }
            }

            return serviceTypes;
        }
    }
}
