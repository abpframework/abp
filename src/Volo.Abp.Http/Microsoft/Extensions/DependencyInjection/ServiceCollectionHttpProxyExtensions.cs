using System;
using Castle.DynamicProxy;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.Http;
using Volo.Abp.Http.DynamicProxying;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionHttpProxyExtensions
    {
        private static readonly ProxyGenerator ProxyGeneratorInstance = new ProxyGenerator();

        public static IServiceCollection AddHttpClientProxy<T>(this IServiceCollection services, string baseUrl)
        {
            return services.AddHttpClientProxy(typeof(T), baseUrl);
        }

        public static IServiceCollection AddHttpClientProxy(this IServiceCollection services, Type type, string baseUrl)
        {
            services.AddTransient(type, serviceProvider =>
            {
                return ProxyGeneratorInstance
                    .CreateInterfaceProxyWithoutTarget(
                        type,
                        serviceProvider.GetRequiredService<CastleAbpInterceptorAdapter<DynamicHttpProxyInterceptor>>()
                    );
            });
            
            return services;
        }
    }
}
