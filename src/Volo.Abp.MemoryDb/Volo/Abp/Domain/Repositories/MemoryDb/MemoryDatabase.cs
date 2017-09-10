using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    public class MemoryDatabase : IMemoryDatabase
    {
        private readonly ConcurrentDictionary<Type, object> _sets;

        private readonly ConcurrentDictionary<Type, InMemoryIdGenerator> _idGenerators;

        public MemoryDatabase()
        {
            _sets = new ConcurrentDictionary<Type, object>();
            _idGenerators = new ConcurrentDictionary<Type, InMemoryIdGenerator>();
        }

        public List<TEntity> Collection<TEntity>()
        {
            return _sets.GetOrAdd(typeof(TEntity), _ => new List<TEntity>()) as List<TEntity>;
        }

        public TPrimaryKey GenerateNextId<TEntity, TPrimaryKey>()
        {
            return _idGenerators
                .GetOrAdd(typeof(TEntity), () => new InMemoryIdGenerator())
                .GenerateNext<TPrimaryKey>();
        }
    }
}