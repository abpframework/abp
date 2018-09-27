using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;

namespace Volo.Abp.IdentityServer.EntityFrameworkCore
{
    public static class IdentityServerDbContextModelCreatingExtensions
    {
        public static void ConfigureIdentityServer(
            this ModelBuilder builder, 
            [CanBeNull] string tablePrefix = AbpIdentityServerConsts.DefaultDbTablePrefix, 
            [CanBeNull] string schema = AbpIdentityServerConsts.DefaultDbSchema)
        {
            Check.NotNull(builder, nameof(builder));

            if (tablePrefix == null)
            {
                tablePrefix = "";
            }

            builder.Entity<Client>(client =>
            {
                client.ToTable(tablePrefix + "Clients", schema);

                client.Property(x => x.ClientId).HasMaxLength(ClientConsts.ClientIdMaxLength).IsRequired();
                client.Property(x => x.ProtocolType).HasMaxLength(ClientConsts.ProtocolTypeMaxLength).IsRequired();
                client.Property(x => x.ClientName).HasMaxLength(ClientConsts.ClientNameMaxLength);
                client.Property(x => x.ClientUri).HasMaxLength(ClientConsts.ClientUriMaxLength);
                client.Property(x => x.LogoUri).HasMaxLength(ClientConsts.LogoUriMaxLength);
                client.Property(x => x.Description).HasMaxLength(ClientConsts.DescriptionMaxLength);
                client.Property(x => x.FrontChannelLogoutUri).HasMaxLength(ClientConsts.FrontChannelLogoutUriMaxLength);
                client.Property(x => x.BackChannelLogoutUri).HasMaxLength(ClientConsts.BackChannelLogoutUriMaxLength);
                client.Property(x => x.ClientClaimsPrefix).HasMaxLength(ClientConsts.ClientClaimsPrefixMaxLength);
                client.Property(x => x.PairWiseSubjectSalt).HasMaxLength(ClientConsts.PairWiseSubjectSaltMaxLength);

                client.HasMany(x => x.AllowedScopes).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                client.HasMany(x => x.ClientSecrets).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                client.HasMany(x => x.AllowedGrantTypes).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                client.HasMany(x => x.AllowedCorsOrigins).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                client.HasMany(x => x.RedirectUris).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                client.HasMany(x => x.PostLogoutRedirectUris).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                client.HasMany(x => x.IdentityProviderRestrictions).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                client.HasMany(x => x.Claims).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                client.HasMany(x => x.Properties).WithOne().HasForeignKey(x => x.ClientId).IsRequired();

                client.HasIndex(x => x.ClientId).IsUnique();
            });

            builder.Entity<ClientGrantType>(grantType =>
            {
                grantType.ToTable(tablePrefix + "ClientGrantTypes", schema);

                grantType.HasKey(x => new { x.ClientId, x.GrantType });

                grantType.Property(x => x.GrantType).HasMaxLength(ClientGrantTypeConsts.GrantTypeMaxLength).IsRequired();
            });

            builder.Entity<ClientRedirectUri>(redirectUri =>
            {
                redirectUri.ToTable(tablePrefix + "ClientRedirectUris", schema);

                redirectUri.HasKey(x => new { x.ClientId, x.RedirectUri });

                redirectUri.Property(x => x.RedirectUri).HasMaxLength(ClientRedirectUriConsts.RedirectUriMaxLength).IsRequired();
            });

            builder.Entity<ClientPostLogoutRedirectUri>(postLogoutRedirectUri =>
            {
                postLogoutRedirectUri.ToTable(tablePrefix + "ClientPostLogoutRedirectUris", schema);

                postLogoutRedirectUri.HasKey(x => new { x.ClientId, x.PostLogoutRedirectUri });

                postLogoutRedirectUri.Property(x => x.PostLogoutRedirectUri).HasMaxLength(ClientPostLogoutRedirectUriConsts.PostLogoutRedirectUriMaxLength).IsRequired();
            });

            builder.Entity<ClientScope>(scope =>
            {
                scope.ToTable(tablePrefix + "ClientScopes", schema);

                scope.HasKey(x => new { x.ClientId, x.Scope });

                scope.Property(x => x.Scope).HasMaxLength(ClientScopeConsts.ScopeMaxLength).IsRequired();
            });

            builder.Entity<ClientSecret>(secret =>
            {
                secret.ToTable(tablePrefix + "ClientSecrets", schema);

                secret.HasKey(x => new { x.ClientId, x.Type, x.Value });

                secret.Property(x => x.Type).HasMaxLength(SecretConsts.TypeMaxLength).IsRequired();
                secret.Property(x => x.Value).HasMaxLength(SecretConsts.ValueMaxLength).IsRequired();
                secret.Property(x => x.Description).HasMaxLength(SecretConsts.DescriptionMaxLength);
            });

            builder.Entity<ClientClaim>(claim =>
            {
                claim.ToTable(tablePrefix + "ClientClaims", schema);

                claim.Property(x => x.Type).HasMaxLength(ClientClaimConsts.TypeMaxLength).IsRequired();
                claim.Property(x => x.Value).HasMaxLength(ClientClaimConsts.ValueMaxLength).IsRequired();
            });

            builder.Entity<ClientIdPRestriction>(idPRestriction =>
            {
                idPRestriction.ToTable(tablePrefix + "ClientIdPRestrictions", schema);

                idPRestriction.HasKey(x => new { x.ClientId, x.Provider });

                idPRestriction.Property(x => x.Provider).HasMaxLength(ClientIdPRestrictionConsts.ProviderMaxLength).IsRequired();
            });

            builder.Entity<ClientCorsOrigin>(corsOrigin =>
            {
                corsOrigin.ToTable(tablePrefix + "ClientCorsOrigins", schema);

                corsOrigin.HasKey(x => new { x.ClientId, x.Origin });

                corsOrigin.Property(x => x.Origin).HasMaxLength(ClientCorsOriginConsts.OriginMaxLength).IsRequired();
            });

            builder.Entity<ClientProperty>(property =>
            {
                property.ToTable(tablePrefix + "ClientProperties", schema);

                property.HasKey(x => new { x.ClientId, x.Key });

                property.Property(x => x.Key).HasMaxLength(ClientPropertyConsts.KeyMaxLength).IsRequired();
                property.Property(x => x.Value).HasMaxLength(ClientPropertyConsts.ValueMaxLength).IsRequired();
            });

            builder.Entity<PersistedGrant>(grant =>
            {
                grant.ToTable(tablePrefix + "PersistedGrants", schema);

                grant.Property(x => x.Key).HasMaxLength(PersistedGrantConsts.KeyMaxLength).ValueGeneratedNever();
                grant.Property(x => x.Type).HasMaxLength(PersistedGrantConsts.TypeMaxLength).IsRequired();
                grant.Property(x => x.SubjectId).HasMaxLength(PersistedGrantConsts.SubjectIdMaxLength);
                grant.Property(x => x.ClientId).HasMaxLength(PersistedGrantConsts.ClientIdMaxLength).IsRequired();
                grant.Property(x => x.CreationTime).IsRequired();
                grant.Property(x => x.Data).IsRequired();

                grant.HasKey(x => x.Key); //TODO: What about Id!!!

                grant.HasIndex(x => new { x.SubjectId, x.ClientId, x.Type });
            });

            builder.Entity<IdentityResource>(identityResource =>
            {
                identityResource.ToTable(tablePrefix + "IdentityResources", schema);

                identityResource.Property(x => x.Name).HasMaxLength(IdentityResourceConsts.NameMaxLength).IsRequired();
                identityResource.Property(x => x.DisplayName).HasMaxLength(IdentityResourceConsts.DisplayNameMaxLength);
                identityResource.Property(x => x.Description).HasMaxLength(IdentityResourceConsts.DescriptionMaxLength);

                identityResource.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.IdentityResourceId).IsRequired();
            });

