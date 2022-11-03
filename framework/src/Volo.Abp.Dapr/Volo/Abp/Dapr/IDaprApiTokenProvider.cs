using System.Threading.Tasks;

namespace Volo.Abp.Dapr;

public interface IDaprApiTokenProvider
{
    Task<string> GetDaprApiTokenAsync();

    Task<string> GetAppApiTokenAsync();
}
