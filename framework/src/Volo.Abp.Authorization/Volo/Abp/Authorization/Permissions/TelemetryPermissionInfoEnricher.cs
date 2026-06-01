using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal.Telemetry.Activity;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Volo.Abp.Internal.Telemetry.Activity.Providers;
using Volo.Abp.Internal.Telemetry.Constants;

namespace Volo.Abp.Authorization.Permissions;

[ExposeServices(typeof(ITelemetryActivityEventEnricher), typeof(IHasParentTelemetryActivityEventEnricher<TelemetryApplicationInfoEnricher>))]
public sealed class TelemetryPermissionInfoEnricher : TelemetryActivityEventEnricher, IHasParentTelemetryActivityEventEnricher<TelemetryApplicationInfoEnricher>
{
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;

    public TelemetryPermissionInfoEnricher(IPermissionDefinitionManager permissionDefinitionManager,
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _permissionDefinitionManager = permissionDefinitionManager;
    }

    protected override Task<bool> CanExecuteAsync(ActivityContext context)
    {
        return Task.FromResult(context.ProjectId.HasValue);
    }

    protected async override Task ExecuteAsync(ActivityContext context)
    {
        var permissions = await _permissionDefinitionManager.GetPermissionsAsync();

        var userDefinedPermissionsCount = permissions.Count(IsUserDefinedPermission);
        
        context.Current[ActivityPropertyNames.PermissionCount] = userDefinedPermissionsCount;
    }
    
    private static bool IsUserDefinedPermission(PermissionDefinition permission)
    {
        return permission.Properties.TryGetValue(PermissionDefinitionContext.KnownPropertyNames.CurrentProviderName, out var providerName) &&
               providerName is string &&
               !providerName.ToString()!.StartsWith(TelemetryConsts.VoloNameSpaceFilter);
    }


}