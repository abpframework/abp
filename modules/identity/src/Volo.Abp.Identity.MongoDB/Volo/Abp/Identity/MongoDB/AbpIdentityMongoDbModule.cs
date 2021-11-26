using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Users.MongoDB;

namespace Volo.Abp.Identity.MongoDB;

[DependsOn(
    typeof(AbpIdentityDomainModule),
    typeof(AbpUsersMongoDbModule)
    )]
public class AbpIdentityMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<AbpIdentityMongoDbContext>(options =>
        {
            options.AddRepository<IdentityUser, MongoIdentityUserRepository>();
            options.AddRepository<IdentityRole, MongoIdentityRoleRepository>();
            options.AddRepository<IdentityClaimType, MongoIdentityClaimTypeRepository>();
            options.AddRepository<OrganizationUnit, MongoOrganizationUnitRepository>();
            options.AddRepository<IdentitySecurityLog, MongoIdentitySecurityLogRepository>();
            options.AddRepository<IdentityLinkUser, MongoIdentityLinkUserRepository>();
        });
    }
}
