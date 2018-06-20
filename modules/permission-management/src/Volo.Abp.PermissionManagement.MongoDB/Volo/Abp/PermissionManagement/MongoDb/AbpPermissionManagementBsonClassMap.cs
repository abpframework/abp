using MongoDB.Bson.Serialization;
using Volo.Abp.Threading;

namespace Volo.Abp.PermissionManagement.MongoDB
{
    public static class AbpPermissionManagementBsonClassMap
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                BsonClassMap.RegisterClassMap<PermissionGrant>(map =>
                {
                    map.AutoMap();
                });
            });
        }
    }
}