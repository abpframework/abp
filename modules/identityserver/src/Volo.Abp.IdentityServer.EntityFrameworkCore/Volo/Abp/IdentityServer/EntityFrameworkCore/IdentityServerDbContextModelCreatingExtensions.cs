using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.EntityFrameworkCore.ValueComparers;
using Volo.Abp.EntityFrameworkCore.ValueConverters;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Devices;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;

namespace Volo.Abp.IdentityServer.EntityFrameworkCore
{
    public static class IdentityServerDbContextModelCreatingExtensions
    {
        public static void ConfigureIdentityServer(
            this ModelBuilder builder,
            Action<IdentityServerModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new IdentityServerModelBuilderConfigurationOptions(
                AbpIdentityServerDbProperties.DbTablePrefix,
                AbpIdentityServerDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<Client>(b =>
            {
                b.ToTable(options.TablePrefix + "Clients", options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.ClientId).HasMaxLength(ClientConsts.ClientIdMaxLength).IsRequired();
                b.Property(x => x.ProtocolType).HasMaxLength(ClientConsts.ProtocolTypeMaxLength).IsRequired();
                b.Property(x => x.ClientName).HasMaxLength(ClientConsts.ClientNameMaxLength);
                b.Property(x => x.ClientUri).HasMaxLength(ClientConsts.ClientUriMaxLength);
                b.Property(x => x.LogoUri).HasMaxLength(ClientConsts.LogoUriMaxLength);
                b.Property(x => x.Description).HasMaxLength(ClientConsts.DescriptionMaxLength);
                b.Property(x => x.FrontChannelLogoutUri).HasMaxLength(ClientConsts.FrontChannelLogoutUriMaxLength);
                b.Property(x => x.BackChannelLogoutUri).HasMaxLength(ClientConsts.BackChannelLogoutUriMaxLength);
                b.Property(x => x.ClientClaimsPrefix).HasMaxLength(ClientConsts.ClientClaimsPrefixMaxLength);
                b.Property(x => x.PairWiseSubjectSalt).HasMaxLength(ClientConsts.PairWiseSubjectSaltMaxLength);
                b.Property(x => x.UserCodeType).HasMaxLength(ClientConsts.UserCodeTypeMaxLength);

                b.HasMany(x => x.AllowedScopes).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                b.HasMany(x => x.ClientSecrets).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                b.HasMany(x => x.AllowedGrantTypes).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                b.HasMany(x => x.AllowedCorsOrigins).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                b.HasMany(x => x.RedirectUris).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                b.HasMany(x => x.PostLogoutRedirectUris).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                b.HasMany(x => x.IdentityProviderRestrictions).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                b.HasMany(x => x.Claims).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                b.HasMany(x => x.Properties).WithOne().HasForeignKey(x => x.ClientId).IsRequired();

                b.HasIndex(x => x.ClientId);
            });

            builder.Entity<ClientGrantType>(b =>
            {
                b.ToTable(options.TablePrefix + "ClientGrantTypes", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(x => new { x.ClientId, x.GrantType });

                b.Property(x => x.GrantType).HasMaxLength(ClientGrantTypeConsts.GrantTypeMaxLength).IsRequired();
            });

            builder.Entity<ClientRedirectUri>(b =>
            {
                b.ToTable(options.TablePrefix + "ClientRedirectUris", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(x => new { x.ClientId, x.RedirectUri });

                if (options.DatabaseProvider == EfCoreDatabaseProvider.MySql)
                {
                    b.Property(x => x.RedirectUri).HasMaxLength(300).IsRequired();
                }
                else
                {
                    b.Property(x => x.RedirectUri).HasMaxLength(ClientRedirectUriConsts.RedirectUriMaxLength).IsRequired();
                }
            });

            builder.Entity<ClientPostLogoutRedirectUri>(b =>
            {
                b.ToTable(options.TablePrefix + "ClientPostLogoutRedirectUris", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(x => new { x.ClientId, x.PostLogoutRedirectUri });

                if (options.DatabaseProvider == EfCoreDatabaseProvider.MySql)
                {
                    b.Property(x => x.PostLogoutRedirectUri).HasMaxLength(300).IsRequired();
                }
                else
                {
                    b.Property(x => x.PostLogoutRedirectUri).HasMaxLength(ClientPostLogoutRedirectUriConsts.PostLogoutRedirectUriMaxLength).IsRequired();
                }
            });

            builder.Entity<ClientScope>(b =>
            {
                b.ToTable(options.TablePrefix + "ClientScopes", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(x => new { x.ClientId, x.Scope });

                b.Property(x => x.Scope).HasMaxLength(ClientScopeConsts.ScopeMaxLength).IsRequired();
            });

            builder.Entity<ClientSecret>(b =>
            {
                b.ToTable(options.TablePrefix + "ClientSecrets", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(x => new { x.ClientId, x.Type, x.Value });

                b.Property(x => x.Type).HasMaxLength(SecretConsts.TypeMaxLength).IsRequired();

                if (options.DatabaseProvider == EfCoreDatabaseProvider.MySql)
                {
                    b.Property(x => x.Value).HasMaxLength(300).IsRequired();
                }
                else
                {
                    b.Property(x => x.Value).HasMaxLength(SecretConsts.ValueMaxLength).IsRequired();
                }

                b.Property(x => x.Description).HasMaxLength(SecretConsts.DescriptionMaxLength);
            });

            builder.Entity<ClientClaim>(b =>
            {
                b.ToTable(options.TablePrefix + "ClientClaims", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(x => new { x.ClientId, x.Type, x.Value });

                b.Property(x => x.Type).HasMaxLength(ClientClaimConsts.TypeMaxLength).IsRequired();
                b.Property(x => x.Value).HasMaxLength(ClientClaimConsts.ValueMaxLength).IsRequired();
            });

            builder.Entity<ClientIdPRestriction>(b =>
            {
                b.ToTable(options.TablePrefix + "ClientIdPRestrictions", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(x => new { x.ClientId, x.Provider });

                b.Property(x => x.Provider).HasMaxLength(ClientIdPRestrictionConsts.ProviderMaxLength).IsRequired();
            });

            builder.Entity<ClientCorsOrigin>(b =>
            {
                b.ToTable(options.TablePrefix + "ClientCorsOrigins", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(x => new { x.ClientId, x.Origin });

                b.Property(x => x.Origin).HasMaxLength(ClientCorsOriginConsts.OriginMaxLength).IsRequired();
            });

            builder.Entity<ClientProperty>(b =>
            {
                b.ToTable(options.TablePrefix + "ClientProperties", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(x => new { x.ClientId, x.Key });

                b.Property(x => x.Key).HasMaxLength(ClientPropertyConsts.KeyMaxLength).IsRequired();
                b.Property(x => x.Value).HasMaxLength(ClientPropertyConsts.ValueMaxLength).IsRequired();
            });

            builder.Entity<PersistedGrant>(b =>
            {
                b.ToTable(options.TablePrefix + "PersistedGrants", options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.Key).HasMaxLength(PersistedGrantConsts.KeyMaxLength).ValueGeneratedNever();
                b.Property(x => x.Type).HasMaxLength(PersistedGrantConsts.TypeMaxLength).IsRequired();
                b.Property(x => x.SubjectId).HasMaxLength(PersistedGrantConsts.SubjectIdMaxLength);
                b.Property(x => x.ClientId).HasMaxLength(PersistedGrantConsts.ClientIdMaxLength).IsRequired();
                b.Property(x => x.CreationTime).IsRequired();

                if (options.DatabaseProvider == EfCoreDatabaseProvider.MySql)
                {
                    b.Property(x => x.Data).HasMaxLength(10000).IsRequired();
                }
                else
                {
                    b.Property(x => x.Data).HasMaxLength(PersistedGrantConsts.DataMaxLength).IsRequired();
                }

                b.HasKey(x => x.Key); //TODO: What about Id!!!

                b.HasIndex(x => new { x.SubjectId, x.ClientId, x.Type });
                b.HasIndex(x => x.Expiration);
            });

            builder.Entity<IdentityResource>(b =>
            {
                b.ToTable(options.TablePrefix + "IdentityResources", options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).HasMaxLength(IdentityResourceConsts.NameMaxLength).IsRequired();
                b.Property(x => x.DisplayName).HasMaxLength(IdentityResourceConsts.DisplayNameMaxLength);
                b.Property(x => x.Description).HasMaxLength(IdentityResourceConsts.DescriptionMaxLength);
                b.Property(x => x.Properties)
                    .HasConversion(new AbpJsonValueConverter<Dictionary<string, string>>())
                    .Metadata.SetValueComparer(new AbpDictionaryValueComparer<string, string>());

                b.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.IdentityResourceId).IsRequired();
            });

            builder.Entity<IdentityClaim>(b =>
            {
                b.ToTable(options.TablePrefix + "IdentityClaims", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(x => new { x.IdentityResourceId, x.Type });

                b.Property(x => x.Type).HasMaxLength(UserClaimConsts.TypeMaxLength).IsRequired();
            });

            builder.Entity<ApiResource>(b =>
            {
                b.ToTable(options.TablePrefix + "ApiResources", options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).HasMaxLength(ApiResourceConsts.NameMaxLength).IsRequired();
                b.Property(x => x.DisplayName).HasMaxLength(ApiResourceConsts.DisplayNameMaxLength);
                b.Property(x => x.Description).HasMaxLength(ApiResourceConsts.DescriptionMaxLength);
                b.Property(x => x.Properties)
                    .HasConversion(new AbpJsonValueConverter<Dictionary<string, string>>())
                    .Metadata.SetValueComparer(new AbpDictionaryValueComparer<string, string>());

                b.HasMany(x => x.Secrets).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
                b.HasMany(x => x.Scopes).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
                b.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
            });

            builder.Entity<ApiSecret>(b =>
            {
                b.ToTable(options.TablePrefix + "ApiSecrets", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(x => new { x.ApiResourceId, x.Type, x.Value });

                b.Property(x => x.Type).HasMaxLength(SecretConsts.TypeMaxLength).IsRequired();
                b.Property(x => x.Description).HasMaxLength(SecretConsts.DescriptionMaxLength);

                if (options.DatabaseProvider == EfCoreDatabaseProvider.MySql)
                {
                    b.Property(x => x.Value).HasMaxLength(300).IsRequired();
                }
                else
                {
                    b.Property(x => x.Value).HasMaxLength(SecretConsts.ValueMaxLength).IsRequired();
                }
            });

            builder.Entity<ApiResourceClaim>(b =>
            {
                b.ToTable(options.TablePrefix + "ApiClaims", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(x => new { x.ApiResourceId, x.Type });

                b.Property(x => x.Type).HasMaxLength(UserClaimConsts.TypeMaxLength).IsRequired();
            });

            builder.Entity<ApiScope>(b =>
            {
                b.ToTable(options.TablePrefix + "ApiScopes", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(x => new { x.ApiResourceId, x.Name });

                b.Property(x => x.Name).HasMaxLength(ApiScopeConsts.NameMaxLength).IsRequired();
                b.Property(x => x.DisplayName).HasMaxLength(ApiScopeConsts.DisplayNameMaxLength);
                b.Property(x => x.Description).HasMaxLength(ApiScopeConsts.DescriptionMaxLength);

                b.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => new { x.ApiResourceId, x.Name }).IsRequired();
            });

            builder.Entity<ApiScopeClaim>(b =>
            {
                b.ToTable(options.TablePrefix + "ApiScopeClaims", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(x => new { x.ApiResourceId, x.Name, x.Type });

                b.Property(x => x.Type).HasMaxLength(UserClaimConsts.TypeMaxLength).IsRequired();
                b.Property(x => x.Name).HasMaxLength(ApiScopeConsts.NameMaxLength).IsRequired();
            });

            builder.Entity<DeviceFlowCodes>(b =>
            {
                b.ToTable(options.TablePrefix + "DeviceFlowCodes", options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.DeviceCode).HasMaxLength(200).IsRequired();
                b.Property(x => x.UserCode).HasMaxLength(200).IsRequired();
                b.Property(x => x.SubjectId).HasMaxLength(200);
                b.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
                b.Property(x => x.Expiration).IsRequired();
                b.Property(x => x.Data).HasMaxLength(50000).IsRequired();

                b.HasIndex(x => new { x.UserCode }).IsUnique();
                b.HasIndex(x => x.DeviceCode).IsUnique();
                b.HasIndex(x => x.Expiration);
            });
        }
    }
}
