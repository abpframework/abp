using System;

namespace Volo.Abp.MongoDB
{
    public class MongoEntityMapping
    {
        public Type EntityType { get; set; }

        public string CollectionName { get; set; }

        public MongoEntityMapping(Type entityType, string collectionName = null)
        {
            EntityType = entityType;
            CollectionName = collectionName ?? entityType.Name;
        }
    }
}