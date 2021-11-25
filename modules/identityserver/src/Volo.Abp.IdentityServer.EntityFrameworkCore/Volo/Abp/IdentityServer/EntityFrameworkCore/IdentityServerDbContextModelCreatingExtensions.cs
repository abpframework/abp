using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Devices;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;

namespace Volo.Abp.IdentityServer.EntityFrameworkCore;

public static class IdentityServerDbContextModelCreatingExtensions
{
    public static void ConfigureIdentityServer(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        if (builder.IsTenantOnlyDatabase())
        {
            return;
        }

        #region Client

        builder.Entity<Client>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "Clients", AbpIdentityServerDbProperties.DbSchema);

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
            b.Property(x => x.AllowedIdentityTokenSigningAlgorithms).HasMaxLength(ClientConsts.AllowedIdentityTokenSigningAlgorithms);

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

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<ClientGrantType>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ClientGrantTypes", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.ClientId, x.GrantType });

            b.Property(x => x.GrantType).HasMaxLength(ClientGrantTypeConsts.GrantTypeMaxLength).IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<ClientRedirectUri>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ClientRedirectUris", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.ClientId, x.RedirectUri });

            if (IsDatabaseProvider(builder, EfCoreDatabaseProvider.MySql))
            {
                ClientRedirectUriConsts.RedirectUriMaxLengthValue = 300;
            }

            b.Property(x => x.RedirectUri).HasMaxLength(ClientRedirectUriConsts.RedirectUriMaxLengthValue).IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<ClientPostLogoutRedirectUri>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ClientPostLogoutRedirectUris", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.ClientId, x.PostLogoutRedirectUri });

            if (IsDatabaseProvider(builder, EfCoreDatabaseProvider.MySql))
            {
                ClientPostLogoutRedirectUriConsts.PostLogoutRedirectUriMaxLengthValue = 300;
            }

            b.Property(x => x.PostLogoutRedirectUri)
                .HasMaxLength(ClientPostLogoutRedirectUriConsts.PostLogoutRedirectUriMaxLengthValue)
                .IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<ClientScope>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ClientScopes", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.ClientId, x.Scope });

            b.Property(x => x.Scope).HasMaxLength(ClientScopeConsts.ScopeMaxLength).IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<ClientSecret>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ClientSecrets", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.ClientId, x.Type, x.Value });

            b.Property(x => x.Type).HasMaxLength(ClientSecretConsts.TypeMaxLength).IsRequired();
            if (IsDatabaseProvider(builder, EfCoreDatabaseProvider.MySql, EfCoreDatabaseProvider.Oracle))
            {
                ClientSecretConsts.ValueMaxLength = 300;
            }
            b.Property(x => x.Value).HasMaxLength(ClientSecretConsts.ValueMaxLength).IsRequired();
            b.Property(x => x.Description).HasMaxLength(ClientSecretConsts.DescriptionMaxLength);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<ClientClaim>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ClientClaims", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.ClientId, x.Type, x.Value });

            b.Property(x => x.Type).HasMaxLength(ClientClaimConsts.TypeMaxLength).IsRequired();
            b.Property(x => x.Value).HasMaxLength(ClientClaimConsts.ValueMaxLength).IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<ClientIdPRestriction>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ClientIdPRestrictions", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.ClientId, x.Provider });

            b.Property(x => x.Provider).HasMaxLength(ClientIdPRestrictionConsts.ProviderMaxLength).IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<ClientCorsOrigin>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ClientCorsOrigins", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.ClientId, x.Origin });

            b.Property(x => x.Origin).HasMaxLength(ClientCorsOriginConsts.OriginMaxLength).IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<ClientProperty>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ClientProperties", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.ClientId, x.Key, x.Value });

            b.Property(x => x.Key).HasMaxLength(ClientPropertyConsts.KeyMaxLength).IsRequired();
            if (IsDatabaseProvider(builder, EfCoreDatabaseProvider.MySql))
            {
                ClientPropertyConsts.ValueMaxLength = 300;
            }
            b.Property(x => x.Value).HasMaxLength(ClientPropertyConsts.ValueMaxLength).IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        #endregion

        #region IdentityResource

