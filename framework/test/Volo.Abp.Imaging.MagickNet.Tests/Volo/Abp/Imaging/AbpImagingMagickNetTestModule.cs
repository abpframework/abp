using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Imaging;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpImagingMagickNetModule),
    typeof(AbpTestBaseModule)
)]
public class AbpImagingMagickNetTestModule : AbpModule
{

}