using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Volo.Abp.Authorization
{
    public class RequiresPermissionHandler : AuthorizationHandler<RequiresPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RequiresPermissionRequirement requirement)
        {
            if (requirement.PermissionName == "AllowedPermission")
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}