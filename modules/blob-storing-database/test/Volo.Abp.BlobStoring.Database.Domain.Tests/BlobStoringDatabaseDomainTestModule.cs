using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Database;

[DependsOn(
    typeof(BlobStoringDatabaseEntityFrameworkCoreTestModule)
    )]
public class BlobStoringDatabaseDomainTestModule : AbpModule
{

}
