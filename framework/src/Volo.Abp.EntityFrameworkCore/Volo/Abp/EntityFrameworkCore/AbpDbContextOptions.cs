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

    internal Dictionary<Type, List<KeyValuePair<int?, object>>> Conventions { get; }
    
    internal Dictionary<Type, List<KeyValuePair<int?, object>>> ModelBuilderActions { get; }

    public AbpDbContextOptions()
    {
        DefaultPreConfigureActions = new List<Action<AbpDbContextConfigurationContext>>();
        PreConfigureActions = new Dictionary<Type, List<object>>();
        ConfigureActions = new Dictionary<Type, object>();
        DbContextReplacements = new Dictionary<MultiTenantDbContextType, Type>();
        Conventions = new Dictionary<Type, List<KeyValuePair<int?, object>>>();
        ModelBuilderActions = new Dictionary<Type, List<KeyValuePair<int?, object>>>();
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
    
    public void ConfigureConventions([NotNull] Action<ModelConfigurationBuilder, DbContext> action, Type? dbContextType = null, int? order = null)
    {
        InternalConfigureConventions(action, dbContextType, order);
    }
    
    public void ConfigureConventions<TDbContext>([NotNull] Action<ModelConfigurationBuilder, TDbContext> action, int? order = null)
        where TDbContext : AbpDbContext<TDbContext>
    {
        InternalConfigureConventions(action, typeof(TDbContext), order);
    }

    public void OnModelCreating([NotNull] Action<ModelBuilder, DbContext> action, Type? dbContextType = null, int? order = null)
    {
        InternalOnModelCreating(action, dbContextType, order);
    }
    
    public void OnModelCreating<TDbContext>([NotNull] Action<ModelBuilder, TDbContext> action, int? order = null)
        where TDbContext : AbpDbContext<TDbContext>
    {
        InternalOnModelCreating(action, typeof(TDbContext), order);
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
    
    private void InternalConfigureConventions(object action, Type? dbContextType = null, int? order = null)
    {
        Check.NotNull(action, nameof(action));

        var actions = Conventions.GetOrDefault(dbContextType ?? typeof(AbpDbContext<>));
        if (actions == null)
        {
            Conventions[dbContextType ?? typeof(AbpDbContext<>)] = actions = new List<KeyValuePair<int?, object>>();
        }
        
        actions.Add(new KeyValuePair<int?, object>(order, action));
    }
    
    private void InternalOnModelCreating(object action, Type? dbContextType = null, int? order = null)
    {
        Check.NotNull(action, nameof(action));

        var actions = ModelBuilderActions.GetOrDefault(dbContextType ?? typeof(AbpDbContext<>));
        if (actions == null)
        {
            ModelBuilderActions[dbContextType ?? typeof(AbpDbContext<>)] = actions = new List<KeyValuePair<int?, object>>();
        }
        
        actions.Add(new KeyValuePair<int?, object>(order, action));
    }
}