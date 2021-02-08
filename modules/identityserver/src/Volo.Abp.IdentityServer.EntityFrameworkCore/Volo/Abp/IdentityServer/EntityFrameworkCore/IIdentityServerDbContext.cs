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

namespace Volo.Abp.IdentityServer.EntityFrameworkCore
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(AbpIdentityServerDbProperties.ConnectionStringName)]
    public interface IIdentityServerDbContext : IEfCoreDbContext
    {
        #region ApiResource

        DbSet<ApiResource> ApiResources { get; set; }

        DbSet<ApiResourceSecret> ApiResourceSecrets { get; set; }

        DbSet<ApiResourceClaim> ApiResourceClaims { get; set; }

        DbSet<ApiResourceScope> ApiResourceScopes { get; set; }

        DbSet<ApiResourceProperty> ApiResourceProperties { get; set; }

        #endregion

        #region ApiScope

        DbSet<ApiScope> ApiScopes { get; set; }

        DbSet<ApiScopeClaim> ApiScopeClaims { get; set; }

        DbSet<ApiScopeProperty> ApiScopeProperties { get; set; }

        #endregion

        #region IdentityResource

        DbSet<IdentityResource> IdentityResources { get; set; }

        DbSet<IdentityResourceClaim> IdentityClaims { get; set; }

        DbSet<IdentityResourceProperty> IdentityResourceProperties { get; set; }

        #endregion

        #region Client

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

            #endregion

        DbSet<PersistedGrant> PersistedGrants { get; set; }

        DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }
    }
}
