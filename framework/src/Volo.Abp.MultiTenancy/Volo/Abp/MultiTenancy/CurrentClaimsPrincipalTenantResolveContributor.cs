using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.MultiTenancy
{
    public class CurrentClaimsPrincipalTenantResolveContributor : TenantResolveContributorBase
    {
        public const string ContributorName = "CurrentClaims";

        public override string Name => ContributorName;

        public override void Resolve(ITenantResolveContext context)
        {
            var principal = context.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>().Principal;
            if (principal?.Identity?.IsAuthenticated != true)
            {
                return;
            }

            context.Handled = true;
            context.TenantIdOrName = principal
                .Claims
                .FirstOrDefault(c => c.Type == AbpClaimTypes.TenantId)
                ?.Value;
        }
    }
}