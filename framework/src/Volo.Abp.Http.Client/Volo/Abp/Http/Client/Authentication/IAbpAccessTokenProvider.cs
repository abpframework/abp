using System.Threading.Tasks;

namespace Volo.Abp.Http.Client.Authentication;

public interface IAbpAccessTokenProvider
{
    Task<string?> GetTokenAsync();
}
