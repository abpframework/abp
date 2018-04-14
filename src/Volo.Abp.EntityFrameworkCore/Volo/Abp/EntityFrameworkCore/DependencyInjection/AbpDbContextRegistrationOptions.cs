using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.DependencyInjection
{
    public class AbpDbContextRegistrationOptions : CommonDbContextRegistrationOptions, IAbpDbContextRegistrationOptionsBuilder
    {
        public Dictionary<Type, object> EntityOptions { get; }

        public AbpDbContextRegistrationOptions(Type originalDbContextType, IServiceCollection services)
            : base(originalDbContextType, services)
        {
            EntityOptions = new Dictionary<Type, object>();
        }

        public void Entity<TEntity>(Action<EntityOptions<TEntity>> optionsAction) where TEntity : IEntity
        {
            Services.Configure<EntityOptions>(options =>
            {
                options.Entity(optionsAction);
            });
        }
    }
}