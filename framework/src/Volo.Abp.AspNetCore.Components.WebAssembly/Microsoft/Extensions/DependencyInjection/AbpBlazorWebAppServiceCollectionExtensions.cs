using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp;
using Volo.Abp.AspNetCore.Components.WebAssembly.WebApp;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpBlazorWebAppServiceCollectionExtensions
{
    public static IServiceCollection AddBlazorWebAppServices([NotNull] this IServiceCollection services)
    {
        Check.NotNull(services, nameof(services));

        services.AddSingleton<AuthenticationStateProvider, RemoteAuthenticationStateProvider>();
        services.Replace(ServiceDescriptor.Transient<IAbpAccessTokenProvider, CookieBasedWebAssemblyAbpAccessTokenProvider>());
        services.Replace(ServiceDescriptor.Transient<ICurrentPrincipalAccessor, RemoteCurrentPrincipalAccessor>());

        return services;
    }

    public static IServiceCollection AddBlazorWebAppTieredServices([NotNull] this IServiceCollection services)
    {
        Check.NotNull(services, nameof(services));

        services.AddScoped<AuthenticationStateProvider, RemoteAuthenticationStateProvider>();
        services.Replace(ServiceDescriptor.Singleton<IAbpAccessTokenProvider, PersistentComponentStateAbpAccessTokenProvider>());
        services.Replace(ServiceDescriptor.Transient<ICurrentPrincipalAccessor, RemoteCurrentPrincipalAccessor>());

        return services;
    }
}
