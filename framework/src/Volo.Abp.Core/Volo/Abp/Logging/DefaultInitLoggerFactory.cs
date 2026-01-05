using System;
using System.Collections.Generic;

namespace Volo.Abp.Logging;

public class DefaultInitLoggerFactory : IInitLoggerFactory
{
    private readonly Dictionary<Type, object> _cache = [];

    public virtual IInitLogger<T> Create<T>()
    {
        return (IInitLogger<T>)_cache.GetOrAdd(typeof(T), () => new DefaultInitLogger<T>());
    }
}
