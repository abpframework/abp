using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Security.Claims;

public interface IAbpClaimsChecker
{
    Task CheckAsync(AbpClaimsCheckContext context);
}
