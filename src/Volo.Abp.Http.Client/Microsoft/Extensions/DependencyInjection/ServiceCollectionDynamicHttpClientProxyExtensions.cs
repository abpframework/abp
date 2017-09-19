using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Volo.Abp.Application.Services;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.DynamicProxying;
using Volo.Abp.Http.Modeling;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionDynamicHttpClientProxyExtensions
    {
        private static readonly ProxyGenerator ProxyGeneratorInstance = new ProxyGenerator();

        public static IServiceCollection AddHttpClientProxies(
            this IServiceCollection services,
            Assembly assembly,
            string baseUrl,
            string moduleName = ModuleApiDescriptionModel.DefaultModuleName)
        {
            //TODO: Add option to change type filter

            var serviceTypes = assembly.GetTypes().Where(t =>
                t.IsInterface && t.IsPublic && typeof(IRemoteService).IsAssignableFrom(t)
            );

            foreach (var serviceType in serviceTypes)
            {
                services.AddHttpClientProxy(serviceType, baseUrl);
            }

            return services;
        }

        public static IServiceCollection AddHttpClientProxy<T>(this IServiceCollection services, string baseUrl)
        {
            return services.AddHttpClientProxy(typeof(T), baseUrl);
        }

        public static IServiceCollection AddHttpClientProxy(this IServiceCollection services, Type type, string baseUrl)
        {
            services.Configure<AbpHttpClientOptions>(options =>
            {
                options.HttpClientProxies[type] = new DynamicHttpClientProxyConfig(baseUrl, type);
            });

            var interceptorType = typeof(DynamicHttpProxyInterceptor<>).MakeGenericType(type);
            services.AddTransient(interceptorType);

            var interceptorAdapterType = typeof(CastleAbpInterceptorAdapter<>).MakeGenericType(interceptorType);
            return services.AddTransient(
                type,
                serviceProvider => ProxyGeneratorInstance
                    .CreateInterfaceProxyWithoutTarget(
                        type,
                        (IInterceptor)serviceProvider.GetRequiredService(interceptorAdapterType)
                    )
            );
        }
    }
}
