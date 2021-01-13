using System.Security.Claims;
using System.Threading.Tasks;

namespace Volo.Abp.Security.Claims
{
    public interface IAbpClaimsIdentityService
    {
        Task AddClaimsAsync(ClaimsIdentity identity);
    }
}
