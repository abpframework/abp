using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Volo.Abp.Authorization;

public class RolesRequirementHandler : AuthorizationHandler<RolesRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RolesRequirement requirement)
    {
        if (requirement.RequiresAll
                ? requirement.NormalizedRoleNames.All(role => context.User.IsInRole(role))
                : requirement.NormalizedRoleNames.Any(role => context.User.IsInRole(role)))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}