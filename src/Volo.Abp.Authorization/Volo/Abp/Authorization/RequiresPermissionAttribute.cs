using Microsoft.AspNetCore.Authorization;

namespace Volo.Abp.Authorization
{
    public class RequiresPermissionAttribute : AuthorizeAttribute
    {
        public RequiresPermissionAttribute(string permissionName)
        {
            Policy = permissionName;
        }
    }
}