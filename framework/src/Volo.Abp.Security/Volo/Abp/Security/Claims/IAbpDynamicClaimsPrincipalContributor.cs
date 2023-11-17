using System.Threading.Tasks;

namespace Volo.Abp.Security.Claims;

public interface IAbpDynamicClaimsPrincipalContributor
{
    Task ContributeAsync(AbpClaimsPrincipalContributorContext context);
}
