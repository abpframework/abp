using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.BlobStoring.Database.EntityFrameworkCore
{
    public static class BlobStoringDatabaseDbContextModelCreatingExtensions
    {
        public static void ConfigureDatabaseBlobStoring(
            this ModelBuilder builder,
            Action<BlobStoringDatabaseModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BlobStoringDatabaseModelBuilderConfigurationOptions(
                BlobStoringDatabaseDbProperties.DbTablePrefix,
                BlobStoringDatabaseDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<Container>(b =>
            {
                b.ToTable(options.TablePrefix + "Containers", options.Schema);

                b.ConfigureByConvention();

                b.Property(p => p.Name).IsRequired().HasMaxLength(ContainerConsts.MaxNameLength);

                b.HasIndex(x => x.Name);
            });

            builder.Entity<Blob>(b =>
            {
                b.ToTable(options.TablePrefix + "Blobs", options.Schema);

                b.ConfigureByConvention();

                b.Property(p => p.Name).IsRequired().HasMaxLength(ContainerConsts.MaxNameLength);
                b.Property(p => p.ContainerId).IsRequired();

                b.HasIndex(x => x.Name);
            });
        }
    }
}