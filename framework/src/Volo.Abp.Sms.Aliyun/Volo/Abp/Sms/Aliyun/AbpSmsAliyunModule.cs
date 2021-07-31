using EasyAbp.Abp.Aliyun.Common.Configurations;
using EasyAbp.Abp.Aliyun.Sms;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Sms.Aliyun
{
    [DependsOn(typeof(AbpSmsModule),
        typeof(AbpAliyunSmsModule))]
    public class AbpSmsAliyunModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpAliyunSmsOptions>(configuration.GetSection("AbpAliyunSms"));
            Configure<AbpAliyunOptions>(configuration.GetSection("AbpAliyunSms"));
        }
    }
}