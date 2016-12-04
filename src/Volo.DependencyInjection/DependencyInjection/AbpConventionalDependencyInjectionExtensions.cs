using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Internal;

namespace Volo.DependencyInjection
{
    public static class AbpConventionalDependencyInjectionExtensions
    {
        //TODO: Check if assembly/type is added before or add TryAdd versions of them?

        public static void AddAssemblyOf<T>(this IServiceCollection services)
        {
            services.AddAssembly(typeof(T).GetTypeInfo().Assembly);
        }

        public static void AddAssembly(this IServiceCollection services, Assembly assembly)
        {
            services.AddTypes(AssemblyHelper.GetAllTypes(assembly).FilterInjectableTypes().ToArray());
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
            //TODO: Find exposed services for the type

            if (typeof(ITransientDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                services.AddTransient(type);
            }

            if (typeof(ISingletonDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                services.AddSingleton(type);
            }

            if (typeof(IScopedDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                services.AddScoped(type);
            }
        }

        private static IEnumerable<Type> FilterInjectableTypes(this IEnumerable<Type> types)
        {
            return types.Where(t =>
            {
                var typeInfo = t.GetTypeInfo();
                return typeInfo.IsClass && !typeInfo.IsAbstract && !typeInfo.IsGenericType;
            });
        }
    }
}
