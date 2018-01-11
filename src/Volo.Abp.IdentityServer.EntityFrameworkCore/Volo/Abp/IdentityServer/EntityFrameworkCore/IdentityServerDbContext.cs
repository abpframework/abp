using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;

namespace Volo.Abp.IdentityServer.EntityFrameworkCore
{
    public class IdentityServerDbContext : AbpDbContext<IdentityServerDbContext>
    {
        public const string TablePrefix = "AbpIds";

        public DbSet<Client> Clients { get; set; }

        public DbSet<PersistedGrant> PersistedGrants { get; set; }

        public DbSet<ApiResource> ApiResources { get; set; }

        public DbSet<IdentityResource> IdentityResources { get; set; }

        public DbSet<ClientGrantType> ClientGrantTypes { get; set; }

        public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }

        public DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }

        public DbSet<ClientScope> ClientScopes { get; set; }

        public DbSet<ClientSecret> ClientSecrets { get; set; }

        public DbSet<ClientClaim> ClientClaims { get; set; }

        public DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; set; }

        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }

        public DbSet<ClientProperty> ClientProperties { get; set; }

        public DbSet<IdentityClaim> IdentityClaims { get; set; }
        
        public DbSet<ApiSecret> ApiSecrets { get; set; }

        public DbSet<ApiResourceClaim> ApiResourceClaims { get; set; }

        public DbSet<ApiScope> ApiScopes { get; set; }

        public DbSet<ApiScopeClaim> ApiScopeClaims { get; set; }
            
        public IdentityServerDbContext(DbContextOptions<IdentityServerDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Client>(client =>
            {
                client.ToTable(TablePrefix + "Clients");

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

                client.HasMany(x => x.AllowedGrantTypes).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                client.HasMany(x => x.RedirectUris).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                client.HasMany(x => x.PostLogoutRedirectUris).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                client.HasMany(x => x.AllowedScopes).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                client.HasMany(x => x.ClientSecrets).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                client.HasMany(x => x.Claims).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                client.HasMany(x => x.IdentityProviderRestrictions).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                client.HasMany(x => x.AllowedCorsOrigins).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
                client.HasMany(x => x.Properties).WithOne().HasForeignKey(x => x.ClientId).IsRequired();

                client.HasIndex(x => x.ClientId).IsUnique();
            });

            builder.Entity<ClientGrantType>(grantType =>
            {
                grantType.ToTable(TablePrefix + "ClientGrantTypes");

                grantType.HasKey(x => new {x.ClientId, x.GrantType});

                grantType.Property(x => x.ClientId).IsRequired();
                grantType.Property(x => x.GrantType).HasMaxLength(ClientGrantTypeConsts.GrantTypeMaxLength).IsRequired();
            });
            
            builder.Entity<ClientRedirectUri>(redirectUri =>
            {
                redirectUri.ToTable(TablePrefix + "ClientRedirectUris");

                redirectUri.Property(x => x.RedirectUri).HasMaxLength(ClientRedirectUriConsts.RedirectUriMaxLength).IsRequired();
            });

            builder.Entity<ClientPostLogoutRedirectUri>(postLogoutRedirectUri =>
            {
                postLogoutRedirectUri.ToTable(TablePrefix + "ClientPostLogoutRedirectUris");

                postLogoutRedirectUri.Property(x => x.PostLogoutRedirectUri).HasMaxLength(ClientPostLogoutRedirectUriConsts.PostLogoutRedirectUriMaxLength).IsRequired();
            });

            builder.Entity<ClientScope>(scope =>
            {
                scope.ToTable(TablePrefix + "ClientScopes");

                scope.Property(x => x.Scope).HasMaxLength(ClientScopeConsts.ScopeMaxLength).IsRequired();
            });

            builder.Entity<ClientSecret>(secret =>
            {
                secret.ToTable(TablePrefix + "ClientSecrets");

                secret.Property(x => x.Value).HasMaxLength(ClientSecretConsts.ValueMaxLength).IsRequired();
                secret.Property(x => x.Type).HasMaxLength(ClientSecretConsts.TypeMaxLength);
                secret.Property(x => x.Description).HasMaxLength(ClientSecretConsts.DescriptionMaxLength);
            });

            builder.Entity<ClientClaim>(claim =>
            {
                claim.ToTable(TablePrefix + "ClientClaims");

                claim.Property(x => x.Type).HasMaxLength(ClientClaimConsts.TypeMaxLength).IsRequired();
                claim.Property(x => x.Value).HasMaxLength(ClientClaimConsts.ValueMaxLength).IsRequired();
            });

            builder.Entity<ClientIdPRestriction>(idPRestriction =>
            {
                idPRestriction.ToTable(TablePrefix + "ClientIdPRestrictions");

                idPRestriction.Property(x => x.Provider).HasMaxLength(ClientIdPRestrictionConsts.ProviderMaxLength).IsRequired();
            });

            builder.Entity<ClientCorsOrigin>(corsOrigin =>
            {
                corsOrigin.ToTable(TablePrefix + "ClientCorsOrigins");

                corsOrigin.Property(x => x.Origin).HasMaxLength(ClientCorsOriginConsts.OriginMaxLength).IsRequired();
            });

            builder.Entity<ClientProperty>(property =>
            {
                property.ToTable(TablePrefix + "ClientProperties");

                property.Property(x => x.Key).HasMaxLength(ClientPropertyConsts.KeyMaxLength).IsRequired();
                property.Property(x => x.Value).HasMaxLength(ClientPropertyConsts.ValueMaxLength).IsRequired();
            });

            builder.Entity<PersistedGrant>(grant =>
            {
                grant.ToTable(TablePrefix + "PersistedGrants");

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
                identityResource.ToTable(TablePrefix + "IdentityResources");

                identityResource.Property(x => x.Name).HasMaxLength(IdentityResourceConsts.NameMaxLength).IsRequired();
                identityResource.Property(x => x.DisplayName).HasMaxLength(IdentityResourceConsts.DisplayNameMaxLength);
                identityResource.Property(x => x.Description).HasMaxLength(IdentityResourceConsts.DescriptionMaxLength);

                identityResource.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.IdentityResourceId).IsRequired();

                identityResource.HasIndex(x => x.Name).IsUnique();
            });

            builder.Entity<IdentityClaim>(claim =>
            {
                claim.ToTable(TablePrefix + "IdentityClaims");

                claim.Property(x => x.Type).HasMaxLength(IdentityClaimConsts.TypeMaxLength).IsRequired();
            });

            builder.Entity<ApiResource>(apiResource =>
            {
                apiResource.ToTable(TablePrefix + "ApiResources");

                apiResource.Property(x => x.Name).HasMaxLength(ApiResourceConsts.NameMaxLength).IsRequired();
                apiResource.Property(x => x.DisplayName).HasMaxLength(ApiResourceConsts.DisplayNameMaxLength);
                apiResource.Property(x => x.Description).HasMaxLength(ApiResourceConsts.DescriptionMaxLength);

                apiResource.HasMany(x => x.Secrets).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
                apiResource.HasMany(x => x.Scopes).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
                apiResource.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();

                apiResource.HasIndex(x => x.Name).IsUnique();
            });

            builder.Entity<ApiSecret>(apiSecret =>
            {
                apiSecret.ToTable(TablePrefix + "ApiSecrets");

                apiSecret.Property(x => x.Description).HasMaxLength(ApiSecretConsts.DescriptionMaxLength);
                apiSecret.Property(x => x.Value).HasMaxLength(ApiSecretConsts.ValueMaxLength);
                apiSecret.Property(x => x.Type).HasMaxLength(ApiSecretConsts.TypeMaxLength);
            });

            builder.Entity<ApiResourceClaim>(apiClaim =>
            {
                apiClaim.ToTable(TablePrefix + "ApiClaims");

                apiClaim.Property(x => x.Type).HasMaxLength(ApiResourceClaimConsts.TypeMaxLength).IsRequired();
            });

            builder.Entity<ApiScope>(apiScope =>
            {
                apiScope.ToTable(TablePrefix + "ApiScopes");

                apiScope.Property(x => x.Name).HasMaxLength(ApiScopeConsts.NameMaxLength).IsRequired();
                apiScope.Property(x => x.DisplayName).HasMaxLength(ApiScopeConsts.DisplayNameMaxLength);
                apiScope.Property(x => x.Description).HasMaxLength(ApiScopeConsts.DescriptionMaxLength);

                apiScope.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.ApiScopeId).IsRequired();

                apiScope.HasIndex(x => x.Name).IsUnique();
            });

            builder.Entity<ApiScopeClaim>(apiScopeClaim =>
            {
                apiScopeClaim.ToTable(TablePrefix + "ApiScopeClaims");

                apiScopeClaim.Property(x => x.Type).HasMaxLength(ApiScopeClaimConsts.TypeMaxLength).IsRequired();
            });
        }
    }
}
