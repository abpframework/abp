using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

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

        public IMemoryDatabaseCollection<TEntity> Collection<TEntity>()
            where TEntity : class, IEntity
        {
            return _sets.GetOrAdd(typeof(TEntity),
                    _ => new MemoryDatabaseCollection<TEntity>(new MemoryDbBinarySerializer())) as
                MemoryDatabaseCollection<TEntity>;
        }

        public TKey GenerateNextId<TEntity, TKey>()
        {
            return _entityIdGenerators
                .GetOrAdd(typeof(TEntity), () => new InMemoryIdGenerator())
                .GenerateNext<TKey>();
        }
    }
}