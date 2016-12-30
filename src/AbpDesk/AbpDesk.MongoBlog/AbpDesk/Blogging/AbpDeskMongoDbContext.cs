using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace AbpDesk.Blogging
{
    [ConnectionStringName(ConnectionStringName)]
    public class AbpDeskMongoDbContext : AbpMongoDbContext
    {
        public const string ConnectionStringName = "AbpDeskMongoBlog";

        private static readonly Type[] EntityCollectionTypes = {
            typeof(BlogPost)
        };

        public override IReadOnlyList<Type> GetEntityCollectionTypes()
        {
            return EntityCollectionTypes;
        }
    }
}