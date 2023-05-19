using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.AutoMapper;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpObjectExtendingTestModule)
)]
public class AutoMapperTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AutoMapperTestModule>();
        });
    }
}
