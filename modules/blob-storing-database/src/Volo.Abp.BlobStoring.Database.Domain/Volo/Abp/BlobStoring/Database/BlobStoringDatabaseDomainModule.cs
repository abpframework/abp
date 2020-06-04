using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Database
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpBlobStoringModule),
        typeof(BlobStoringDatabaseDomainSharedModule)
        )]
    public class BlobStoringDatabaseDomainModule : AbpModule
    {

    }
}
