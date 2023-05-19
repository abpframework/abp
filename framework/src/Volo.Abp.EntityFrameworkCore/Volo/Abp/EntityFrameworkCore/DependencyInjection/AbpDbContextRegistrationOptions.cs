using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.DependencyInjection;

public class AbpDbContextRegistrationOptions : AbpCommonDbContextRegistrationOptions, IAbpDbContextRegistrationOptionsBuilder
{
    public Dictionary<Type, object> AbpEntityOptions { get; }

    public AbpDbContextRegistrationOptions(Type originalDbContextType, IServiceCollection services)
        : base(originalDbContextType, services)
    {
        AbpEntityOptions = new Dictionary<Type, object>();
    }

    public void Entity<TEntity>(Action<AbpEntityOptions<TEntity>> optionsAction) where TEntity : IEntity
    {
        Services.Configure<AbpEntityOptions>(options =>
        {
            options.Entity(optionsAction);
        });
    }
}
