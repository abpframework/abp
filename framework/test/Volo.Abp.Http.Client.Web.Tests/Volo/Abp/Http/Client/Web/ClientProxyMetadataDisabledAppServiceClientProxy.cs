using System.Threading.Tasks;
using Volo.Abp.Http.Client.ClientProxying;

namespace Volo.Abp.Http.Client.Web;

[RemoteService(IsMetadataEnabled = false)]
public class ClientProxyMetadataDisabledAppServiceClientProxy : ClientProxyBase<IClientProxyMetadataDisabledAppService>, IClientProxyMetadataDisabledAppService
{
    public virtual Task<string> GetAsync()
    {
        return Task.FromResult("metadata-disabled");
    }
}
