using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Database
{
    [DependsOn(
        typeof(DatabaseDomainSharedModule)
        )]
    public class DatabaseDomainModule : AbpModule
    {

    }
}
