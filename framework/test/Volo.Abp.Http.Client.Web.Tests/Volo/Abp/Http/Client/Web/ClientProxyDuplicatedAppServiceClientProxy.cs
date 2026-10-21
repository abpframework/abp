using System.Threading.Tasks;
using Volo.Abp.Http.Client.ClientProxying;

namespace Volo.Abp.Http.Client.Web;

public class ClientProxyDuplicatedAppServiceClientProxy : ClientProxyBase<IClientProxyDuplicatedAppService>, IClientProxyDuplicatedAppService
{
    public virtual Task<string> GetAsync()
    {
        return Task.FromResult("proxy");
    }
}