        builder.Entity<IdentityResource>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "IdentityResources", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Name).HasMaxLength(IdentityResourceConsts.NameMaxLength).IsRequired();
            b.Property(x => x.DisplayName).HasMaxLength(IdentityResourceConsts.DisplayNameMaxLength);
            b.Property(x => x.Description).HasMaxLength(IdentityResourceConsts.DescriptionMaxLength);

            b.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.IdentityResourceId).IsRequired();
            b.HasMany(x => x.Properties).WithOne().HasForeignKey(x => x.IdentityResourceId).IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<IdentityResourceClaim>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "IdentityResourceClaims", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.IdentityResourceId, x.Type });

            b.Property(x => x.Type).HasMaxLength(UserClaimConsts.TypeMaxLength).IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<IdentityResourceProperty>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "IdentityResourceProperties", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.IdentityResourceId, x.Key, x.Value });

            b.Property(x => x.Key).HasMaxLength(IdentityResourcePropertyConsts.KeyMaxLength).IsRequired();
            if (IsDatabaseProvider(builder, EfCoreDatabaseProvider.MySql, EfCoreDatabaseProvider.Oracle))
            {
                IdentityResourcePropertyConsts.ValueMaxLength = 300;
            }
            b.Property(x => x.Value).HasMaxLength(IdentityResourcePropertyConsts.ValueMaxLength).IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        #endregion

        #region ApiResource

        builder.Entity<ApiResource>(b =>
       {
           b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ApiResources", AbpIdentityServerDbProperties.DbSchema);

           b.ConfigureByConvention();

           b.Property(x => x.Name).HasMaxLength(ApiResourceConsts.NameMaxLength).IsRequired();
           b.Property(x => x.DisplayName).HasMaxLength(ApiResourceConsts.DisplayNameMaxLength);
           b.Property(x => x.Description).HasMaxLength(ApiResourceConsts.DescriptionMaxLength);
           b.Property(x => x.AllowedAccessTokenSigningAlgorithms).HasMaxLength(ApiResourceConsts.AllowedAccessTokenSigningAlgorithmsMaxLength);

           b.HasMany(x => x.Secrets).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
           b.HasMany(x => x.Scopes).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
           b.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
           b.HasMany(x => x.Properties).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();

           b.ApplyObjectExtensionMappings();
       });

        builder.Entity<ApiResourceSecret>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ApiResourceSecrets", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.ApiResourceId, x.Type, x.Value });

            b.Property(x => x.Type).HasMaxLength(ApiResourceSecretConsts.TypeMaxLength).IsRequired();

            if (IsDatabaseProvider(builder, EfCoreDatabaseProvider.MySql, EfCoreDatabaseProvider.Oracle))
            {
                ApiResourceSecretConsts.ValueMaxLength = 300;
            }
            b.Property(x => x.Value).HasMaxLength(ApiResourceSecretConsts.ValueMaxLength).IsRequired();

            b.Property(x => x.Description).HasMaxLength(ApiResourceSecretConsts.DescriptionMaxLength);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<ApiResourceClaim>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ApiResourceClaims", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.ApiResourceId, x.Type });

            b.Property(x => x.Type).HasMaxLength(UserClaimConsts.TypeMaxLength).IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<ApiResourceScope>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ApiResourceScopes", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.ApiResourceId, x.Scope });

            b.Property(x => x.Scope).HasMaxLength(ApiResourceScopeConsts.ScopeMaxLength).IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<ApiResourceProperty>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ApiResourceProperties", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.ApiResourceId, x.Key, x.Value });

            b.Property(x => x.Key).HasMaxLength(ApiResourcePropertyConsts.KeyMaxLength).IsRequired();
            if (IsDatabaseProvider(builder, EfCoreDatabaseProvider.MySql, EfCoreDatabaseProvider.Oracle))
            {
                ApiResourcePropertyConsts.ValueMaxLength = 300;
            }
            b.Property(x => x.Value).HasMaxLength(ApiResourcePropertyConsts.ValueMaxLength).IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        #endregion

        #region ApiScope

        builder.Entity<ApiScope>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ApiScopes", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Name).HasMaxLength(ApiScopeConsts.NameMaxLength).IsRequired();
            b.Property(x => x.DisplayName).HasMaxLength(ApiScopeConsts.DisplayNameMaxLength);
            b.Property(x => x.Description).HasMaxLength(ApiScopeConsts.DescriptionMaxLength);

            b.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.ApiScopeId).IsRequired();
            b.HasMany(x => x.Properties).WithOne().HasForeignKey(x => x.ApiScopeId).IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<ApiScopeClaim>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ApiScopeClaims", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.ApiScopeId, x.Type });

            b.Property(x => x.Type).HasMaxLength(UserClaimConsts.TypeMaxLength).IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<ApiScopeProperty>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "ApiScopeProperties", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.ApiScopeId, x.Key, x.Value });

            b.Property(x => x.Key).HasMaxLength(ApiScopePropertyConsts.KeyMaxLength).IsRequired();
            if (IsDatabaseProvider(builder, EfCoreDatabaseProvider.MySql, EfCoreDatabaseProvider.Oracle))
            {
                ApiScopePropertyConsts.ValueMaxLength = 300;
            }
            b.Property(x => x.Value).HasMaxLength(ApiScopePropertyConsts.ValueMaxLength).IsRequired();

            b.ApplyObjectExtensionMappings();
        });

        #endregion

        #region PersistedGrant

        builder.Entity<PersistedGrant>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "PersistedGrants", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Key).HasMaxLength(PersistedGrantConsts.KeyMaxLength).ValueGeneratedNever();
            b.Property(x => x.Type).HasMaxLength(PersistedGrantConsts.TypeMaxLength).IsRequired();
            b.Property(x => x.SubjectId).HasMaxLength(PersistedGrantConsts.SubjectIdMaxLength);
            b.Property(x => x.SessionId).HasMaxLength(PersistedGrantConsts.SessionIdMaxLength);
            b.Property(x => x.ClientId).HasMaxLength(PersistedGrantConsts.ClientIdMaxLength).IsRequired();
            b.Property(x => x.Description).HasMaxLength(PersistedGrantConsts.DescriptionMaxLength);
            b.Property(x => x.CreationTime).IsRequired();

            if (IsDatabaseProvider(builder, EfCoreDatabaseProvider.MySql))
            {
                PersistedGrantConsts.DataMaxLengthValue = 10000; //TODO: MySQL accepts 20.000. We can consider to change in v3.0.
                }

            b.Property(x => x.Data).HasMaxLength(PersistedGrantConsts.DataMaxLengthValue).IsRequired();

            b.HasKey(x => x.Key); //TODO: What about Id!!!

                b.HasIndex(x => new { x.SubjectId, x.ClientId, x.Type });
            b.HasIndex(x => new { x.SubjectId, x.SessionId, x.Type });
            b.HasIndex(x => x.Expiration);

            b.ApplyObjectExtensionMappings();
        });

        #endregion

        #region DeviceFlowCodes

        builder.Entity<DeviceFlowCodes>(b =>
        {
            b.ToTable(AbpIdentityServerDbProperties.DbTablePrefix + "DeviceFlowCodes", AbpIdentityServerDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.DeviceCode).HasMaxLength(DeviceFlowCodesConsts.DeviceCodeMaxLength).IsRequired();
            b.Property(x => x.UserCode).HasMaxLength(DeviceFlowCodesConsts.UserCodeMaxLength).IsRequired();
            b.Property(x => x.SubjectId).HasMaxLength(DeviceFlowCodesConsts.SubjectIdMaxLength);
            b.Property(x => x.SessionId).HasMaxLength(DeviceFlowCodesConsts.SessionIdMaxLength);
            b.Property(x => x.ClientId).HasMaxLength(DeviceFlowCodesConsts.ClientIdMaxLength).IsRequired();
            b.Property(x => x.Description).HasMaxLength(DeviceFlowCodesConsts.DescriptionMaxLength);
            b.Property(x => x.CreationTime).IsRequired();
            b.Property(x => x.Expiration).IsRequired();

            if (IsDatabaseProvider(builder, EfCoreDatabaseProvider.MySql))
            {
                DeviceFlowCodesConsts.DataMaxLength = 10000; //TODO: MySQL accepts 20.000. We can consider to change in v3.0.
                }
            b.Property(x => x.Data).HasMaxLength(DeviceFlowCodesConsts.DataMaxLength).IsRequired();

            b.HasIndex(x => new { x.UserCode });
            b.HasIndex(x => x.DeviceCode).IsUnique();
            b.HasIndex(x => x.Expiration);

            b.ApplyObjectExtensionMappings();
        });

        #endregion

        builder.TryConfigureObjectExtensions<IdentityServerDbContext>();
    }

    private static bool IsDatabaseProvider(
        ModelBuilder modelBuilder,
        params EfCoreDatabaseProvider[] providers)
    {
        foreach (var provider in providers)
        {
            if (modelBuilder.GetDatabaseProvider() == provider)
            {
                return true;
            }
        }

        return false;
    }
}
