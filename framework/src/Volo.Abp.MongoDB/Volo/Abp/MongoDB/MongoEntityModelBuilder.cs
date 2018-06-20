using System;

namespace Volo.Abp.MongoDB
{
    public class MongoEntityModelBuilder : IMongoEntityModel
    {
        public Type EntityType { get; }

        public string CollectionName { get; set; }

        public MongoEntityModelBuilder(Type entityType)
        {
            Check.NotNull(entityType, nameof(entityType));

            EntityType = entityType;
        }
    }
}