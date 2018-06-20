using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    public class MemoryDatabase : IMemoryDatabase
    {
        private readonly ConcurrentDictionary<Type, object> _sets;

        private readonly ConcurrentDictionary<Type, InMemoryIdGenerator> _entityIdGenerators;

        public MemoryDatabase()
        {
            _sets = new ConcurrentDictionary<Type, object>();
            _entityIdGenerators = new ConcurrentDictionary<Type, InMemoryIdGenerator>();
        }

        public List<TEntity> Collection<TEntity>()
        {
            return _sets.GetOrAdd(typeof(TEntity), _ => new List<TEntity>()) as List<TEntity>;
        }

        public TKey GenerateNextId<TEntity, TKey>()
        {
            return _entityIdGenerators
                .GetOrAdd(typeof(TEntity), () => new InMemoryIdGenerator())
                .GenerateNext<TKey>();
        }
    }
}