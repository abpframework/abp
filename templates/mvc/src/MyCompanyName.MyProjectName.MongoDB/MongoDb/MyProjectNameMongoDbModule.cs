using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Identity.MongoDB;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.MongoDB;
using Volo.Abp.SettingManagement.MongoDB;

namespace MyCompanyName.MyProjectName.MongoDb
{
    [DependsOn(
        typeof(AbpPermissionManagementMongoDbModule),
        typeof(AbpSettingManagementMongoDbModule),
        typeof(AbpIdentityMongoDbModule)
        )]
    public class MyProjectNameMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<MyProjectNameMongoDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });

            context.Services.AddAssemblyOf<MyProjectNameMongoDbModule>();
        }
    }
}
