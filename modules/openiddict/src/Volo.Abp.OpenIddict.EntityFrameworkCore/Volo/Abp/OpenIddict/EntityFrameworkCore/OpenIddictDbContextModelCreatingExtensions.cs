using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict.EntityFrameworkCore;

public static class OpenIddictDbContextModelCreatingExtensions
{
    public static void ConfigureOpenIddict(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        if (builder.IsTenantOnlyDatabase())
        {
            return;
        }

        builder.Entity<OpenIddictApplication>(b =>
        {
            b.ToTable(AbpOpenIddictDbProperties.DbTablePrefix + "Applications", AbpOpenIddictDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasIndex(x => x.ClientId)
                .IsUnique();

            b.Property(x => x.ClientId)
                .HasMaxLength(OpenIddictApplicationConsts.ClientIdMaxLength);

            b.Property(x => x.ConsentType)
                .HasMaxLength(OpenIddictApplicationConsts.ConsentTypeMaxLength);

            b.Property(x => x.Type)
                .HasMaxLength(OpenIddictApplicationConsts.TypeMaxLength);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<OpenIddictAuthorization>(b =>
        {
            b.ToTable(AbpOpenIddictDbProperties.DbTablePrefix + "Authorizations", AbpOpenIddictDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasIndex(x => new
            {
                x.ApplicationId,
                x.Status,
                x.Subject,
                x.Type
            });

            b.Property(x => x.Status)
                .HasMaxLength(OpenIddictAuthorizationConsts.StatusMaxLength);

            b.Property(x => x.Subject)
                .HasMaxLength(OpenIddictAuthorizationConsts.SubjectMaxLength);

            b.Property(x => x.Type)
                .HasMaxLength(OpenIddictAuthorizationConsts.TypeMaxLength);

            b.HasOne<OpenIddictApplication>().WithMany().HasForeignKey(x => x.ApplicationId).IsRequired(false);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<OpenIddictScope>(b =>
        {
            b.ToTable(AbpOpenIddictDbProperties.DbTablePrefix + "Scopes", AbpOpenIddictDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasIndex(x => x.Name)
                .IsUnique();

            b.Property(x => x.Name)
                .HasMaxLength(OpenIddictScopeConsts.NameMaxLength);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<OpenIddictToken>(b =>
        {
            b.ToTable(AbpOpenIddictDbProperties.DbTablePrefix + "Tokens", AbpOpenIddictDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasIndex(x => x.ReferenceId)
                .IsUnique();

            b.HasIndex(x => new
            {
                x.ApplicationId,
                x.Status,
                x.Subject,
                x.Type
            });

            b.Property(x => x.ReferenceId)
                .HasMaxLength(OpenIddictTokenConsts.ReferenceIdMaxLength);

            b.Property(x => x.Status)
                .HasMaxLength(OpenIddictTokenConsts.StatusMaxLength);

            b.Property(x => x.Subject)
                .HasMaxLength(OpenIddictTokenConsts.SubjectMaxLength);

            b.Property(x => x.Type)
                .HasMaxLength(OpenIddictTokenConsts.TypeMaxLength);

            b.HasOne<OpenIddictApplication>().WithMany().HasForeignKey(x => x.ApplicationId).IsRequired(false);
            b.HasOne<OpenIddictAuthorization>().WithMany().HasForeignKey(x => x.AuthorizationId).IsRequired(false);

            b.ApplyObjectExtensionMappings();
        });

    }
}
