using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Authorization.Permissions.Resources;

public class ClientResourcePermissionValueProvider : ResourcePermissionValueProvider
{
    public const string ProviderName = "C";

    public override string Name => ProviderName;

    protected ICurrentTenant CurrentTenant { get; }

    public ClientResourcePermissionValueProvider(IResourcePermissionStore resourcePermissionStore, ICurrentTenant currentTenant)
        : base(resourcePermissionStore)
    {
        CurrentTenant = currentTenant;
    }

    public override async Task<PermissionGrantResult> CheckAsync(ResourcePermissionValueCheckContext context)
    {
        var clientId = context.Principal?.FindFirst(AbpClaimTypes.ClientId)?.Value;

        if (clientId == null)
        {
            return PermissionGrantResult.Undefined;
        }

        using (CurrentTenant.Change(null))
        {
            return await ResourcePermissionStore.IsGrantedAsync(context.Permission.Name, context.ResourceName, context.ResourceKey, Name, clientId)
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Undefined;
        }
    }

    public override async Task<MultiplePermissionGrantResult> CheckAsync(ResourcePermissionValuesCheckContext context)
    {
        var permissionNames = context.Permissions.Select(x => x.Name).Distinct().ToArray();
        Check.NotNullOrEmpty(permissionNames, nameof(permissionNames));

        var clientId = context.Principal?.FindFirst(AbpClaimTypes.ClientId)?.Value;
        if (clientId == null)
        {
            return new MultiplePermissionGrantResult(permissionNames);
        }

        using (CurrentTenant.Change(null))
        {
            return await ResourcePermissionStore.IsGrantedAsync(permissionNames, context.ResourceName, context.ResourceKey, Name, clientId);
        }
    }
}
