using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace Volo.Abp.SettingManagement;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule),
    typeof(AbpSettingManagementDomainModule))]
public class AbpSettingManagementTestBaseModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        using (var scope = context.ServiceProvider.CreateScope())
        {
            AsyncHelper.RunSync(() => scope.ServiceProvider
                .GetRequiredService<SettingTestDataBuilder>()
                .BuildAsync());
        }
    }
}
