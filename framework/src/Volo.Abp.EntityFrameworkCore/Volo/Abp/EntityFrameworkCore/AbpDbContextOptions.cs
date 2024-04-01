using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.EntityFrameworkCore;

public class AbpDbContextOptions
{
    internal List<Action<AbpDbContextConfigurationContext>> DefaultPreConfigureActions { get; }

    internal Action<AbpDbContextConfigurationContext>? DefaultConfigureAction { get; set; }

    internal Dictionary<Type, List<object>> PreConfigureActions { get; }

    internal Dictionary<Type, object> ConfigureActions { get; }

    internal Dictionary<MultiTenantDbContextType, Type> DbContextReplacements { get; }

    internal Action<DbContext, ModelConfigurationBuilder>? DefaultConventionAction { get; set; }

    internal Dictionary<Type, List<object>> ConventionActions { get; }

    internal Action<DbContext, ModelBuilder>? DefaultOnModelCreatingAction { get; set; }

    internal Dictionary<Type, List<object>> OnModelCreatingActions { get; }

    public AbpDbContextOptions()
    {
        DefaultPreConfigureActions = new List<Action<AbpDbContextConfigurationContext>>();
        PreConfigureActions = new Dictionary<Type, List<object>>();
        ConfigureActions = new Dictionary<Type, object>();
        DbContextReplacements = new Dictionary<MultiTenantDbContextType, Type>();
        ConventionActions = new Dictionary<Type, List<object>>();
        OnModelCreatingActions = new Dictionary<Type, List<object>>();
    }

    public void PreConfigure([NotNull] Action<AbpDbContextConfigurationContext> action)
    {
        Check.NotNull(action, nameof(action));

        DefaultPreConfigureActions.Add(action);
    }

    public void Configure([NotNull] Action<AbpDbContextConfigurationContext> action)
    {
        Check.NotNull(action, nameof(action));

        DefaultConfigureAction = action;
    }

    public void ConfigureDefaultConvention([NotNull] Action<DbContext, ModelConfigurationBuilder> action)
    {
        Check.NotNull(action, nameof(action));

        DefaultConventionAction = action;
    }

    public void ConfigureConventions<TDbContext>([NotNull] Action<TDbContext, ModelConfigurationBuilder> action)
        where TDbContext : AbpDbContext<TDbContext>
    {
        Check.NotNull(action, nameof(action));

        var actions = ConventionActions.GetOrDefault(typeof(TDbContext));
        if (actions == null)
        {
            ConventionActions[typeof(TDbContext)] = new List<object>
            {
                new Action<DbContext, ModelConfigurationBuilder>((dbContext, builder) => action((TDbContext)dbContext, builder))
            };
            return;
        }

        actions.Add(action);
    }

    public void ConfigureDefaultOnModelCreating([NotNull] Action<DbContext, ModelBuilder> action)
    {
        Check.NotNull(action, nameof(action));

        DefaultOnModelCreatingAction = action;
    }

    public void ConfigureOnModelCreating<TDbContext>([NotNull] Action<TDbContext, ModelBuilder> action)
        where TDbContext : AbpDbContext<TDbContext>
    {
        Check.NotNull(action, nameof(action));

        var actions = OnModelCreatingActions.GetOrDefault(typeof(TDbContext));
        if (actions == null)
        {
            OnModelCreatingActions[typeof(TDbContext)] = new List<object>
            {
                new Action<DbContext, ModelBuilder>((dbContext, builder) => action((TDbContext)dbContext, builder))
            };
            return;
        }

        actions.Add(action);
    }

    public bool IsConfiguredDefault()
    {
        return DefaultConfigureAction != null;
    }

    public void PreConfigure<TDbContext>([NotNull] Action<AbpDbContextConfigurationContext<TDbContext>> action)
        where TDbContext : AbpDbContext<TDbContext>
    {
        Check.NotNull(action, nameof(action));

        var actions = PreConfigureActions.GetOrDefault(typeof(TDbContext));
        if (actions == null)
        {
            PreConfigureActions[typeof(TDbContext)] = actions = new List<object>();
        }

        actions.Add(action);
    }

    public void Configure<TDbContext>([NotNull] Action<AbpDbContextConfigurationContext<TDbContext>> action)
        where TDbContext : AbpDbContext<TDbContext>
    {
        Check.NotNull(action, nameof(action));

        ConfigureActions[typeof(TDbContext)] = action;
    }

    public bool IsConfigured<TDbContext>()
    {
        return IsConfigured(typeof(TDbContext));
    }

    public bool IsConfigured(Type dbContextType)
    {
        return ConfigureActions.ContainsKey(dbContextType);
    }

    internal Type GetReplacedTypeOrSelf(Type dbContextType, MultiTenancySides multiTenancySides = MultiTenancySides.Both)
    {
        var replacementType = dbContextType;
        while (true)
        {
            var foundType = DbContextReplacements.LastOrDefault(x => x.Key.Type == replacementType && x.Key.MultiTenancySide.HasFlag(multiTenancySides));
            if (!foundType.Equals(default(KeyValuePair<MultiTenantDbContextType, Type>)))
            {
                if (foundType.Value == dbContextType)
                {
                    throw new AbpException(
                        "Circular DbContext replacement found for " +
                        dbContextType.AssemblyQualifiedName
                    );
                }
                replacementType = foundType.Value;
            }
            else
            {
                return replacementType;
            }
        }
    }
}
