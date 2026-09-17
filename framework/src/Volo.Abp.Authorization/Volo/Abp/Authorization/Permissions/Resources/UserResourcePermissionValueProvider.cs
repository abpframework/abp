using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Authorization.Permissions.Resources;

public class UserResourcePermissionValueProvider : ResourcePermissionValueProvider
{
    public const string ProviderName = "U";

    public override string Name => ProviderName;

    public UserResourcePermissionValueProvider(IResourcePermissionStore resourcePermissionStore)
        : base(resourcePermissionStore)
    {

    }

    public override async Task<PermissionGrantResult> CheckAsync(ResourcePermissionValueCheckContext context)
    {
        var userId = context.Principal?.FindFirst(AbpClaimTypes.UserId)?.Value;

        if (userId == null)
        {
            return PermissionGrantResult.Undefined;
        }

        return await ResourcePermissionStore.IsGrantedAsync(context.Permission.Name, context.ResourceName, context.ResourceKey, Name, userId)
            ? PermissionGrantResult.Granted
            : PermissionGrantResult.Undefined;
    }

    public override async Task<MultiplePermissionGrantResult> CheckAsync(ResourcePermissionValuesCheckContext context)
    {
        var permissionNames = context.Permissions.Select(x => x.Name).Distinct().ToArray();
        Check.NotNullOrEmpty(permissionNames, nameof(permissionNames));

        var userId = context.Principal?.FindFirst(AbpClaimTypes.UserId)?.Value;
        if (userId == null)
        {
            return new MultiplePermissionGrantResult(permissionNames);
        }

        return await ResourcePermissionStore.IsGrantedAsync(permissionNames, context.ResourceName, context.ResourceKey, Name, userId);
    }
}
