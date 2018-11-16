using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace Volo.Abp.Users
{
    //TODO: Consider to (somehow) move this to the framework to the same assemblily of ICurrentUser!

    [DependsOn(
        typeof(AbpMultiTenancyAbstractionsModule),
        typeof(AbpEventBusModule)
        )]
    public class AbpUsersAbstractionModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<SettingOptions>(options =>
            {
                options.ValueProviders.Add<UserSettingValueProvider>();
            });
        }
    }
}
