using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Docs.Projects;

namespace Volo.Docs.EntityFrameworkCore
{
    public static class DocsDbContextModelBuilderExtensions
    {
        public static void ConfigureDocs(
            [NotNull] this ModelBuilder builder,
            Action<DocsModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new DocsModelBuilderConfigurationOptions(
                DocsDbProperties.DbTablePrefix,
                DocsDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<Project>(b =>
            {
                b.ToTable(options.TablePrefix + "Projects", options.Schema);

                b.ConfigureConcurrencyStamp();
                b.ConfigureExtraProperties();

                b.Property(x => x.Name).IsRequired().HasMaxLength(ProjectConsts.MaxNameLength);
                b.Property(x => x.ShortName).IsRequired().HasMaxLength(ProjectConsts.MaxShortNameLength);
                b.Property(x => x.DefaultDocumentName).IsRequired().HasMaxLength(ProjectConsts.MaxDefaultDocumentNameLength);
                b.Property(x => x.NavigationDocumentName).IsRequired().HasMaxLength(ProjectConsts.MaxNavigationDocumentNameLength);
                b.Property(x => x.LatestVersionBranchName).HasMaxLength(ProjectConsts.MaxLatestVersionBranchNameLength);
            });
        }
    }
}