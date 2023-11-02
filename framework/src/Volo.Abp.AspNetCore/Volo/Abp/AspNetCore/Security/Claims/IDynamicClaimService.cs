using System.Security.Claims;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Security.Claims;

public interface IDynamicClaimService
{
    Task<Claim[]> GetClaimsAsync();
}