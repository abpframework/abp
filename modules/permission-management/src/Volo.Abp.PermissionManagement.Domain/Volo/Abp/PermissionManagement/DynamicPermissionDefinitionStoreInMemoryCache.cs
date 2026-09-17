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
    protected IList<PermissionDefinition> ResourcePermissionDefinitions { get; }
    protected ISimpleStateCheckerSerializer StateCheckerSerializer { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public SemaphoreSlim SyncSemaphore { get; } = new(1, 1);

    public DateTime? LastCheckTime { get; set; }

    public DynamicPermissionDefinitionStoreInMemoryCache(
        ISimpleStateCheckerSerializer stateCheckerSerializer,
        ILocalizableStringSerializer localizableStringSerializer)
    {
        StateCheckerSerializer = stateCheckerSerializer;
        LocalizableStringSerializer = localizableStringSerializer;

        PermissionGroupDefinitions = new Dictionary<string, PermissionGroupDefinition>();
        PermissionDefinitions = new Dictionary<string, PermissionDefinition>();
        ResourcePermissionDefinitions = new List<PermissionDefinition>();
    }

    public Task FillAsync(
        List<PermissionGroupDefinitionRecord> permissionGroupRecords,
        List<PermissionDefinitionRecord> permissionRecords)
    {
        PermissionGroupDefinitions.Clear();
        PermissionDefinitions.Clear();
        ResourcePermissionDefinitions.Clear();

        var context = new PermissionDefinitionContext(null);

        var resourcePermissionRecords = permissionRecords.Where(x => !x.ResourceName.IsNullOrWhiteSpace());
        foreach (var resourcePermissionRecord in resourcePermissionRecords)
        {
            var resourcePermission = context.AddResourcePermission(resourcePermissionRecord.Name,
                resourcePermissionRecord.ResourceName,
                resourcePermissionRecord.ManagementPermissionName,
                resourcePermissionRecord.DisplayName != null ? LocalizableStringSerializer.Deserialize(resourcePermissionRecord.DisplayName) : null,
                resourcePermissionRecord.MultiTenancySide,
                resourcePermissionRecord.IsEnabled);

            ApplyPermissionProperties(resourcePermission, resourcePermissionRecord);

            ResourcePermissionDefinitions.Add(resourcePermission);
        }

        var permissions = permissionRecords.Where(x => x.ResourceName.IsNullOrWhiteSpace()).ToList();
        foreach (var permissionGroupRecord in permissionGroupRecords)
        {
            var permissionGroup = context.AddGroup(
                permissionGroupRecord.Name,
                permissionGroupRecord.DisplayName != null ? LocalizableStringSerializer.Deserialize(permissionGroupRecord.DisplayName) : null
            );

            PermissionGroupDefinitions[permissionGroup.Name] = permissionGroup;

            foreach (var property in permissionGroupRecord.ExtraProperties)
            {
                permissionGroup[property.Key] = property.Value;
            }

            var permissionRecordsInThisGroup = permissions
                .Where(p => p.GroupName == permissionGroup.Name);

            foreach (var permissionRecord in permissionRecordsInThisGroup.Where(x => x.ParentName == null))
            {
                AddPermissionRecursively(permissionGroup, permissionRecord, permissions);
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

    public PermissionDefinition GetResourcePermissionOrNull(string resourceName, string name)
    {
        return ResourcePermissionDefinitions.FirstOrDefault(p => p.ResourceName == resourceName && p.Name == name);
    }

    public IReadOnlyList<PermissionDefinition> GetResourcePermissions()
    {
        return ResourcePermissionDefinitions.ToList();
    }

    private void AddPermissionRecursively(ICanAddChildPermission permissionContainer,
        PermissionDefinitionRecord permissionRecord,
        List<PermissionDefinitionRecord> allPermissionRecords)
    {
        var permission = permissionContainer.AddPermission(
            permissionRecord.Name,
            permissionRecord.DisplayName != null ? LocalizableStringSerializer.Deserialize(permissionRecord.DisplayName) : null,
            permissionRecord.MultiTenancySide,
            permissionRecord.IsEnabled
        );

        PermissionDefinitions[permission.Name] = permission;

        ApplyPermissionProperties(permission, permissionRecord);

        foreach (var subPermission in allPermissionRecords.Where(p => p.ParentName == permissionRecord.Name))
        {
            AddPermissionRecursively(permission, subPermission, allPermissionRecords);
        }
    }

    private void ApplyPermissionProperties(PermissionDefinition permission, PermissionDefinitionRecord permissionRecord)
    {
        if (!permissionRecord.Providers.IsNullOrWhiteSpace())
        {
            permission.Providers.AddRange(permissionRecord.Providers.Split(','));
        }

        if (!permissionRecord.StateCheckers.IsNullOrWhiteSpace())
        {
            var checkers = StateCheckerSerializer
                .DeserializeArray(
                    permissionRecord.StateCheckers,
                    permission
                );
            permission.StateCheckers.AddRange(checkers);
        }

        foreach (var property in permissionRecord.ExtraProperties)
        {
            permission[property.Key] = property.Value;
        }
    }
}
