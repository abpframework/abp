using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.BlobStoring.Database.EntityFrameworkCore
{
    [ConnectionStringName(BlobStoringDatabaseDbProperties.ConnectionStringName)]
    public class BlobStoringDbContext : AbpDbContext<BlobStoringDbContext>, IBlobStoringDbContext
    {
        public DbSet<DatabaseBlobContainer> BlobContainers { get; set; }

        public DbSet<DatabaseBlob> Blobs { get; set; }

        public BlobStoringDbContext(DbContextOptions<BlobStoringDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureBlobStoring();
        }
    }
}