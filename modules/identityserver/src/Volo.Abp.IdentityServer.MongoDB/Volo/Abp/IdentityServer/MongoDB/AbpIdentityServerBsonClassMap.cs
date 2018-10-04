using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4.Models;
using MongoDB.Bson.Serialization;
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
                });
                BsonClassMap.RegisterClassMap<Client>(map =>
                {
                    map.AutoMap();
                });
                BsonClassMap.RegisterClassMap<IdentityResource>(map =>
                {
                    map.AutoMap();
                });
                BsonClassMap.RegisterClassMap<PersistedGrant>(map =>
                {
                    map.AutoMap();
                });
            });
        }
    }
}
