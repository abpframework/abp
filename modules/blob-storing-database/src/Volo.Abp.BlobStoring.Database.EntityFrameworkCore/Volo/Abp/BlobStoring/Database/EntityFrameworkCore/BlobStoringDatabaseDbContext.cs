using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.BlobStoring.Database.EntityFrameworkCore
{
    [ConnectionStringName(BlobStoringDatabaseDbProperties.ConnectionStringName)]
    public class BlobStoringDatabaseDbContext : AbpDbContext<BlobStoringDatabaseDbContext>, IBlobStoringDatabaseDbContext
    {
        public DbSet<Container> Containers { get; set; }

        public DbSet<Blob> Blobs { get; set; }

        public BlobStoringDatabaseDbContext(DbContextOptions<BlobStoringDatabaseDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureDatabaseBlobStoring();
        }

    }
}