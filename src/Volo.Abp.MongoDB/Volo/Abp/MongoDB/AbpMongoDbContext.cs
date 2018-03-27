using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;

namespace Volo.Abp.MongoDB
{
    public abstract class AbpMongoDbContext : IAbpMongoDbContext
    {
        private static readonly MongoEntityMapping[] EmptyTypeList = new MongoEntityMapping[0];

        public IMongoDatabase Database { get; private set; }

        private readonly Lazy<Dictionary<Type, MongoEntityMapping>> _mappingsByType;

        protected AbpMongoDbContext()
        {
            //TODO: Cache model/mappings

            _mappingsByType = new Lazy<Dictionary<Type, MongoEntityMapping>>(() =>
            {
                return GetMappings().ToDictionary(m => m.EntityType);
            }, true);
        }

        public virtual IReadOnlyList<MongoEntityMapping> GetMappings()
        {
            return EmptyTypeList;
        }

        public virtual IMongoCollection<T> Collection<T>()
        {
            return Database.GetCollection<T>(GetCollectionName<T>());
        }

        public virtual string GetCollectionName<T>()
        {
            return GetMapping<T>().CollectionName;
        }

        protected virtual MongoEntityMapping GetMapping<T>()
        {
            var mapping = _mappingsByType.Value.GetOrDefault(typeof(T));
            if (mapping == null)
            {
                throw new AbpException("Unmapped entity type: " + typeof(T).AssemblyQualifiedName);
            }

            return mapping;
        }

        public void InitializeDatabase(IMongoDatabase database)
        {
            Database = database;
        }
    }
}