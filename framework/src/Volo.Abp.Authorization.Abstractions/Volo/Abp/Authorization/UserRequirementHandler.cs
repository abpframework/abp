using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Authorization;

public class UserRequirementHandler : AuthorizationHandler<UserRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UserRequirement requirement)
    {
        if (requirement.UserId != null && requirement.UserId == context.User.FindUserId())
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (requirement.UserName != null)
        {
            var currentUserName = context.User.Claims?.FirstOrDefault(x => x.Type == AbpClaimTypes.UserName)?.Value;

            if (requirement.UserName == currentUserName)
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}