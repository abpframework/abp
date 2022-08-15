using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.PermissionManagement;

public class PermissionDefinitionSerializer : IPermissionDefinitionSerializer, ITransientDependency
{
    protected ISimpleStateCheckerSerializer StateCheckerSerializer { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IStringLocalizerFactory StringLocalizerFactory { get; }

    public PermissionDefinitionSerializer(
        IGuidGenerator guidGenerator,
        IStringLocalizerFactory stringLocalizerFactory,
        ISimpleStateCheckerSerializer stateCheckerSerializer)
    {
        StateCheckerSerializer = stateCheckerSerializer;
        GuidGenerator = guidGenerator;
        StringLocalizerFactory = stringLocalizerFactory;
    }
    
    public Task<PermissionDefinitionRecord> SerializeAsync(
        PermissionDefinition permission,
        PermissionGroupDefinition permissionGroup)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var permissionRecord = new PermissionDefinitionRecord(
                GuidGenerator.Create(),
                permissionGroup?.Name,
                permission.Name,
                permission.Parent?.Name,
                permission.DisplayName.Localize(StringLocalizerFactory),
                permission.IsEnabled,
                permission.MultiTenancySide,
                SerializeProviders(permission.Providers),
                SerializeStateCheckers(permission.StateCheckers)
            );

            foreach (var property in permission.Properties)
            {
                permissionRecord.SetProperty(property.Key, property.Value);
            }
            
            return Task.FromResult(permissionRecord);
        }
    }

    public Task<PermissionDefinition> DeserializeAsync(
        PermissionDefinitionRecord permissionRecord)
    {
        throw new System.NotImplementedException();
    }

    public Task<PermissionGroupDefinitionRecord> SerializeAsync(PermissionGroupDefinition permissionGroup)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var permissionGroupRecord = new PermissionGroupDefinitionRecord(
                GuidGenerator.Create(),
                permissionGroup.Name,
                permissionGroup.DisplayName.Localize(StringLocalizerFactory)
            );

            foreach (var property in permissionGroup.Properties)
            {
                permissionGroupRecord.SetProperty(property.Key, property.Value);
            }
            
            return Task.FromResult(permissionGroupRecord);
        }
    }

    public async Task<(PermissionGroupDefinitionRecord[], PermissionDefinitionRecord[])> 
        SerializeAsync(IEnumerable<PermissionGroupDefinition> permissionGroups)
    {
        var permissionGroupRecords = new List<PermissionGroupDefinitionRecord>();
        var permissionRecords = new List<PermissionDefinitionRecord>();
        
        foreach (var permissionGroup in permissionGroups)
        {
            permissionGroupRecords.Add(await SerializeAsync(permissionGroup));
            
            foreach (var permission in permissionGroup.GetPermissionsWithChildren())
            {
                permissionRecords.Add(await SerializeAsync(permission, permissionGroup));
            }
        }

        return (permissionGroupRecords.ToArray(), permissionRecords.ToArray());
    }

    public Task<PermissionGroupDefinition> DeserializeAsync(PermissionGroupDefinitionRecord permissionGroupRecord)
    {
        throw new System.NotImplementedException();
    }
    
    protected virtual string SerializeStateCheckers(List<ISimpleStateChecker<PermissionDefinition>> stateCheckers)
    {
        return StateCheckerSerializer.Serialize(stateCheckers);
    }

    protected virtual string SerializeProviders(ICollection<string> providers)
    {
        return providers.Any()
            ? providers.JoinAsString(",")
            : null;
    }
}