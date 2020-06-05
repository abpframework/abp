using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.MailKit
{
    [DependsOn(
        typeof(AbpMailKitModule),
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule))]
    public class AbpMailKitTestModule :AbpModule
    {
    }
}
