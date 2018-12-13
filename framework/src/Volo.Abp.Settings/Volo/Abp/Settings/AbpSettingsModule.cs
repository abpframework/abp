using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace Volo.Abp.Settings
{
    [DependsOn(
        typeof(AbpLocalizationAbstractionsModule),
        typeof(AbpSecurityModule)
        )]
    public class AbpSettingsModule : AbpModule
    {

    }
}
