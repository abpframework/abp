using System.Threading.Tasks;
using Volo.Abp.Http.Client.ClientProxying;

namespace Volo.Abp.Http.Client.Web;

public class ClientProxyDisabledActionAppServiceClientProxy : ClientProxyBase<IClientProxyDisabledActionAppService>, IClientProxyDisabledActionAppService
{
    [RemoteService(false)]
    public virtual Task<string> GetAsync()
    {
        return Task.FromResult("disabled-action");
    }
}
