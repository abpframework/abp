using MongoDB.Bson.Serialization;
using Volo.Abp.Threading;

namespace Volo.Abp.AuditLogging.MongoDB
{
    public static class AbpAuditLoggingBsonClassMap
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                BsonClassMap.RegisterClassMap<AuditLog>(map =>
                {
                    map.AutoMap();
                });
            });
        }
    }
}
