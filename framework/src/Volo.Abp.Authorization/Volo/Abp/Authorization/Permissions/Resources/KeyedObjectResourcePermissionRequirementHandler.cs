using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Volo.Abp.Authorization.Permissions.Resources;

public class KeyedObjectResourcePermissionRequirementHandler : AuthorizationHandler<ResourcePermissionRequirement, IKeyedObject>
{
    protected readonly IResourcePermissionChecker PermissionChecker;

    public KeyedObjectResourcePermissionRequirementHandler(IResourcePermissionChecker permissionChecker)
    {
        PermissionChecker = permissionChecker;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ResourcePermissionRequirement requirement,
        IKeyedObject? resource)
    {
        if (resource == null)
        {
            return;
        }

        var resourceName = resource.GetType().FullName!;
        var resourceKey = resource.GetObjectKey() ?? throw new AbpException("The resource doesn't have a key.");
        
        if (await PermissionChecker.IsGrantedAsync(context.User, requirement.PermissionName, resourceName, resourceKey))
        {
            context.Succeed(requirement);
        }
    }
}
