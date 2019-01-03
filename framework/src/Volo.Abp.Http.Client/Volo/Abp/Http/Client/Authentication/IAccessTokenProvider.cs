using System.Threading.Tasks;
using Volo.Abp.Http.Client.DynamicProxying;

namespace Volo.Abp.Http.Client.Authentication
{
    public interface IAccessTokenProvider
    {
        Task<string> GetOrNullAsync(DynamicHttpClientProxyConfig config);
    }
}
