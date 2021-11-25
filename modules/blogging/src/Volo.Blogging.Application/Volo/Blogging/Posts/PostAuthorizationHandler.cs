using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Blogging.Posts
{
    public class PostAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Post>
    {
        private readonly IPermissionChecker _permissionChecker;

        public PostAuthorizationHandler(IPermissionChecker permissionChecker)
        {
            _permissionChecker = permissionChecker;
        }

        protected async override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Post resource)
        {
            if (requirement.Name == CommonOperations.Delete.Name && await HasDeletePermission(context, resource))
            {
                context.Succeed(requirement);
                return;
            }

            if (requirement.Name == CommonOperations.Update.Name && await HasUpdatePermission(context, resource))
            {
                context.Succeed(requirement);
                return;
            }
        }

        private async Task<bool> HasDeletePermission(AuthorizationHandlerContext context, Post resource)
        {
            if (resource.CreatorId != null && resource.CreatorId == context.User.FindUserId())
            {
                return true;
            }

            if (await _permissionChecker.IsGrantedAsync(context.User, BloggingPermissions.Posts.Delete))
            {
                return true;
            }

            return false;
        }

        private async Task<bool> HasUpdatePermission(AuthorizationHandlerContext context, Post resource)
        {
            if (resource.CreatorId != null && resource.CreatorId == context.User.FindUserId())
            {
                return true;
            }

            if (await _permissionChecker.IsGrantedAsync(context.User, BloggingPermissions.Posts.Update))
            {
                return true;
            }

            return false;
        }
    }
}