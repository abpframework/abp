using Volo.Abp.Modularity;

namespace Volo.Abp.Sms.Aliyun
{
    [DependsOn(typeof(AbpSmsAliyunModule))]
    public class AbpSmsAliyunTestsModule : AbpModule
    {

    }
}