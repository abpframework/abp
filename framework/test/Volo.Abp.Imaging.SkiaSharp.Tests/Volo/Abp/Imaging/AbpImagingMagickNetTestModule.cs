using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Imaging;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpImagingSkiaSharpModule),
    typeof(AbpTestBaseModule)
)]
public class AbpImagingSkiaSharpTestModule : AbpModule
{

}
