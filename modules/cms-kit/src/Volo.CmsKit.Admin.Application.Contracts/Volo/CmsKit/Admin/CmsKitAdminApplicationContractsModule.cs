using Volo.Abp.Modularity;

namespace Volo.CmsKit.Admin;

[DependsOn(
    typeof(CmsKitCommonApplicationContractsModule)
    )]
public class CmsKitAdminApplicationContractsModule : AbpModule
{

}
