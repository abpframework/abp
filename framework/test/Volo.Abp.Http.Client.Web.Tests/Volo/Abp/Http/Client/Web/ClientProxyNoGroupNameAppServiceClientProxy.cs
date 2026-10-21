using System.Threading.Tasks;
using Volo.Abp.Http.Client.ClientProxying;

namespace Volo.Abp.Http.Client.Web;

public class ClientProxyNoGroupNameAppServiceClientProxy : ClientProxyBase<IClientProxyNoGroupNameAppService>, IClientProxyNoGroupNameAppService
{
    public virtual Task<string> GetAsync()
    {
        return Task.FromResult("no-group-name");
    }
}
