using Volo.Abp.Modularity;

namespace Volo.Abp.Minify
{
    [DependsOn(
        typeof(AbpMinifyModule),
        typeof(AbpTestBaseModule))]
    public class AbpMinifyTestModule : AbpModule
    {
    }
}