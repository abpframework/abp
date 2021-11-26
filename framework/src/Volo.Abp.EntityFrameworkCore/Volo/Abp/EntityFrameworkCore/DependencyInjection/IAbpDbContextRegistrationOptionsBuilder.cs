using System;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.DependencyInjection;

public interface IAbpDbContextRegistrationOptionsBuilder : IAbpCommonDbContextRegistrationOptionsBuilder
{
    void Entity<TEntity>([NotNull] Action<AbpEntityOptions<TEntity>> optionsAction)
        where TEntity : IEntity;
}
