using MongoDB.Bson.Serialization;
using Volo.Abp.Threading;

namespace Volo.Abp.SettingManagement.MongoDB
{
    public static class AbpSettingManagementBsonClassMap
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                BsonClassMap.RegisterClassMap<Setting>(map =>
                {
                    map.AutoMap();
                });
            });
        }
    }
}