using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Users;

namespace Volo.Abp.MultiTenancy
{
    public class CurrentClaimsPrincipalTenantResolveContributor : TenantResolveContributorBase
    {
        public const string ContributorName = "CurrentClaims";

        public override string Name => ContributorName;

        public override void Resolve(ITenantResolveContext context)
        {
            var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
            if (currentUser.IsAuthenticated != true)
            {
                return;
            }

            context.Handled = true;
            context.TenantIdOrName = currentUser.TenantId?.ToString();
        }
    }
}