using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Security.Claims
{
    public class AbpClaimsIdentityService : IAbpClaimsIdentityService, ITransientDependency
    {
        protected AbpClaimOptions Options { get; }
        protected IServiceScopeFactory ServiceScopeFactory { get; }

        public AbpClaimsIdentityService(
            IServiceScopeFactory serviceScopeFactory,
            IOptions<AbpClaimOptions> abpClaimOptions)
        {
            ServiceScopeFactory = serviceScopeFactory;
            Options = abpClaimOptions.Value;
        }

        public async Task AddClaimsAsync(ClaimsIdentity identity)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = new ClaimsIdentityContext(identity, scope.ServiceProvider);

                foreach (var contributorType in Options.ClaimsIdentityContributors)
                {
                    var contributor = (IClaimsIdentityContributor) scope.ServiceProvider.GetRequiredService(contributorType);
                    await contributor.AddClaimsAsync(context);
                }
            }
        }
    }
}
