using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Http.DynamicProxying;

namespace Volo.Abp.Http.Client.ClientProxying;

public class RegularTestControllerStaticClientProxy : ClientProxyBase<IRegularTestController>
{
    public virtual IAsyncEnumerable<int> GetStreamAsync(int count)
    {
        return RequestAsyncEnumerable<int>(nameof(GetStreamAsync), new ClientProxyRequestTypeValue
        {
            { typeof(int), count }
        });
    }

    public virtual IAsyncEnumerable<int> GetStreamWithLeadingCancellationTokenAsync(CancellationToken cancellationToken, int count)
    {
        return RequestAsyncEnumerable<int>(nameof(GetStreamWithLeadingCancellationTokenAsync), new ClientProxyRequestTypeValue
        {
            { typeof(CancellationToken), cancellationToken },
            { typeof(int), count }
        });
    }

    public virtual async Task<string> GetWithStringPathAsync(string name)
    {
        return await RequestAsync<string>(nameof(GetWithStringPathAsync), new ClientProxyRequestTypeValue
        {
            { typeof(string), name }
        });
    }

    public virtual async Task<string> GetWithSwappedQueryNamesAsync(int first, int second)
    {
        return await RequestAsync<string>(nameof(GetWithSwappedQueryNamesAsync), new ClientProxyRequestTypeValue
        {
            { typeof(int), first },
            { typeof(int), second }
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