            builder.Entity<IdentityClaim>(claim =>
            {
                claim.ToTable(tablePrefix + "IdentityClaims", schema);

                claim.HasKey(x => new { x.IdentityResourceId, x.Type });

                claim.Property(x => x.Type).HasMaxLength(UserClaimConsts.TypeMaxLength).IsRequired();
            });

            builder.Entity<ApiResource>(apiResource =>
            {
                apiResource.ToTable(tablePrefix + "ApiResources", schema);

                apiResource.Property(x => x.Name).HasMaxLength(ApiResourceConsts.NameMaxLength).IsRequired();
                apiResource.Property(x => x.DisplayName).HasMaxLength(ApiResourceConsts.DisplayNameMaxLength);
                apiResource.Property(x => x.Description).HasMaxLength(ApiResourceConsts.DescriptionMaxLength);

                apiResource.HasMany(x => x.Secrets).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
                apiResource.HasMany(x => x.Scopes).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
                apiResource.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
            });

            builder.Entity<ApiSecret>(apiSecret =>
            {
                apiSecret.ToTable(tablePrefix + "ApiSecrets", schema);

                apiSecret.HasKey(x => new { x.ApiResourceId, x.Type, x.Value });

                apiSecret.Property(x => x.Type).HasMaxLength(SecretConsts.TypeMaxLength).IsRequired();
                apiSecret.Property(x => x.Value).HasMaxLength(SecretConsts.ValueMaxLength).IsRequired();
                apiSecret.Property(x => x.Description).HasMaxLength(SecretConsts.DescriptionMaxLength);
            });

            builder.Entity<ApiResourceClaim>(apiClaim =>
            {
                apiClaim.ToTable(tablePrefix + "ApiClaims", schema);

                apiClaim.HasKey(x => new { x.ApiResourceId, x.Type });

                apiClaim.Property(x => x.Type).HasMaxLength(UserClaimConsts.TypeMaxLength).IsRequired();
            });

            builder.Entity<ApiScope>(apiScope =>
            {
                apiScope.ToTable(tablePrefix + "ApiScopes", schema);

                apiScope.HasKey(x => new { x.ApiResourceId, x.Name });

                apiScope.Property(x => x.Name).HasMaxLength(ApiScopeConsts.NameMaxLength).IsRequired();
                apiScope.Property(x => x.DisplayName).HasMaxLength(ApiScopeConsts.DisplayNameMaxLength);
                apiScope.Property(x => x.Description).HasMaxLength(ApiScopeConsts.DescriptionMaxLength);

                apiScope.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => new { x.ApiResourceId, x.Name }).IsRequired();
            });

            builder.Entity<ApiScopeClaim>(apiScopeClaim =>
            {
                apiScopeClaim.ToTable(tablePrefix + "ApiScopeClaims", schema);

                apiScopeClaim.HasKey(x => new { x.ApiResourceId, x.Name, x.Type });

                apiScopeClaim.Property(x => x.Type).HasMaxLength(UserClaimConsts.TypeMaxLength).IsRequired();
                apiScopeClaim.Property(x => x.Name).HasMaxLength(ApiScopeConsts.NameMaxLength).IsRequired();
            });
        }
    }
}
