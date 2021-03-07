using System;
using System.Collections.Generic;

namespace Volo.Abp.Logging
{
    public class InitLoggerFactory : IInitLoggerFactory
    {
        private readonly Dictionary<Type, object> _cache = new Dictionary<Type, object>();

        public virtual IInitLogger<T> Create<T>()
        {
            return (IInitLogger<T>)_cache.GetOrAdd(typeof(T), () => new DefaultInitLogger<T>());;
        }
    }
}
