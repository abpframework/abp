using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using JetBrains.Annotations;
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
        public static IServiceCollection AddHttpClientProxies(
            [NotNull] this IServiceCollection services,
            [NotNull] Assembly assembly,
            [NotNull] string remoteServiceConfigurationName = RemoteServiceConfigurationDictionary.DefaultName,
            bool asDefaultServices = true)
        {
            Check.NotNull(services, nameof(assembly));

            var serviceTypes = assembly.GetTypes().Where(IsSuitableForDynamicClientProxying).ToArray();

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
        public static IServiceCollection AddHttpClientProxy<T>(
            [NotNull] this IServiceCollection services,
            [NotNull] string remoteServiceConfigurationName = RemoteServiceConfigurationDictionary.DefaultName,
            bool asDefaultService = true)
        {
            return services.AddHttpClientProxy(
                typeof(T),
                remoteServiceConfigurationName,
                asDefaultService
            );
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

            AddHttpClientFactory(services, remoteServiceConfigurationName);

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

        private static IServiceCollection AddHttpClientFactory(
            [NotNull] this IServiceCollection services,
            [NotNull] string remoteServiceConfigurationName = RemoteServiceConfigurationDictionary.DefaultName)
        {
            var preOptions = services.ExecutePreConfiguredActions<AbpHttpClientBuilderOptions>();

            if (preOptions.ConfiguredProxyClients.Contains(remoteServiceConfigurationName))
            {
                return services;
            }

            var clientBuilder = services.AddHttpClient(remoteServiceConfigurationName, (provider, client) =>
            {
                foreach (var clientBuildAction in preOptions.ProxyClientActions)
                {
                    clientBuildAction(remoteServiceConfigurationName, provider, client);
                }
            });

            foreach (var clientBuildAction in preOptions.ProxyClientBuildActions)
            {
                clientBuildAction(remoteServiceConfigurationName, clientBuilder);
            }

            services.PreConfigure<AbpHttpClientBuilderOptions>(options =>
            {
                options.ConfiguredProxyClients.Add(remoteServiceConfigurationName);
            });

            return services;
        }

        /// <summary>
        /// Checks wether the type is suitable to use with the dynamic proxying.
        /// Currently the type is checked statically against some fixed conditions.
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>True, if the type is suitable for dynamic proxying. Otherwise false.</returns>
        private static bool IsSuitableForDynamicClientProxying(Type type)
        {
            //TODO: Add option to change type filter

            return type.IsInterface
                && type.IsPublic
                && !type.IsGenericType
                && typeof(IRemoteService).IsAssignableFrom(type);
        }
    }
}
