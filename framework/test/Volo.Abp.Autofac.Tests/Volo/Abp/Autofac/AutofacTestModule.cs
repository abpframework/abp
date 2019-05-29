using Volo.Abp.Modularity;

namespace Volo.Abp.Autofac
{
    [DependsOn(typeof(AutofacModule))]
    public class AutofacTestModule : AbpModule
    {

    }
}