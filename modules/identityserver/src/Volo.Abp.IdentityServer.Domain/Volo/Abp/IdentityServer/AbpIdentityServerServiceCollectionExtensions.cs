using IdentityServer4.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Volo.Abp.IdentityServer
{
    public static class AbpIdentityServerServiceCollectionExtensions
    {
        public static void AddAbpStrictRedirectUriValidator(this IServiceCollection services, params string[] domainFormats)
        {
            services.Configure<AbpRedirectUriValidatorOptions>(options =>
            {
                options.DomainFormats.AddRange(domainFormats);
            });

            services.Replace(ServiceDescriptor.Transient<IRedirectUriValidator, AbpStrictRedirectUriValidator>());
        }
    }
}
