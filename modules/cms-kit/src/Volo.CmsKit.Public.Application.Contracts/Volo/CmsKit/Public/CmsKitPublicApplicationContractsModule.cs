using Volo.Abp.Modularity;

namespace Volo.CmsKit.Public
{
    [DependsOn(
        typeof(CmsKitCommonApplicationContractsModule)
        )]
    public class CmsKitPublicApplicationContractsModule : AbpModule
    {

    }
}
