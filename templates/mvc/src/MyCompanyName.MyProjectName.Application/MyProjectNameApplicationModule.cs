using Microsoft.Extensions.DependencyInjection;
using MyCompanyName.MyProjectName.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameDomainModule),
        typeof(AbpIdentityApplicationModule))]
    public class MyProjectNameApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<MyProjectNamePermissionDefinitionProvider>();
            });

            context.Services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<MyProjectNameApplicationAutoMapperProfile>();
            });

            context.Services.AddAssemblyOf<MyProjectNameApplicationModule>();
        }
    }
}
