using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp;
using Volo.Abp.AspNetCore.Components.WebAssembly.WebApp;
using Volo.Abp.Http.Client.Authentication;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpBlazorWebAppServiceCollectionExtensions
{
    public static IServiceCollection AddBlazorWebAppServices([NotNull] this IServiceCollection services)
    {
        Check.NotNull(services, nameof(services));

        services.AddSingleton<AuthenticationStateProvider, RemoteAuthenticationStateProvider>();
        services.Replace(ServiceDescriptor.Transient<IAbpAccessTokenProvider, CookieBasedWebAssemblyAbpAccessTokenProvider>());

        return services;
    }

    [Obsolete("Use AddBlazorWebAppServices instead. See https://github.com/abpframework/abp/issues/22622")]
    public static IServiceCollection AddBlazorWebAppTieredServices([NotNull] this IServiceCollection services)
    {
        Check.NotNull(services, nameof(services));

        // Compatibility with old template code
        services.AddTransient<AddBlazorWebAppTieredServicesHasBeenCalled>();

        services.AddScoped<AuthenticationStateProvider, RemoteAuthenticationStateProvider>();
        services.Replace(ServiceDescriptor.Singleton<IAbpAccessTokenProvider, PersistentComponentStateAbpAccessTokenProvider>());

        return services;
    }
}
