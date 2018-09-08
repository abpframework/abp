using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsDomainSharedModule),
        typeof(AbpDddDomainModule))]
    public class DocsDomainModule : AbpModule
    {
        
    }
}
