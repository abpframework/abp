using System;
using Castle.DynamicProxy;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.Http;
using Volo.Abp.Http.DynamicProxying;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionDynamicHttpClientProxyExtensions
    {
        private static readonly ProxyGenerator ProxyGeneratorInstance = new ProxyGenerator();

        //TODO: AddHttpClientProxies for adding all services from single assembly!

        public static IServiceCollection AddHttpClientProxy<T>(this IServiceCollection services, string baseUrl)
        {
            return services.AddHttpClientProxy(typeof(T), baseUrl);
        }

        public static IServiceCollection AddHttpClientProxy(this IServiceCollection services, Type type, string baseUrl)
        {
            services.Configure<AbpHttpOptions>(options =>
            {
                options.HttpClientProxies[type] = new DynamicHttpClientProxyConfig(type, baseUrl);
            });

            var interceptorType = typeof(DynamicHttpProxyInterceptor<>).MakeGenericType(type);
            services.AddTransient(interceptorType);

            var interceptorAdapterType = typeof(CastleAbpInterceptorAdapter<>).MakeGenericType(interceptorType);
            return services.AddTransient(
                type,
                serviceProvider => ProxyGeneratorInstance
                    .CreateInterfaceProxyWithoutTarget(
                        type,
                        (IInterceptor) serviceProvider.GetRequiredService(interceptorAdapterType)
                    )
            );
        }
    }
}
