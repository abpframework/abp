using MongoDB.Bson.Serialization;
using Volo.Abp.MongoDB;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity.MongoDB
{
    public static class AbpIdentityBsonClassMap
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                BsonClassMap.RegisterClassMap<IdentityUser>(map =>
                {
                    map.AutoMap();
                    map.ConfigureExtraProperties();
                });

                BsonClassMap.RegisterClassMap<IdentityRole>(map =>
                {
                    map.AutoMap();
                    map.ConfigureExtraProperties();
                });

                BsonClassMap.RegisterClassMap<IdentityClaimType>(map =>
                {
                    map.AutoMap();
                });
            });
        }
    }
}