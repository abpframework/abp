using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    //public interface IMemoryDbRepository<TEntity> : IMemoryDbRepository<TEntity, Guid>, IQueryableRepository<TEntity>
    //    where TEntity : class, IEntity<Guid>
    //{
        
    //}

    public interface IMemoryDbRepository<TEntity, TPrimaryKey> : IQueryableRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        IMemoryDatabase Database { get; }

        List<TEntity> Collection { get; }
    }
}
