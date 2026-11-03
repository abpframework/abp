using System.Threading.Tasks;
using Volo.Abp.Http.DynamicProxying;

namespace Volo.Abp.Http.Client.ClientProxying;

public class RegularTestControllerStaticClientProxy : ClientProxyBase<IRegularTestController>
{
    public virtual async Task<string> GetWithStringPathAsync(string name)
    {
        return await RequestAsync<string>(nameof(GetWithStringPathAsync), new ClientProxyRequestTypeValue
        {
            { typeof(string), name }
        });
    }

    public virtual async Task<string> GetWithCatchAllPathAsync(string path)
    {
        return await RequestAsync<string>(nameof(GetWithCatchAllPathAsync), new ClientProxyRequestTypeValue
        {
            { typeof(string), path }
        });
    }

    public virtual async Task<string> GetWithSingleStarCatchAllPathAsync(string path)
    {
        return await RequestAsync<string>(nameof(GetWithSingleStarCatchAllPathAsync), new ClientProxyRequestTypeValue
        {
            { typeof(string), path }
        });
    }
}
