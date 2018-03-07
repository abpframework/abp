using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.MultiTenancy
{
    public class CurrentClaimsPrincipalTenantResolveContributer : ITenantResolveContributer
    {
        public void Resolve(ITenantResolveContext context)
        {
            var principal = context.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>().Principal;
            if (principal?.Identity?.IsAuthenticated != true)
            {
                return;
            }

            context.TenantIdOrName = principal.Claims.FirstOrDefault(c => c.Type == AbpClaimTypes.TenantId)?.Value;
            context.Handled = true;
        }
    }
}