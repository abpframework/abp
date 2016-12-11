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
        //TODO: Return IServiceCollection from all methods

        public static void AddAssemblyOf<T>(this IServiceCollection services)
        {
            services.AddAssembly(typeof(T).GetTypeInfo().Assembly);
        }

        public static void AddAssembly(this IServiceCollection services, Assembly assembly)
        {
            var types = AssemblyHelper.GetAllTypes(assembly).Where(t =>
            {
                var typeInfo = t.GetTypeInfo();
                return typeInfo.IsClass && !typeInfo.IsAbstract && !typeInfo.IsGenericType && !typeInfo.IsDefined(typeof(DisableAutoDependencyInjectionRegistrationAttribute));
            });
            
            services.AddTypes(types.ToArray());
        }

        public static void AddTypes(this IServiceCollection services, params Type[] types)
        {
            foreach (var type in types)
            {
                services.AddType(type);
            }
        }

        public static void AddType(this IServiceCollection services, Type type)
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
        }
        
        private static List<Type> FindServiceTypes(Type type)
        {
            var customExposedServices = type.GetTypeInfo().GetCustomAttributes().OfType<IExposedServiceTypesProvider>().SelectMany(p => p.GetExposedServiceTypes()).ToList();
            if (customExposedServices.Any())
            {
                return customExposedServices;
            }

            var serviceTypes = new List<Type>();

            serviceTypes.Add(type);

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
