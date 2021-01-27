using Autofac.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Settings;

namespace Volo.Abp.MultiLingualObject
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpLocalizationModule),
        typeof(AbpSettingsModule),
        typeof(AbpObjectMappingModule),
        typeof(AbpMultiLingualObjectModule),
        typeof(AbpTestBaseModule)
    )]
    public class AbpMultiLingualObjectTestModule : AbpModule
    {
    }
}
