using Volo.Abp.Castle;
using Volo.Abp.Modularity;

namespace Volo.Abp.Autofac
{
    [DependsOn(typeof(CastleCoreModule))]
    public class AutofacModule : AbpModule
    {

    }
}
