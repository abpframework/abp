using System;
using Castle.DynamicProxy;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.DynamicProxying;
using Volo.Abp.Http.Modeling;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionDynamicHttpClientProxyExtensions
    {
        private static readonly ProxyGenerator ProxyGeneratorInstance = new ProxyGenerator();

        //TODO: AddHttpClientProxies for adding all services from single assembly!

        public static IServiceCollection AddHttpClientProxy<T>(this IServiceCollection services, string baseUrl, string moduleName = ModuleApiDescriptionModel.DefaultServiceModuleName)
        {
            return services.AddHttpClientProxy(typeof(T), baseUrl, moduleName);
        }

        public static IServiceCollection AddHttpClientProxy(this IServiceCollection services, Type type, string baseUrl, string moduleName = ModuleApiDescriptionModel.DefaultServiceModuleName)
        {
            services.Configure<AbpHttpClientOptions>(options =>
            {
                options.HttpClientProxies[type] = new DynamicHttpClientProxyConfig(moduleName, baseUrl, type);
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
