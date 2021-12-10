using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.DependencyInjection;

public class AbpEntityOptions<TEntity>
    where TEntity : IEntity
{
    public static AbpEntityOptions<TEntity> Empty { get; } = new AbpEntityOptions<TEntity>();

    public Func<IQueryable<TEntity>, IQueryable<TEntity>> DefaultWithDetailsFunc { get; set; }
}

public class AbpEntityOptions
{
    private readonly IDictionary<Type, object> _options;

    public AbpEntityOptions()
    {
        _options = new Dictionary<Type, object>();
    }

    public AbpEntityOptions<TEntity> GetOrNull<TEntity>()
        where TEntity : IEntity
    {
        return _options.GetOrDefault(typeof(TEntity)) as AbpEntityOptions<TEntity>;
    }

    public void Entity<TEntity>([NotNull] Action<AbpEntityOptions<TEntity>> optionsAction)
        where TEntity : IEntity
    {
        Check.NotNull(optionsAction, nameof(optionsAction));

        optionsAction(
            _options.GetOrAdd(
                typeof(TEntity),
                () => new AbpEntityOptions<TEntity>()
            ) as AbpEntityOptions<TEntity>
        );
    }
}
