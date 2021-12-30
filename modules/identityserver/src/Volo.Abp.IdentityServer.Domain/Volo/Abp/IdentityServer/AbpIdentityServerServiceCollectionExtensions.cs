using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Volo.Abp.IdentityServer;

public static class AbpIdentityServerServiceCollectionExtensions
{
    public static void AddAbpStrictRedirectUriValidator(this IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient<IRedirectUriValidator, AbpStrictRedirectUriValidator>());
    }

    public static void AddAbpClientConfigurationValidator(this IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient<IClientConfigurationValidator, AbpClientConfigurationValidator>());
    }

    public static void AddAbpWildcardSubdomainCorsPolicyService(this IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient<ICorsPolicyService, AbpWildcardSubdomainCorsPolicyService>());
    }
}
