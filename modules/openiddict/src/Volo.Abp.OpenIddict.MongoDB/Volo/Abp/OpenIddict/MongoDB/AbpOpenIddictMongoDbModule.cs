using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict.MongoDB
{
    [DependsOn(
        typeof(AbpOpenIddictDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class AbpOpenIddictMongoDbModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<OpenIddictCoreBuilder>(builder => builder.AddAbpStore());
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<OpenIddictMongoDbContext>(options =>
            {
                options.AddRepository<OpenIddictApplication, MongoDbOpenIddictApplicationRepository>();
                options.AddRepository<OpenIddictAuthorization, MongoDbOpenIddictAuthorizationRepository>();
                options.AddRepository<OpenIddictScope, MongoDbOpenIddictScopeRepository>();
                options.AddRepository<OpenIddictToken, MongoDbOpenIddictTokenRepository>();
            });
        }
    }
}
