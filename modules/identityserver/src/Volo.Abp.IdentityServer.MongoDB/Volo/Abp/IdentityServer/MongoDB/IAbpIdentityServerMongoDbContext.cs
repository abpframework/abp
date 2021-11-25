using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Devices;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.IdentityServer.MongoDB;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpIdentityServerDbProperties.ConnectionStringName)]
public interface IAbpIdentityServerMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<ApiResource> ApiResources { get; }

    IMongoCollection<ApiScope> ApiScopes { get; }

    IMongoCollection<Client> Clients { get; }

    IMongoCollection<IdentityResource> IdentityResources { get; }

    IMongoCollection<PersistedGrant> PersistedGrants { get; }

    IMongoCollection<DeviceFlowCodes> DeviceFlowCodes { get; }
}
