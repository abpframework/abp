using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Volo.Abp.Application.Services;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.DynamicProxying;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionDynamicHttpClientProxyExtensions
    {
        private static readonly ProxyGenerator ProxyGeneratorInstance = new ProxyGenerator();

        public static IServiceCollection AddHttpClientProxies(this IServiceCollection services, Assembly assembly, string remoteServiceName = RemoteServiceConfigurationDictionary.DefaultName)
        {
            //TODO: Add option to change type filter

            var serviceTypes = assembly.GetTypes().Where(t =>
                t.IsInterface && t.IsPublic && typeof(IRemoteService).IsAssignableFrom(t)
            );

            foreach (var serviceType in serviceTypes)
            {
                services.AddHttpClientProxy(serviceType, remoteServiceName);
            }

            return services;
        }

        public static IServiceCollection AddHttpClientProxy<T>(this IServiceCollection services, string remoteServiceName = RemoteServiceConfigurationDictionary.DefaultName)
        {
            return services.AddHttpClientProxy(typeof(T), remoteServiceName);
        }

        public static IServiceCollection AddHttpClientProxy(this IServiceCollection services, Type type, string remoteServiceName = RemoteServiceConfigurationDictionary.DefaultName)
        {
            services.Configure<AbpHttpClientOptions>(options =>
            {
                options.HttpClientProxies[type] = new DynamicHttpClientProxyConfig(type, remoteServiceName);
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
