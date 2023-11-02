using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Security.Claims;

/* This will be used by DynamicClaimsMiddleware to get dynamic claims 
 * and override the claims in the current principle.
 */
public class DynamicClaimService : IDynamicClaimService, ITransientDependency
{
    public Task<Claim[]> GetClaimsAsync()
    {
        /* TODO:
         * - If current user is not authenticated, return empty array.
         * - Try to get from distributed cache for the current user, if available
         * - If not available, call IDynamicClaimRefreshService.RefreshAsync()
         *   Then check from distributed cache again. If still not found, ignore it.
         */
        
        throw new NotImplementedException();
    }
}

public interface IDynamicClaimRefreshService
{
    Task RefreshAsync();
}

[Dependency(TryRegister = true)]
public class NullDynamicClaimRefreshService : IDynamicClaimRefreshService, ISingletonDependency
{
    public Task RefreshAsync()
    {
        /* TODO: Set distributed cache with an empty claim list
         */
        throw new NotImplementedException();
        
        /* TODO: 
         * IDynamicClaimRefreshService will have more implementations:
         * - AccountDynamicClaimRefreshService is the real implementation
         *   that constructs claims using UserManager.
         *   It will be effective in a monolith application, where the account
         *   module is installed in the application
         * - RemoteDynamicClaimRefreshService is used in a system (like a microservice solution)
         *   where the account module is located remotely. In that case, we make a REST call
         *   to the account service.
         *   RemoteDynamicClaimRefreshService should be registered only if
         *   NullDynamicClaimRefreshService is used. I mean AccountDynamicClaimRefreshService
         *   (in-process implementation) should work by default if available.
         *   RemoteDynamicClaimRefreshService uses AbpDynamicClaimsOptions.RemoteRefreshUrl (and issuer)
         */
    }
}

