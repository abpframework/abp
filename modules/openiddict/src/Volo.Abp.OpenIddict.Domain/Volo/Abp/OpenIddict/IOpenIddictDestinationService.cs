using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Volo.Abp.OpenIddict
{
    public interface IOpenIddictDestinationService
    {
        IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal);

        Task SetDestinationsAsync(ClaimsPrincipal principal);
    }
}