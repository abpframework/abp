using System;
using System.Collections.Generic;

namespace Volo.Abp.MongoDB
{
    public abstract class AbpMongoDbContext
    {
        //TODO: Array of an EntityInfo object which contains EntityType and CollectionName
        private static readonly Type[] EmptyTypeList = new Type[0];

        public virtual IReadOnlyList<Type> GetEntityCollectionTypes()
        {
            return EmptyTypeList;
        }
    }
}