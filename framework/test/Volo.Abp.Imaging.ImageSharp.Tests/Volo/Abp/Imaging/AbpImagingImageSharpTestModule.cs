using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Imaging;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpImagingImageSharpModule),
    typeof(AbpTestBaseModule)
)]

public class AbpImagingImageSharpTestModule : AbpModule
{

}