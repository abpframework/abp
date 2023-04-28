using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Authorization;

public class UsersRequirementHandler : AuthorizationHandler<UsersRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UsersRequirement requirement)
    {
        var currentUser = context.User;
        var currentUserId = currentUser.FindUserId();

        if (!currentUser.Identity.IsAuthenticated || !currentUserId.HasValue)
        {
            return Task.CompletedTask;
        }

        if (requirement.UserIds != null && requirement.UserIds.Contains(currentUserId.Value))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (requirement.UserNames != null)
        {
            var currentUserName = context.User.Claims?.FirstOrDefault(x => x.Type == AbpClaimTypes.UserName)?.Value;

            if (requirement.UserNames.Contains(currentUserName))
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}