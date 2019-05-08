using System;
using System.Collections.Generic;

namespace Volo.Abp.EventBus.Distributed
{
    public class EtoMappingDictionary : Dictionary<Type, Type>
    {
        public void Add<TEntity, TEntityEto>()
        {
            this[typeof(TEntity)] = typeof(TEntityEto);
        }
    }
}