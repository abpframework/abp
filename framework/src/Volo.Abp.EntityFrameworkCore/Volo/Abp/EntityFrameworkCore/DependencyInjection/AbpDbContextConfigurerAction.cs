using System;
using JetBrains.Annotations;

namespace Volo.Abp.EntityFrameworkCore.DependencyInjection
{
    public class AbpDbContextConfigurerAction : IAbpDbContextConfigurer
    {
        [NotNull]
        public Action<AbpDbContextConfigurationContext> Action { get; }

        public AbpDbContextConfigurerAction([NotNull] Action<AbpDbContextConfigurationContext> action)
        {
            Check.NotNull(action, nameof(action));

            Action = action;
        }

        public void Configure(AbpDbContextConfigurationContext context)
        {
            Action.Invoke(context);
        }
    }

    public class AbpDbContextConfigurerAction<TDbContext> : AbpDbContextConfigurerAction
        where TDbContext : AbpDbContext<TDbContext>
    {
        public AbpDbContextConfigurerAction([NotNull] Action<AbpDbContextConfigurationContext> action) 
            : base(action)
        {
        }
    }
}