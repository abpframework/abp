using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.Dapr;

public interface IDaprAppApiTokenValidator
{
    Task CheckDaprAppApiTokenAsync();

    Task<bool> IsValidDaprAppApiTokenAsync();

    Task<string> GetDaprAppApiTokenOrNullAsync();
}
