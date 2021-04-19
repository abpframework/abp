using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Security.Claims;
using Volo.Abp.State;

namespace Volo.Abp.Authorization
{
    public class TestRequireEditionPermissionStateProvider : IStateProvider<PermissionDefinition>
    {
        public Task<bool> IsEnabledAsync(StateCheckContext<PermissionDefinition> context)
        {
            var currentPrincipalAccessor = context.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
            return Task.FromResult(currentPrincipalAccessor.Principal?.FindEditionId() != null);
        }
    }
}
