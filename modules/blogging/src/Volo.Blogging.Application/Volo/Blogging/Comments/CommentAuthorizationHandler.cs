using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Blogging.Comments
{
    public class CommentAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Comment>
    {
        protected IPermissionChecker PermissionChecker { get; }

        public CommentAuthorizationHandler(IPermissionChecker permissionChecker)
        {
            PermissionChecker = permissionChecker;
        }

        protected async override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Comment resource)
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

        private async Task<bool> HasDeletePermission(AuthorizationHandlerContext context, Comment resource)
        {
            if (resource.CreatorId != null && resource.CreatorId == context.User.FindUserId())
            {
                return true;
            }

            if (await PermissionChecker.IsGrantedAsync(context.User, BloggingPermissions.Comments.Delete))
            {
                return true;
            }

            return false;
        }

        private async Task<bool> HasUpdatePermission(AuthorizationHandlerContext context, Comment resource)
        {
            if (resource.CreatorId != null && resource.CreatorId == context.User.FindUserId())
            {
                return true;
            }

            if (await PermissionChecker.IsGrantedAsync(context.User, BloggingPermissions.Comments.Update))
            {
                return true;
            }

            return false;
        }
    }
}