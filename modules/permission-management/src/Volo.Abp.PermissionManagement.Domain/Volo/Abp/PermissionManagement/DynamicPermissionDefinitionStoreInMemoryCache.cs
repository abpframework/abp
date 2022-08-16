using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.PermissionManagement;

public class DynamicPermissionDefinitionStoreInMemoryCache : 
    IDynamicPermissionDefinitionStoreInMemoryCache,
    ISingletonDependency
{
    public string CacheStamp { get; set; }
    
    protected IDictionary<string, PermissionGroupDefinition> PermissionGroupDefinitions { get; }
    protected IDictionary<string, PermissionDefinition> PermissionDefinitions { get; }
    protected ISimpleStateCheckerSerializer StateCheckerSerializer { get; }

    public DynamicPermissionDefinitionStoreInMemoryCache(ISimpleStateCheckerSerializer stateCheckerSerializer)
    {
        StateCheckerSerializer = stateCheckerSerializer;
        PermissionGroupDefinitions = new Dictionary<string, PermissionGroupDefinition>();
        PermissionDefinitions = new Dictionary<string, PermissionDefinition>();
    }

    public SemaphoreSlim SyncSemaphore { get; } = new(1, 1);

    public Task FillAsync(
        List<PermissionGroupDefinitionRecord> permissionGroupRecords,
        List<PermissionDefinitionRecord> permissionRecords)
    {
        PermissionGroupDefinitions.Clear();
        PermissionDefinitions.Clear();
        
        var context = new PermissionDefinitionContext(null);
        
        foreach (var permissionGroupRecord in permissionGroupRecords)
        {
            var permissionGroup = context.AddGroup(
                permissionGroupRecord.Name,
                new FixedLocalizableString(permissionGroupRecord.DisplayName) //TODO: Consider localization
            );
            
            PermissionGroupDefinitions[permissionGroup.Name] = permissionGroup;

            foreach (var property in permissionGroupRecord.ExtraProperties)
            {
                permissionGroup[property.Key] = property.Value;
            }

            var permissionRecordsInThisGroup = permissionRecords
                .Where(p => p.GroupName == permissionGroup.Name);
            
            foreach (var permissionRecord in permissionRecordsInThisGroup)
            {
                AddPermissionRecursively(permissionGroup, permissionRecord, permissionRecords);
            }
        }

        return Task.CompletedTask;
    }

    public PermissionDefinition GetPermissionOrNull(string name)
    {
        return PermissionDefinitions.GetOrDefault(name);
    }

    public IReadOnlyList<PermissionDefinition> GetPermissions()
    {
        return PermissionDefinitions.Values.ToList();
    }

    public IReadOnlyList<PermissionGroupDefinition> GetGroups()
    {
        return PermissionGroupDefinitions.Values.ToList();
    }

    private void AddPermissionRecursively(ICanAddChildPermission permissionContainer,
        PermissionDefinitionRecord permissionRecord,
        List<PermissionDefinitionRecord> allPermissionRecords)
    {
        var permission = permissionContainer.AddPermission(
            permissionRecord.Name,
            new FixedLocalizableString(permissionRecord.DisplayName),
            permissionRecord.MultiTenancySide,
            permissionRecord.IsEnabled
        );
        
        PermissionDefinitions[permission.Name] = permission;

        if (!permissionRecord.Providers.IsNullOrWhiteSpace())
        {
            permission.Providers.AddRange(permissionRecord.Providers.Split(','));
        }

        if (!permissionRecord.StateCheckers.IsNullOrWhiteSpace())
        {
            var checkers = StateCheckerSerializer
                .DeserializeArray<PermissionDefinition>(
                    permissionRecord.StateCheckers
                );
            permission.StateCheckers.AddRange(checkers);
        }

        foreach (var property in permissionRecord.ExtraProperties)
        {
            permission[property.Key] = property.Value;
        }

        foreach (var subPermission in allPermissionRecords.Where(p => p.ParentName == permissionRecord.Name))
        {
            AddPermissionRecursively(permission, subPermission, allPermissionRecords);
        }
    }
}