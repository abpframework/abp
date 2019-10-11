using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.MongoDB;
using IdentityResource = Volo.Abp.IdentityServer.IdentityResources.IdentityResource;

namespace Volo.Abp.IdentityServer.MongoDB
{
    [ConnectionStringName(AbpIdentityServerDbProperties.ConnectionStringName)]
    public class AbpIdentityServerMongoDbContext : AbpMongoDbContext, IAbpIdentityServerMongoDbContext
    {
        public IMongoCollection<ApiResource> ApiResources => Collection<ApiResource>();

        public IMongoCollection<Client> Clients => Collection<Client>();

        public IMongoCollection<IdentityResource> IdentityResources => Collection<IdentityResource>();

        public IMongoCollection<PersistedGrant> PersistedGrants => Collection<PersistedGrant>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureIdentityServer();
        }
    }
}
