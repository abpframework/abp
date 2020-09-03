using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.MultiLingualObject
{
    [DependsOn(
        typeof(AbpLocalizationModule))]
    public class AbpMultiLingualObjectModule : AbpModule
    {
    }
}
