using System;
using System.Collections.Generic;

namespace Volo.Abp.MongoDB
{
    public abstract class AbpMongoDbContext
    {
        private static readonly Type[] EmptyTypeList = new Type[0];

        public virtual IReadOnlyList<Type> GetEntityCollectionTypes()
        {
            return EmptyTypeList;
        }
    }
}