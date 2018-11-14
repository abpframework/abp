using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization;
using Volo.Abp.MongoDB;
using Volo.Abp.Threading;
using Volo.Blogging.Users;

namespace Volo.Blogging.MongoDB
{
    public static class AbpBloggingBsonClassMap
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                BsonClassMap.RegisterClassMap<BlogUser>(map =>
                {
                    map.AutoMap();
                    map.ConfigureExtraProperties();
                });
            });
        }
    }
}
