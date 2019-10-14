using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;

namespace Volo.Abp.IdentityServer.EntityFrameworkCore
{
    [ConnectionStringName(AbpIdentityServerDbProperties.ConnectionStringName)]
    public interface IIdentityServerDbContext : IEfCoreDbContext
    {
        DbSet<ApiResource> ApiResources { get; set; }

        DbSet<ApiSecret> ApiSecrets { get; set; }

        DbSet<ApiResourceClaim> ApiResourceClaims { get; set; }

        DbSet<ApiScope> ApiScopes { get; set; }

        DbSet<ApiScopeClaim> ApiScopeClaims { get; set; }

        DbSet<IdentityResource> IdentityResources { get; set; }

        DbSet<IdentityClaim> IdentityClaims { get; set; }

        DbSet<Client> Clients { get; set; }

        DbSet<ClientGrantType> ClientGrantTypes { get; set; }

        DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }

        DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }

        DbSet<ClientScope> ClientScopes { get; set; }

        DbSet<ClientSecret> ClientSecrets { get; set; }

        DbSet<ClientClaim> ClientClaims { get; set; }

        DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; set; }

        DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }

        DbSet<ClientProperty> ClientProperties { get; set; }

        DbSet<PersistedGrant> PersistedGrants { get; set; }
    }
}
