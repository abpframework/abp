using System;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.DependencyInjection
{
    public interface IAbpDbContextRegistrationOptionsBuilder : ICommonDbContextRegistrationOptionsBuilder
    {
        void Entity<TEntity>([NotNull] Action<EntityOptions<TEntity>> optionsAction)
            where TEntity : IEntity;
    }
}