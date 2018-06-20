using Microsoft.Extensions.DependencyInjection;
using MyCompanyName.MyProjectName.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameDomainModule),
        typeof(AbpIdentityApplicationModule))]
    public class MyProjectNameApplicationModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<MyProjectNamePermissionDefinitionProvider>();
            });

            services.AddAssemblyOf<MyProjectNameApplicationModule>();
        }
    }
}
