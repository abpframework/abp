using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    [DependsOn(
        typeof(MyProjectNameDomainModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule))]
    public class MyProjectNameEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAbpDbContext<MyProjectNameDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });

            services.AddAssemblyOf<MyProjectNameEntityFrameworkCoreModule>();
        }
    }
}
