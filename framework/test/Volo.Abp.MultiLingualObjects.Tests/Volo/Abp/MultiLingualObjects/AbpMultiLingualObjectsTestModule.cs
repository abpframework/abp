using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Settings;

namespace Volo.Abp.MultiLingualObjects
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpLocalizationModule),
        typeof(AbpSettingsModule),
        typeof(AbpObjectMappingModule),
        typeof(AbpMultiLingualObjectsModule),
        typeof(AbpTestBaseModule)
    )]
    public class AbpMultiLingualObjectsTestModule : AbpModule
    {
    }
}
