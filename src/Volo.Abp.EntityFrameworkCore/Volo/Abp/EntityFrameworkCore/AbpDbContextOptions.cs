using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore
{
    public class AbpDbContextOptions
    {
        internal Action<AbpDbContextConfigurationContext> DefaultConfigureAction { get; set; }

        internal Dictionary<Type, object> ConfigureActions { get; set; }

        public AbpDbContextOptions()
        {
            ConfigureActions = new Dictionary<Type, object>();
        }

        public void Configure([NotNull] Action<AbpDbContextConfigurationContext> action)
        {
            Check.NotNull(action, nameof(action));

            DefaultConfigureAction = action;
        }

        public void Configure<TDbContext>([NotNull] Action<AbpDbContextConfigurationContext<TDbContext>> action) 
            where TDbContext : AbpDbContext<TDbContext>
        {
            Check.NotNull(action, nameof(action));

            ConfigureActions[typeof(TDbContext)] = action;
        }
    }
}