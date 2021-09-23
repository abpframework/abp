using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.BlobStoring.Database.EntityFrameworkCore
{
    public static class BlobStoringDbContextModelCreatingExtensions
    {
        public static void ConfigureBlobStoring(
            this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<DatabaseBlobContainer>(b =>
            {
                b.ToTable(BlobStoringDatabaseDbProperties.DbTablePrefix + "BlobContainers", BlobStoringDatabaseDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(p => p.Name).IsRequired().HasMaxLength(DatabaseContainerConsts.MaxNameLength);

                b.HasMany<DatabaseBlob>().WithOne().HasForeignKey(p => p.ContainerId);

                b.HasIndex(x => new {x.TenantId, x.Name});

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<DatabaseBlob>(b =>
            {
                b.ToTable(BlobStoringDatabaseDbProperties.DbTablePrefix + "Blobs", BlobStoringDatabaseDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(p => p.ContainerId).IsRequired(); //TODO: Foreign key!
                b.Property(p => p.Name).IsRequired().HasMaxLength(DatabaseBlobConsts.MaxNameLength);
                b.Property(p => p.Content).HasMaxLength(DatabaseBlobConsts.MaxContentLength);

                b.HasOne<DatabaseBlobContainer>().WithMany().HasForeignKey(p => p.ContainerId);

                b.HasIndex(x => new {x.TenantId, x.ContainerId, x.Name});

                b.ApplyObjectExtensionMappings();
            });

            builder.TryConfigureObjectExtensions<BlobStoringDbContext>();
        }
    }
}
