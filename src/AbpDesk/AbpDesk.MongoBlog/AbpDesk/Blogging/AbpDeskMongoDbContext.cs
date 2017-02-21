using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace AbpDesk.Blogging
{
    [ConnectionStringName(ConnectionStringName)]
    public class AbpDeskMongoDbContext : AbpMongoDbContext
    {
        public const string ConnectionStringName = "AbpDeskMongoBlog";

        private static readonly MongoEntityMapping[] EntityCollectionTypes = {
            new MongoEntityMapping(typeof(BlogPost), "BlogPosts")
        };

        public override IReadOnlyList<MongoEntityMapping> GetMappings()
        {
            return EntityCollectionTypes;
        }
    }
}