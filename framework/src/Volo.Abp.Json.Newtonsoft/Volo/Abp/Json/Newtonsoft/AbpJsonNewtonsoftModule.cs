using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.Newtonsoft;

[DependsOn(typeof(AbpJsonCoreModule), typeof(AbpTimingModule))]
public class AbpJsonNewtonsoftModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpJsonOptions>(options =>
        {
            options.Providers.Add<AbpNewtonsoftJsonSerializerProvider>();
        });
    }
}
