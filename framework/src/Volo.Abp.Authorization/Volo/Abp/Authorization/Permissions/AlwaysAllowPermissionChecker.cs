using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Threading;

namespace Volo.Abp.Authorization.Permissions
{
    /// <summary>
    /// Always allows for any permission.
    /// 
    /// Use IServiceCollection.AddAlwaysAllowAuthorization() to replace
    /// IPermissionChecker with this class. This is useful for tests.
    /// </summary>
    public class AlwaysAllowPermissionChecker : IPermissionChecker
    {
        public Task<bool> IsGrantedAsync(string name)
        {
            return TaskCache.TrueResult;
        }

        public Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
        {
            return TaskCache.TrueResult;
        }
    }
}
