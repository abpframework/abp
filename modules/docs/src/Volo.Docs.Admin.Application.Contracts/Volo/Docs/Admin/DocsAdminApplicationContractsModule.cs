using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Volo.Docs.Admin
{
    [DependsOn(
        typeof(DocsDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class DocsAdminApplicationContractsModule : AbpModule
    {
        
    }
}
