using Volo.Abp.Emailing;
using Volo.Abp.Modularity;

namespace Volo.Abp.MailKit
{
    [DependsOn(typeof(AbpEmailingModule))]
    public class AbpMailKitModule : AbpModule
    {

    }
}
