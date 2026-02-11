using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Imaging;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpImagingAspNetCoreModule),
    typeof(AbpTestBaseModule)
)]
public class AbpImagingAspNetCoreTestModule : AbpModule
{

}