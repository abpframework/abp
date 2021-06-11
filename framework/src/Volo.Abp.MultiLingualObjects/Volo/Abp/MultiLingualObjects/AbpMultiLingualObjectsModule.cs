using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.MultiLingualObjects
{
    [DependsOn(
        typeof(AbpLocalizationModule))]
    public class AbpMultiLingualObjectsModule : AbpModule
    {
    }
}
