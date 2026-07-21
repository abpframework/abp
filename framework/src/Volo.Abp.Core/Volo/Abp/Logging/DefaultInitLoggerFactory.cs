using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Logging;

public class DefaultInitLoggerFactory : IInitLoggerFactory
{
    private readonly Dictionary<Type, object> _cache = [];
    private readonly List<List<AbpInitLogEntry>> _entryLists = [];

    public virtual IInitLogger<T> Create<T>()
    {
        return (IInitLogger<T>)_cache.GetOrAdd(typeof(T), () =>
        {
            var logger = new DefaultInitLogger<T>();
            _entryLists.Add(logger.Entries);
            return logger;
        });
    }

    public virtual List<AbpInitLogEntry> GetAllEntries()
    {
        return _entryLists.SelectMany(l => l).ToList();
    }

    public virtual void ClearAllEntries()
    {
        foreach (var list in _entryLists)
        {
            list.Clear();
        }
    }
}
