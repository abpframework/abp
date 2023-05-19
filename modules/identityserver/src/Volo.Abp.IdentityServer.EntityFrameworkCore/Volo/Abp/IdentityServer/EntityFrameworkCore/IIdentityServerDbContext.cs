using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Devices;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.IdentityServer.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpIdentityServerDbProperties.ConnectionStringName)]
public interface IIdentityServerDbContext : IEfCoreDbContext
{
    #region ApiResource

    DbSet<ApiResource> ApiResources { get; }

    DbSet<ApiResourceSecret> ApiResourceSecrets { get; }

    DbSet<ApiResourceClaim> ApiResourceClaims { get; }

    DbSet<ApiResourceScope> ApiResourceScopes { get; }

    DbSet<ApiResourceProperty> ApiResourceProperties { get; }

    #endregion

    #region ApiScope

    DbSet<ApiScope> ApiScopes { get; }

    DbSet<ApiScopeClaim> ApiScopeClaims { get; }

    DbSet<ApiScopeProperty> ApiScopeProperties { get; }

    #endregion

    #region IdentityResource

    DbSet<IdentityResource> IdentityResources { get; }

    DbSet<IdentityResourceClaim> IdentityClaims { get; }

    DbSet<IdentityResourceProperty> IdentityResourceProperties { get; }

    #endregion

    #region Client

    DbSet<Client> Clients { get; }

    DbSet<ClientGrantType> ClientGrantTypes { get; }

    DbSet<ClientRedirectUri> ClientRedirectUris { get; }

    DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; }

    DbSet<ClientScope> ClientScopes { get; }

    DbSet<ClientSecret> ClientSecrets { get; }

    DbSet<ClientClaim> ClientClaims { get; }

    DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; }

    DbSet<ClientCorsOrigin> ClientCorsOrigins { get; }

    DbSet<ClientProperty> ClientProperties { get; }

    #endregion

    DbSet<PersistedGrant> PersistedGrants { get; }

    DbSet<DeviceFlowCodes> DeviceFlowCodes { get; }
}
