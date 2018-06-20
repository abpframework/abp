using MongoDB.Bson.Serialization;
using Volo.Abp.MongoDB;
using Volo.Abp.Threading;

namespace Volo.Abp.TenantManagement.MongoDb
{
    public static class AbpTenantManagementBsonClassMap
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                BsonClassMap.RegisterClassMap<Tenant>(map =>
                {
                    map.AutoMap();
                    map.ConfigureExtraProperties();
                });
            });
        }
    }
}