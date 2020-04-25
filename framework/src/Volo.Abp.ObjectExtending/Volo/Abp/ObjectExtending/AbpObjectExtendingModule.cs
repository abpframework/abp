using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.ObjectExtending
{
    [DependsOn(
        typeof(AbpLocalizationAbstractionsModule)
        )]
    public class AbpObjectExtendingModule : AbpModule
    {

    }
}
