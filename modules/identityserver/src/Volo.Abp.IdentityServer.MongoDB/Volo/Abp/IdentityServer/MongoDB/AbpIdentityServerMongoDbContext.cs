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

namespace Volo.Abp.IdentityServer.MongoDB
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(AbpIdentityServerDbProperties.ConnectionStringName)]
    public class AbpIdentityServerMongoDbContext : AbpMongoDbContext, IAbpIdentityServerMongoDbContext
    {
        public IMongoCollection<ApiResource> ApiResources => Collection<ApiResource>();

        public IMongoCollection<ApiScope> ApiScopes => Collection<ApiScope>();

        public IMongoCollection<Client> Clients => Collection<Client>();

        public IMongoCollection<IdentityResource> IdentityResources => Collection<IdentityResource>();

        public IMongoCollection<PersistedGrant> PersistedGrants => Collection<PersistedGrant>();

        public IMongoCollection<DeviceFlowCodes> DeviceFlowCodes => Collection<DeviceFlowCodes>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureIdentityServer();
        }
    }
}
