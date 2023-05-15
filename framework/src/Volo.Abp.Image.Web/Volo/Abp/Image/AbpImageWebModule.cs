using Volo.Abp.Modularity;

namespace Volo.Abp.Image;

[DependsOn(typeof(AbpImageModule))]
public class AbpImageWebModule : AbpModule
{
}