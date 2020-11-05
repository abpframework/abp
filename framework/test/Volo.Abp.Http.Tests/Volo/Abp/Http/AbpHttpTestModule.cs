using Volo.Abp.Modularity;

namespace Volo.Abp.Http
{
    [DependsOn(typeof(AbpHttpModule))]
    public class AbpHttpTestModule : AbpModule
    {

    }
}
