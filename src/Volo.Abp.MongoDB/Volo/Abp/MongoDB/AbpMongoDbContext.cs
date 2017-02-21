using System;
using System.Collections.Generic;
using System.Linq;
using Volo.ExtensionMethods.Collections.Generic;

namespace Volo.Abp.MongoDB
{
    public abstract class AbpMongoDbContext
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

        public string GetCollectionName<T>()
        {
            return GetMapping<T>().CollectionName;
        }

        private MongoEntityMapping GetMapping<T>()
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