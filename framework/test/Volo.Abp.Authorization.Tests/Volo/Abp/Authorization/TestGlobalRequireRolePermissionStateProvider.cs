using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.State;

namespace Volo.Abp.Authorization
{
    public class TestGlobalRequireRolePermissionStateProvider : IStateProvider<PermissionDefinition>, ITransientDependency
    {
        public Task<bool> IsEnabledAsync(StateCheckContext<PermissionDefinition> context)
        {
            var currentPrincipalAccessor = context.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
            return Task.FromResult(currentPrincipalAccessor.Principal != null && currentPrincipalAccessor.Principal.IsInRole("admin"));
        }
    }
}
