using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.BlobStoring.Database.EntityFrameworkCore;

[ConnectionStringName(AbpBlobStoringDatabaseDbProperties.ConnectionStringName)]
public interface IBlobStoringDbContext : IEfCoreDbContext
{
    DbSet<DatabaseBlobContainer> BlobContainers { get; }

    DbSet<DatabaseBlob> Blobs { get; }
}
