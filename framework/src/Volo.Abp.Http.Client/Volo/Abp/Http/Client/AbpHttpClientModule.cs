using Volo.Abp.Castle;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http.Client
{
    [DependsOn(typeof(AbpHttpModule))]
    [DependsOn(typeof(AbpCastleCoreModule))]
    public class AbpHttpClientModule : AbpModule
    {

    }
}
