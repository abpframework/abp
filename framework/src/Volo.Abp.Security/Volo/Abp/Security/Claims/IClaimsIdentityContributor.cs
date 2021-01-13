using System.Threading.Tasks;

namespace Volo.Abp.Security.Claims
{
    public interface IClaimsIdentityContributor
    {
        Task AddClaimsAsync(ClaimsIdentityContext context);
    }
}
