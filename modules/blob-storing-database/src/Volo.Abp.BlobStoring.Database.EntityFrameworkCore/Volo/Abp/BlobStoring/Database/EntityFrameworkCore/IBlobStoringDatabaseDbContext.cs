using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.BlobStoring.Database.EntityFrameworkCore
{
    [ConnectionStringName(BlobStoringDatabaseDbProperties.ConnectionStringName)]
    public interface IBlobStoringDatabaseDbContext : IEfCoreDbContext
    {
        DbSet<Container> Containers { get; }

        DbSet<Blob> Blobs { get; }
    }
}