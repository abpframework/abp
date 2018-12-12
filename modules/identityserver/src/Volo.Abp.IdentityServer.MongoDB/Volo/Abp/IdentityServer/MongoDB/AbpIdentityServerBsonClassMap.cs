using MongoDB.Bson.Serialization;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.MongoDB;
using Volo.Abp.Threading;

namespace Volo.Abp.IdentityServer.MongoDB
{
    public class AbpIdentityServerBsonClassMap
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                BsonClassMap.RegisterClassMap<ApiResource>(map =>
                {
                    map.AutoMap();
                    map.ConfigureExtraProperties();
                });
                BsonClassMap.RegisterClassMap<Client>(map =>
                {
                    map.AutoMap();
                    map.ConfigureExtraProperties();
                });
                BsonClassMap.RegisterClassMap<IdentityResource>(map =>
                {
                    map.AutoMap();
                    map.ConfigureExtraProperties();
                });
                BsonClassMap.RegisterClassMap<PersistedGrant>(map =>
                {
                    map.AutoMap();
                    map.ConfigureExtraProperties();
                });
            });
        }
    }
}
