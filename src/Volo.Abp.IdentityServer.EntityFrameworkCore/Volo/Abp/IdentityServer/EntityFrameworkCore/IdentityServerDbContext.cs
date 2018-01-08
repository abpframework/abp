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

                client.HasKey(x => x.Id);

                client.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
                client.Property(x => x.ProtocolType).HasMaxLength(200).IsRequired();
                client.Property(x => x.ClientName).HasMaxLength(200);
                client.Property(x => x.ClientUri).HasMaxLength(2000);
                client.Property(x => x.LogoUri).HasMaxLength(2000);
                client.Property(x => x.Description).HasMaxLength(1000);
                client.Property(x => x.FrontChannelLogoutUri).HasMaxLength(2000);
                client.Property(x => x.BackChannelLogoutUri).HasMaxLength(2000);
                client.Property(x => x.ClientClaimsPrefix).HasMaxLength(200);
                client.Property(x => x.PairWiseSubjectSalt).HasMaxLength(200);

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
                grantType.Property(x => x.GrantType).HasMaxLength(250).IsRequired();

                grantType.Property(x => x.ClientId).IsRequired();
            });

            builder.Entity<ClientRedirectUri>(redirectUri =>
            {
                redirectUri.ToTable(TablePrefix + "ClientRedirectUris");
                redirectUri.Property(x => x.RedirectUri).HasMaxLength(2000).IsRequired();
            });

            builder.Entity<ClientPostLogoutRedirectUri>(postLogoutRedirectUri =>
            {
                postLogoutRedirectUri.ToTable(TablePrefix + "ClientPostLogoutRedirectUris");
                postLogoutRedirectUri.Property(x => x.PostLogoutRedirectUri).HasMaxLength(2000).IsRequired();
            });

            builder.Entity<ClientScope>(scope =>
            {
                scope.ToTable(TablePrefix + "ClientScopes");
                scope.Property(x => x.Scope).HasMaxLength(200).IsRequired();
            });

            builder.Entity<ClientSecret>(secret =>
            {
                secret.ToTable(TablePrefix + "ClientSecrets");
                secret.Property(x => x.Value).HasMaxLength(2000).IsRequired();
                secret.Property(x => x.Type).HasMaxLength(250);
                secret.Property(x => x.Description).HasMaxLength(2000);
            });

            builder.Entity<ClientClaim>(claim =>
            {
                claim.ToTable(TablePrefix + "ClientClaims");
                claim.Property(x => x.Type).HasMaxLength(250).IsRequired();
                claim.Property(x => x.Value).HasMaxLength(250).IsRequired();
            });

            builder.Entity<ClientIdPRestriction>(idPRestriction =>
            {
                idPRestriction.ToTable(TablePrefix + "ClientIdPRestrictions");
                idPRestriction.Property(x => x.Provider).HasMaxLength(200).IsRequired();
            });

            builder.Entity<ClientCorsOrigin>(corsOrigin =>
            {
                corsOrigin.ToTable(TablePrefix + "ClientCorsOrigins");
                corsOrigin.Property(x => x.Origin).HasMaxLength(150).IsRequired();
            });

            builder.Entity<ClientProperty>(property =>
            {
                property.ToTable(TablePrefix + "ClientProperties");
                property.Property(x => x.Key).HasMaxLength(250).IsRequired();
                property.Property(x => x.Value).HasMaxLength(2000).IsRequired();
            });

            builder.Entity<PersistedGrant>(grant =>
            {
                grant.ToTable(TablePrefix + "PersistedGrants");

                grant.Property(x => x.Key).HasMaxLength(200).ValueGeneratedNever();
                grant.Property(x => x.Type).HasMaxLength(50).IsRequired();
                grant.Property(x => x.SubjectId).HasMaxLength(200);
                grant.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
                grant.Property(x => x.CreationTime).IsRequired();
                grant.Property(x => x.Data).IsRequired();

                grant.HasKey(x => x.Key);

                grant.HasIndex(x => new { x.SubjectId, x.ClientId, x.Type });
            });

            builder.Entity<IdentityResource>(identityResource =>
            {
                identityResource.ToTable(TablePrefix + "IdentityResources").HasKey(x => x.Id);

                identityResource.Property(x => x.Name).HasMaxLength(200).IsRequired();
                identityResource.Property(x => x.DisplayName).HasMaxLength(200);
                identityResource.Property(x => x.Description).HasMaxLength(1000);

                identityResource.HasIndex(x => x.Name).IsUnique();


                identityResource.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.IdentityResourceId).IsRequired();
            });

            builder.Entity<IdentityClaim>(claim =>
            {
                claim.ToTable(TablePrefix + "IdentityClaims").HasKey(x => x.Id);

                claim.Property(x => x.Type).HasMaxLength(200).IsRequired();
            });

            builder.Entity<ApiResource>(apiResource =>
            {
                apiResource.ToTable(TablePrefix + "ApiResources").HasKey(x => x.Id);

                apiResource.Property(x => x.Name).HasMaxLength(200).IsRequired();
                apiResource.Property(x => x.DisplayName).HasMaxLength(200);
                apiResource.Property(x => x.Description).HasMaxLength(1000);

                apiResource.HasIndex(x => x.Name).IsUnique();

                apiResource.HasMany(x => x.Secrets).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
                apiResource.HasMany(x => x.Scopes).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
                apiResource.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
            });

            builder.Entity<ApiSecret>(apiSecret =>
            {
                apiSecret.ToTable(TablePrefix + "ApiSecrets").HasKey(x => x.Id);

                apiSecret.Property(x => x.Description).HasMaxLength(1000);
                apiSecret.Property(x => x.Value).HasMaxLength(2000);
                apiSecret.Property(x => x.Type).HasMaxLength(250);
            });

            builder.Entity<ApiResourceClaim>(apiClaim =>
            {
                apiClaim.ToTable(TablePrefix + "ApiClaims").HasKey(x => x.Id);

                apiClaim.Property(x => x.Type).HasMaxLength(200).IsRequired();
            });

            builder.Entity<ApiScope>(apiScope =>
            {
                apiScope.ToTable(TablePrefix + "ApiScopes").HasKey(x => x.Id);

                apiScope.Property(x => x.Name).HasMaxLength(200).IsRequired();
                apiScope.Property(x => x.DisplayName).HasMaxLength(200);
                apiScope.Property(x => x.Description).HasMaxLength(1000);

                apiScope.HasIndex(x => x.Name).IsUnique();

                apiScope.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.ApiScopeId).IsRequired();
            });

            builder.Entity<ApiScopeClaim>(apiScopeClaim =>
            {
                apiScopeClaim.ToTable(TablePrefix + "ApiScopeClaims").HasKey(x => x.Id);

                apiScopeClaim.Property(x => x.Type).HasMaxLength(200).IsRequired();
            });
        }
    }
}
