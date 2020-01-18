using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using JetBrains.Annotations;
using Polly;
using Volo.Abp;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.DynamicProxying;
using Volo.Abp.Validation;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionDynamicHttpClientProxyExtensions
    {
        private static readonly ProxyGenerator ProxyGeneratorInstance = new ProxyGenerator();

        /// <summary>
        /// Registers HTTP Client Proxies for all public interfaces
        /// extend the <see cref="IRemoteService"/> interface in the
        /// given <paramref name="assembly"/>.
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="assembly">The assembly containing the service interfaces</param>
        /// <param name="remoteServiceConfigurationName">
        /// The name of the remote service configuration to be used by the HTTP Client proxies.
        /// See <see cref="AbpRemoteServiceOptions"/>.
        /// </param>
        /// <param name="asDefaultServices">
        /// True, to register the HTTP client proxy as the default implementation for the services.
        /// </param>
        /// <param name="configureHttpClientBuilder">
        /// A delegate that is used to configure an <see cref="T:Microsoft.Extensions.DependencyInjection.IHttpClientBuilder" />.
        /// </param>
        public static IServiceCollection AddHttpClientProxies(
            [NotNull] this IServiceCollection services,
            [NotNull] Assembly assembly,
            [NotNull] string remoteServiceConfigurationName = RemoteServiceConfigurationDictionary.DefaultName,
            bool asDefaultServices = true,
            Action<IHttpClientBuilder> configureHttpClientBuilder = null)
        {
            Check.NotNull(services, nameof(assembly));

            AddHttpClientFactoryAndPolicy(services, remoteServiceConfigurationName, configureHttpClientBuilder);

            //TODO: Make a configuration option and add remoteServiceName inside it!
            //TODO: Add option to change type filter

            var serviceTypes = assembly.GetTypes().Where(t =>
                t.IsInterface && t.IsPublic && typeof(IRemoteService).IsAssignableFrom(t)
            );

            foreach (var serviceType in serviceTypes)
            {
                services.AddHttpClientProxy(
                    serviceType,
                    remoteServiceConfigurationName,
                    asDefaultServices
                );
            }

            return services;
        }

        /// <summary>
        /// Registers HTTP Client Proxy for given service type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the service</typeparam>
        /// <param name="services">Service collection</param>
        /// <param name="remoteServiceConfigurationName">
        /// The name of the remote service configuration to be used by the HTTP Client proxy.
        /// See <see cref="AbpRemoteServiceOptions"/>.
        /// </param>
        /// <param name="asDefaultService">
        /// True, to register the HTTP client proxy as the default implementation for the service <typeparamref name="T"/>.
        /// </param>
        /// <param name="configureHttpClientBuilder">
        /// A delegate that is used to configure an <see cref="T:Microsoft.Extensions.DependencyInjection.IHttpClientBuilder" />.
        /// </param>
        public static IServiceCollection AddHttpClientProxy<T>(
            [NotNull] this IServiceCollection services,
            [NotNull] string remoteServiceConfigurationName = RemoteServiceConfigurationDictionary.DefaultName,
            bool asDefaultService = true,
            Action<IHttpClientBuilder> configureHttpClientBuilder = null)
        {
            AddHttpClientFactoryAndPolicy(services, remoteServiceConfigurationName, configureHttpClientBuilder);

            return services.AddHttpClientProxy(
                typeof(T),
                remoteServiceConfigurationName,
                asDefaultService
            );
        }

        /// <summary>
        /// Use IHttpClientFactory and polly
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="remoteServiceConfigurationName">
        /// The name of the remote service configuration to be used by the HTTP Client proxies.
        /// See <see cref="AbpRemoteServiceOptions"/>.
        /// </param>
        /// <param name="configureHttpClientBuilder">
        /// A delegate that is used to configure an <see cref="T:Microsoft.Extensions.DependencyInjection.IHttpClientBuilder" />.
        /// </param>
        public static IServiceCollection AddHttpClientFactoryAndPolicy(
            [NotNull] this IServiceCollection services,
            [NotNull] string remoteServiceConfigurationName = RemoteServiceConfigurationDictionary.DefaultName,
            Action<IHttpClientBuilder> configureHttpClientBuilder = null)
        {
            var httpClientBuilder = services.AddHttpClient(remoteServiceConfigurationName);
            if (configureHttpClientBuilder == null)
            {
                httpClientBuilder.AddTransientHttpErrorPolicy(builder =>
                    // retry 3 times
                    builder.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i))));
            }
            else
            {
                configureHttpClientBuilder.Invoke(httpClientBuilder);
            }

            return services;
        }

        /// <summary>
        /// Registers HTTP Client Proxy for given service <paramref name="type"/>.
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="type">Type of the service</param>
        /// <param name="remoteServiceConfigurationName">
        /// The name of the remote service configuration to be used by the HTTP Client proxy.
        /// See <see cref="AbpRemoteServiceOptions"/>.
        /// </param>
        /// <param name="asDefaultService">
        /// True, to register the HTTP client proxy as the default implementation for the service <paramref name="type"/>.
        /// </param>
        public static IServiceCollection AddHttpClientProxy(
            [NotNull] this IServiceCollection services,
            [NotNull] Type type,
            [NotNull] string remoteServiceConfigurationName = RemoteServiceConfigurationDictionary.DefaultName,
            bool asDefaultService = true)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(type, nameof(type));
            Check.NotNullOrWhiteSpace(remoteServiceConfigurationName, nameof(remoteServiceConfigurationName));

            services.Configure<AbpHttpClientOptions>(options =>
            {
                options.HttpClientProxies[type] = new DynamicHttpClientProxyConfig(type, remoteServiceConfigurationName);
            });

            var interceptorType = typeof(DynamicHttpProxyInterceptor<>).MakeGenericType(type);
            services.AddTransient(interceptorType);

            var interceptorAdapterType = typeof(AbpAsyncDeterminationInterceptor<>).MakeGenericType(interceptorType);
            
            var validationInterceptorAdapterType =
                typeof(AbpAsyncDeterminationInterceptor<>).MakeGenericType(typeof(ValidationInterceptor));

            if (asDefaultService)
            {
                services.AddTransient(
                    type,
                    serviceProvider => ProxyGeneratorInstance
                        .CreateInterfaceProxyWithoutTarget(
                            type,
                            (IInterceptor)serviceProvider.GetRequiredService(validationInterceptorAdapterType),
                            (IInterceptor)serviceProvider.GetRequiredService(interceptorAdapterType)
                        )
                );
            }

            services.AddTransient(
                typeof(IHttpClientProxy<>).MakeGenericType(type),
                serviceProvider =>
                {
                    var service = ProxyGeneratorInstance
                        .CreateInterfaceProxyWithoutTarget(
                            type,
                            (IInterceptor)serviceProvider.GetRequiredService(validationInterceptorAdapterType),
                            (IInterceptor)serviceProvider.GetRequiredService(interceptorAdapterType)
                        );

                    return Activator.CreateInstance(
                        typeof(HttpClientProxy<>).MakeGenericType(type),
                        service
                    );
                });

            return services;
        }
    }
}
