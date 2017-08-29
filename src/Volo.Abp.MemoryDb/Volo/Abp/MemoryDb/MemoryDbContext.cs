using System;
using System.Collections.Generic;

namespace Volo.Abp.MemoryDb
{
    public abstract class MemoryDbContext
    {
        private static readonly Type[] EmptyTypeList = new Type[0];

        public virtual IReadOnlyList<Type> GetEntityTypes()
        {
            return EmptyTypeList;
        }
    }
}