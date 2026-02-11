using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.StaticDefinitions;

namespace Volo.Abp.Authorization.Permissions;

public class StaticPermissionDefinitionStore : IStaticPermissionDefinitionStore, ISingletonDependency
{
    protected IServiceProvider ServiceProvider { get; }
    protected AbpPermissionOptions Options { get; }
    protected IStaticDefinitionCache<PermissionGroupDefinition, (Dictionary<string, PermissionGroupDefinition>, List<PermissionDefinition>)> GroupCache { get; }
    protected IStaticDefinitionCache<PermissionDefinition, Dictionary<string, PermissionDefinition>> DefinitionCache { get; }

    public StaticPermissionDefinitionStore(
        IServiceProvider serviceProvider,
        IOptions<AbpPermissionOptions> options,
        IStaticDefinitionCache<PermissionGroupDefinition, (Dictionary<string, PermissionGroupDefinition>, List<PermissionDefinition>)> groupCache,
        IStaticDefinitionCache<PermissionDefinition, Dictionary<string, PermissionDefinition>> definitionCache)
    {
        ServiceProvider = serviceProvider;
        Options = options.Value;
        GroupCache = groupCache;
        DefinitionCache = definitionCache;
    }

    public async Task<PermissionDefinition?> GetOrNullAsync(string name)
    {
        var defs = await GetPermissionDefinitionsAsync();
        return defs.GetOrDefault(name);
    }

    public virtual async Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync()
    {
        var defs = await GetPermissionDefinitionsAsync();
        return defs.Values.ToImmutableList();
    }

    public virtual async Task<PermissionDefinition?> GetResourcePermissionOrNullAsync(string resourceName, string name)
    {
        var (_, resourcePermissions) = await GetPermissionGroupDefinitionsAsync();
        return resourcePermissions.FirstOrDefault(p => p.ResourceName == resourceName && p.Name == name);
    }

    public virtual async Task<IReadOnlyList<PermissionDefinition>> GetResourcePermissionsAsync()
    {
        var (_, resourcePermissions) = await GetPermissionGroupDefinitionsAsync();
        return resourcePermissions.ToImmutableList();
    }

    public async Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync()
    {
        var (groups, _) = await GetPermissionGroupDefinitionsAsync();
        return groups.Values.ToImmutableList();
    }

    protected virtual async Task<(Dictionary<string, PermissionGroupDefinition>, List<PermissionDefinition>)> GetPermissionGroupDefinitionsAsync()
    {
        return await GroupCache.GetOrCreateAsync(CreatePermissionGroupDefinitionsAsync);
    }

    protected virtual async Task<Dictionary<string, PermissionDefinition>> GetPermissionDefinitionsAsync()
    {
        return await DefinitionCache.GetOrCreateAsync(CreatePermissionDefinitionsAsync);
    }

    protected virtual Task<(Dictionary<string, PermissionGroupDefinition>, List<PermissionDefinition>)> CreatePermissionGroupDefinitionsAsync()
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            var context = new PermissionDefinitionContext(scope.ServiceProvider);

            var providers = Options
                .DefinitionProviders
                .Select(p => (scope.ServiceProvider.GetRequiredService(p) as IPermissionDefinitionProvider)!)
                .ToList();

            foreach (var provider in providers)
            {
                context.CurrentProvider = provider;
                provider.PreDefine(context);
            }

            foreach (var provider in providers)
            {
                context.CurrentProvider = provider;
                provider.Define(context);
            }

            foreach (var provider in providers)
            {
                context.CurrentProvider = provider;
                provider.PostDefine(context);
            }

            context.CurrentProvider = null;

            return Task.FromResult((context.Groups, context.ResourcePermissions));
        }
    }

    protected virtual async Task<Dictionary<string, PermissionDefinition>> CreatePermissionDefinitionsAsync()
    {
        var permissions = new Dictionary<string, PermissionDefinition>();

        var (groups, _) = await GetPermissionGroupDefinitionsAsync();
        foreach (var groupDefinition in groups.Values)
        {
            foreach (var permission in groupDefinition.Permissions)
            {
                AddPermissionToDictionaryRecursively(permissions, permission);
            }
        }

        return permissions;
    }

    protected virtual void AddPermissionToDictionaryRecursively(
        Dictionary<string, PermissionDefinition> permissions,
        PermissionDefinition permission)
    {
        if (permissions.ContainsKey(permission.Name))
        {
            throw new AbpException("Duplicate permission name: " + permission.Name);
        }

        permissions[permission.Name] = permission;

        foreach (var child in permission.Children)
        {
            AddPermissionToDictionaryRecursively(permissions, child);
        }
    }
}
