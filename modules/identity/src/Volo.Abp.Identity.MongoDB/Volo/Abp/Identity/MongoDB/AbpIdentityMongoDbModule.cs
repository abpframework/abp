using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Users.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    [DependsOn(
        typeof(AbpIdentityDomainModule),
        typeof(AbpUsersMongoDbModule)
        )]
    public class AbpIdentityMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            AbpIdentityBsonClassMap.Configure();

            context.Services.AddMongoDbContext<AbpIdentityMongoDbContext>(options =>
            {
                options.AddRepository<IdentityUser, MongoIdentityUserRepository>();
                options.AddRepository<IdentityRole, MongoIdentityRoleRepository>();
            });
        }
    }
}
