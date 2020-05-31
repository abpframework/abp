using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.BlobStoring.Database.EntityFrameworkCore
{
    public static class BlobStoringDbContextModelCreatingExtensions
    {
        public static void ConfigureBlobStoring(
            this ModelBuilder builder,
            Action<BlobStoringModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BlobStoringModelBuilderConfigurationOptions(
                BlobStoringDatabaseDbProperties.DbTablePrefix,
                BlobStoringDatabaseDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<DatabaseBlobContainer>(b =>
            {
                b.ToTable(options.TablePrefix + "BlobContainers", options.Schema);

                b.ConfigureByConvention();

                b.Property(p => p.Name).IsRequired().HasMaxLength(DatabaseContainerConsts.MaxNameLength);

                b.HasIndex(x => new {x.TenantId, x.Name});
            });

            builder.Entity<DatabaseBlob>(b =>
            {
                b.ToTable(options.TablePrefix + "Blobs", options.Schema);

                b.ConfigureByConvention();

                b.Property(p => p.ContainerId).IsRequired(); //TODO: Foreign key!
                b.Property(p => p.Name).IsRequired().HasMaxLength(DatabaseBlobConsts.MaxNameLength);
                b.Property(p => p.Content).HasMaxLength(DatabaseBlobConsts.MaxContentLength);

                b.HasIndex(x => new {x.TenantId, x.ContainerId, x.Name});
            });
        }
    }
}