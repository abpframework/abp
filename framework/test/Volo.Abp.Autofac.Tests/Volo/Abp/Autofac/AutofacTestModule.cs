using Volo.Abp.Modularity;

namespace Volo.Abp.Autofac
{
    [DependsOn(typeof(AbpAutofacModule))]
    public class AutofacTestModule : AbpModule
    {

    }
}