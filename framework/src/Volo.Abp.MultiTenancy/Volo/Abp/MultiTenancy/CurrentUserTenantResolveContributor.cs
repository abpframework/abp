using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Users;

namespace Volo.Abp.MultiTenancy
{
    public class CurrentUserTenantResolveContributor : TenantResolveContributorBase
    {
        public const string ContributorName = "CurrentUser";

        public override string Name => ContributorName;

        public override Task ResolveAsync(ITenantResolveContext context)
        {
            var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
            if (currentUser.IsAuthenticated)
            {
                context.Handled = true;
                context.TenantIdOrName = currentUser.TenantId?.ToString();
            }

            return Task.CompletedTask;
        }
    }
}
