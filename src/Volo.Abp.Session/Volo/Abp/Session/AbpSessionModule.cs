using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Permissions;
using Volo.Abp.Security;
using Volo.Abp.Settings;

namespace Volo.Abp.Session
{
    [DependsOn(typeof(AbpSecurityModule))]
    [DependsOn(typeof(AbpSettingsModule))]
    [DependsOn(typeof(AbpPermissionsModule))]
    public class AbpSessionModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SettingOptions>(options =>
            {
                options.ValueProviders.Add<UserSettingValueProvider>();
            });

            services.Configure<PermissionOptions>(options =>
            {
                options.ValueProviders.Add<UserPermissionValueProvider>();
            });

            services.AddAssemblyOf<AbpSessionModule>();
        }
    }
}
