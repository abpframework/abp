using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.Imaging;

[DependsOn(typeof(AbpThreadingModule))]
public class AbpImagingAbstractionsModule : AbpModule
{
}