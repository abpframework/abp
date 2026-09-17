using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Sms.TencentCloud;

[DependsOn(typeof(AbpSmsModule))]
public class AbpSmsTencentCloudModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpTencentCloudSmsOptions>(configuration.GetSection("AbpTencentCloudSms"));
    }
}
