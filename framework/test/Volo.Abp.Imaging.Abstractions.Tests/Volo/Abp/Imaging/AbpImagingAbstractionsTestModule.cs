using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Imaging;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpImagingAbstractionsModule),
    typeof(AbpTestBaseModule)
)]
public class AbpImagingAbstractionsTestModule : AbpModule
{

}