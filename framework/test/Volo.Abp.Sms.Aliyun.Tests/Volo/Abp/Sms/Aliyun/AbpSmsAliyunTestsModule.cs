using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Sms.Aliyun;

[DependsOn(typeof(AbpSmsAliyunModule))]
public class AbpSmsAliyunTestsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpAliyunSmsOptions>(
            configuration.GetSection("AbpAliyunSms")
        );
    }
}
