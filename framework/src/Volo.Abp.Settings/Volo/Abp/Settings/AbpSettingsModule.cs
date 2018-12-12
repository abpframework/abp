using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.Settings
{
    [DependsOn(
        typeof(AbpLocalizationAbstractionsModule)
        )]
    public class AbpSettingsModule : AbpModule
    {

    }
}
