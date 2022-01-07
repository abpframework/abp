using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories.MemoryDb;

public class MemoryDatabase : IMemoryDatabase, ITransientDependency
{
    private readonly ConcurrentDictionary<Type, object> _sets;

    private readonly ConcurrentDictionary<Type, InMemoryIdGenerator> _entityIdGenerators;

    private readonly IServiceProvider _serviceProvider;

    public MemoryDatabase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _sets = new ConcurrentDictionary<Type, object>();
        _entityIdGenerators = new ConcurrentDictionary<Type, InMemoryIdGenerator>();
    }

    public IMemoryDatabaseCollection<TEntity> Collection<TEntity>()
        where TEntity : class, IEntity
    {
        return _sets.GetOrAdd(typeof(TEntity),
                _ => _serviceProvider.GetRequiredService<IMemoryDatabaseCollection<TEntity>>()) as
            IMemoryDatabaseCollection<TEntity>;
    }

    public TKey GenerateNextId<TEntity, TKey>()
    {
        return _entityIdGenerators
            .GetOrAdd(typeof(TEntity), () => new InMemoryIdGenerator())
            .GenerateNext<TKey>();
    }
}
