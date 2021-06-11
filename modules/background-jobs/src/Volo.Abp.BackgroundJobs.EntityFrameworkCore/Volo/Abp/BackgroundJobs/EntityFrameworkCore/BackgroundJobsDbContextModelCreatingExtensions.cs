using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.BackgroundJobs.EntityFrameworkCore
{
    public static class BackgroundJobsDbContextModelCreatingExtensions
    {
        public static void ConfigureBackgroundJobs(
            this ModelBuilder builder,
            Action<BackgroundJobsModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            if (builder.IsTenantOnlyDatabase())
            {
                return;
            }

            var options = new BackgroundJobsModelBuilderConfigurationOptions(
                BackgroundJobsDbProperties.DbTablePrefix,
                BackgroundJobsDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<BackgroundJobRecord>(b =>
            {
                b.ToTable(options.TablePrefix + "BackgroundJobs", options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.JobName).IsRequired().HasMaxLength(BackgroundJobRecordConsts.MaxJobNameLength);
                b.Property(x => x.JobArgs).IsRequired().HasMaxLength(BackgroundJobRecordConsts.MaxJobArgsLength);
                b.Property(x => x.TryCount).HasDefaultValue(0);
                b.Property(x => x.NextTryTime);
                b.Property(x => x.LastTryTime);
                b.Property(x => x.IsAbandoned).HasDefaultValue(false);
                b.Property(x => x.Priority).HasDefaultValue(BackgroundJobPriority.Normal);

                b.HasIndex(x => new { x.IsAbandoned, x.NextTryTime });

                b.ApplyObjectExtensionMappings();
            });

            builder.TryConfigureObjectExtensions<BackgroundJobsDbContext>();
        }
    }
}
