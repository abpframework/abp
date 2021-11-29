using System.Threading.Tasks;

namespace Volo.Abp.Security.Claims
{
    public interface IAbpClaimsPrincipalContributor
    {
        Task ContributeAsync(AbpClaimsPrincipalContributorContext context);
    }
}
