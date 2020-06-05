using System;
using System.Collections;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    public class MemoryDatabaseCollection<TEntity> : IMemoryDatabaseCollection<TEntity>
        where TEntity : class, IEntity
    {
        private readonly Dictionary<string, byte[]> _dictionary = new Dictionary<string, byte[]>();

        private readonly IMemoryDbSerializer _memoryDbSerializer;

        public MemoryDatabaseCollection(IMemoryDbSerializer memoryDbSerializer)
        {
            _memoryDbSerializer = memoryDbSerializer;
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            foreach (var entity in _dictionary.Values)
            {
                yield return _memoryDbSerializer.Deserialize(entity, typeof(TEntity)).As<TEntity>();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TEntity entity)
        {
            _dictionary.Add(GetEntityKey(entity), _memoryDbSerializer.Serialize(entity));
        }

        public void Update(TEntity entity)
        {
            if (_dictionary.ContainsKey(GetEntityKey(entity)))
            {
                _dictionary[GetEntityKey(entity)] = _memoryDbSerializer.Serialize(entity);
            }
        }

        public void Remove(TEntity entity)
        {
            _dictionary.Remove(GetEntityKey(entity));
        }

        private string GetEntityKey(TEntity entity)
        {
            return entity.GetKeys().JoinAsString(",");
        }
    }
}