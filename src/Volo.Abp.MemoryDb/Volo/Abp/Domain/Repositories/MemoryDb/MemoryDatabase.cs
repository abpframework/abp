using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    public class MemoryDatabase : IMemoryDatabase
    {
        private readonly ConcurrentDictionary<Type, object> _sets;

        private readonly object _syncObj = new object();

        public MemoryDatabase()
        {
            _sets = new ConcurrentDictionary<Type, object>();
        }

        public List<TEntity> Collection<TEntity>()
        {
            return _sets.GetOrAdd(typeof(TEntity), _ => new List<TEntity>()) as List<TEntity>;
        }
    }
}