using System.Threading.Tasks;
using Volo.Abp.Http.Client.ClientProxying;

namespace Volo.Abp.Http.Client.Web;

// Returns the received value instead of calling a remote service, so the tests can see what the proxy controller bound.
public class ClientProxyTestAppServiceClientProxy : ClientProxyBase<IClientProxyTestAppService>, IClientProxyTestAppService
{
    public virtual Task<string> GetFromBodyAsync(string value)
    {
        return Task.FromResult(value);
    }

    public virtual Task<string> GetFromQueryAsync(string value)
    {
        return Task.FromResult(value);
    }
}
