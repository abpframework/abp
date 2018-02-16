using Microsoft.AspNetCore.Authorization;

namespace Volo.Abp.Authorization
{
    public class RequiresPermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; set; }
    }
}