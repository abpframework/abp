﻿using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Docs.Documents;
using Volo.Docs.Projects;

namespace Volo.Docs.EntityFrameworkCore
{
    public static class DocsDbContextModelBuilderExtensions
    {
        public static void ConfigureDocs(
            [NotNull] this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            if (builder.IsTenantOnlyDatabase())
            {
                return;
            }

            builder.Entity<Project>(b =>
            {
                b.ToTable(AbpDocsDbProperties.DbTablePrefix + "Projects", AbpDocsDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasMaxLength(ProjectConsts.MaxNameLength);
                b.Property(x => x.ShortName).IsRequired().HasMaxLength(ProjectConsts.MaxShortNameLength);
                b.Property(x => x.DefaultDocumentName).IsRequired().HasMaxLength(ProjectConsts.MaxDefaultDocumentNameLength);
                b.Property(x => x.NavigationDocumentName).IsRequired().HasMaxLength(ProjectConsts.MaxNavigationDocumentNameLength);
                b.Property(x => x.ParametersDocumentName).IsRequired().HasMaxLength(ProjectConsts.MaxParametersDocumentNameLength);
                b.Property(x => x.LatestVersionBranchName).HasMaxLength(ProjectConsts.MaxLatestVersionBranchNameLength);

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<Document>(b =>
            {
                b.ToTable(AbpDocsDbProperties.DbTablePrefix + "Documents", AbpDocsDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasMaxLength(DocumentConsts.MaxNameLength);
                b.Property(x => x.Version).IsRequired().HasMaxLength(DocumentConsts.MaxVersionNameLength);
                b.Property(x => x.LanguageCode).IsRequired().HasMaxLength(DocumentConsts.MaxLanguageCodeNameLength);
                b.Property(x => x.FileName).IsRequired().HasMaxLength(DocumentConsts.MaxFileNameNameLength);
                b.Property(x => x.Content).IsRequired();
                b.Property(x => x.Format).HasMaxLength(DocumentConsts.MaxFormatNameLength);
                b.Property(x => x.EditLink).HasMaxLength(DocumentConsts.MaxEditLinkLength);
                b.Property(x => x.RootUrl).HasMaxLength(DocumentConsts.MaxRootUrlLength);
                b.Property(x => x.RawRootUrl).HasMaxLength(DocumentConsts.MaxRawRootUrlLength);
                b.Property(x => x.LocalDirectory).HasMaxLength(DocumentConsts.MaxLocalDirectoryLength);

                b.HasMany(x => x.Contributors).WithOne()
                    .HasForeignKey(x => new { x.DocumentId })
                    .IsRequired();

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<DocumentContributor>(b =>
            {
                b.ToTable(AbpDocsDbProperties.DbTablePrefix + "DocumentContributors", AbpDocsDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.HasKey(x => new { x.DocumentId, x.Username });

                b.ApplyObjectExtensionMappings();
            });

            builder.TryConfigureObjectExtensions<DocsDbContext>();
        }
    }
}
