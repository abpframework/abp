using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.PermissionManagement;

//TODO: Extract interface
public class DynamicPermissionDefinitionStoreCache : ISingletonDependency
{
    public string CacheStamp { get; set; }
    
    protected IDictionary<string, PermissionGroupDefinition> PermissionGroupDefinitions { get; }

    protected IDictionary<string, PermissionDefinition> PermissionDefinitions { get; }
    protected ISimpleStateCheckerSerializer StateCheckerSerializer { get; }

    public DynamicPermissionDefinitionStoreCache(ISimpleStateCheckerSerializer stateCheckerSerializer)
    {
        StateCheckerSerializer = stateCheckerSerializer;
        PermissionGroupDefinitions = new Dictionary<string, PermissionGroupDefinition>();
        PermissionDefinitions = new Dictionary<string, PermissionDefinition>();
    }

    public Task FillAsync(
        List<PermissionGroupDefinitionRecord> permissionGroupRecords,
        List<PermissionDefinitionRecord> permissionRecords)
    {
        var context = new PermissionDefinitionContext(null);
        
        foreach (var permissionGroupRecord in permissionGroupRecords)
        {
            var permissionGroup = context.AddGroup(
                permissionGroupRecord.Name,
                new FixedLocalizableString(permissionGroupRecord.DisplayName) //TODO: Consider localization
            );

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

    protected virtual Dictionary<string, PermissionDefinition> CreatePermissionDefinitions()
    {
        var permissions = new Dictionary<string, PermissionDefinition>();

        foreach (var groupDefinition in PermissionGroupDefinitions.Values)
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