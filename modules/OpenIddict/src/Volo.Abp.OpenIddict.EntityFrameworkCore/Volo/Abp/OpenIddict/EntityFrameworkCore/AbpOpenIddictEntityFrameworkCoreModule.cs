using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict.EntityFrameworkCore;

[DependsOn(
    typeof(AbpOpenIddictDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class AbpOpenIddictEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<OpenIddictDbContext>(options =>
        {
            options.AddDefaultRepositories<IOpenIddictDbContext>();

            options.AddRepository<OpenIddictApplication, OpenIddictApplicationRepository>();
            options.AddRepository<OpenIddictAuthorization, OpenIddictAuthorizationRepository>();
            options.AddRepository<OpenIddictScope, OpenIddictScopeRepository>();
            options.AddRepository<OpenIddictToken, OpenIddictTokenRepository>();
        });
    }
}
