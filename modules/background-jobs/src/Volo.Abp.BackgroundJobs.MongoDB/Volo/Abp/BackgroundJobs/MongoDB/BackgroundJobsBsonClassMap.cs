using MongoDB.Bson.Serialization;
using Volo.Abp.MongoDB;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.MongoDB
{
    public static class BackgroundJobsBsonClassMap
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                BsonClassMap.RegisterClassMap<BackgroundJobRecord>(map =>
                {
                    map.AutoMap();
                    map.ConfigureExtraProperties();
                });
            });
        }
    }
}