using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.MongoDB
{
    public abstract class AbpMongoDbContext : IAbpMongoDbContext
    {
        private static readonly MongoEntityMapping[] EmptyTypeList = new MongoEntityMapping[0];

        private readonly Lazy<Dictionary<Type, MongoEntityMapping>> _mappingsByType;

        protected AbpMongoDbContext()
        {
            _mappingsByType = new Lazy<Dictionary<Type, MongoEntityMapping>>(() =>
            {
                return GetMappings().ToDictionary(m => m.EntityType);
            }, true);
        }

        public virtual IReadOnlyList<MongoEntityMapping> GetMappings()
        {
            return EmptyTypeList;
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
    }
}