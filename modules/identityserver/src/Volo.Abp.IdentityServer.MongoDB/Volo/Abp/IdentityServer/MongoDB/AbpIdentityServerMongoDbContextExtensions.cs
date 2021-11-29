using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Devices;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.MongoDB;

namespace Volo.Abp.IdentityServer.MongoDB
{
    public static class AbpIdentityServerMongoDbContextExtensions
    {
        public static void ConfigureIdentityServer(
            this IMongoModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<ApiResource>(b =>
            {
                b.CollectionName = AbpIdentityServerDbProperties.DbTablePrefix + "ApiResources";
            });

            builder.Entity<ApiScope>(b =>
            {
                b.CollectionName = AbpIdentityServerDbProperties.DbTablePrefix + "ApiScopes";
            });

            builder.Entity<IdentityResource>(b =>
            {
                b.CollectionName = AbpIdentityServerDbProperties.DbTablePrefix + "IdentityResources";
            });

            builder.Entity<Client>(b =>
            {
                b.CollectionName = AbpIdentityServerDbProperties.DbTablePrefix + "Clients";
            });

            builder.Entity<PersistedGrant>(b =>
            {
                b.CollectionName = AbpIdentityServerDbProperties.DbTablePrefix + "PersistedGrants";
            });

            builder.Entity<DeviceFlowCodes>(b =>
            {
                b.CollectionName = AbpIdentityServerDbProperties.DbTablePrefix + "DeviceFlowCodes";
            });
        }
    }
}
