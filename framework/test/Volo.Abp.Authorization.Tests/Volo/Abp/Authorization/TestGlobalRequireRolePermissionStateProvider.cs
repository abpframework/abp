using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Authorization
{
    public class TestGlobalRequireRolePermissionStateProvider : IPermissionStateProvider, ITransientDependency
    {
        public Task<bool> IsEnabledAsync(PermissionStateContext context)
        {
            var currentPrincipalAccessor = context.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
            return Task.FromResult(currentPrincipalAccessor.Principal != null && currentPrincipalAccessor.Principal.IsInRole("admin"));
        }
    }
}
