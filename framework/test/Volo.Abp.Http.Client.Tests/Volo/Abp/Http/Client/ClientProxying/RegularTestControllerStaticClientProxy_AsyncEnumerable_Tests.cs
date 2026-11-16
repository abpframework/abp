using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.Http.Client.ClientProxying;

public class RegularTestControllerStaticClientProxy_AsyncEnumerable_Tests : AbpHttpClientTestBase
{
    [Fact]
    public async Task Should_Stream_Items()
    {
        var proxy = ServiceProvider.GetRequiredService<RegularTestControllerStaticClientProxy>();

        (await ToListAsync(proxy.GetStreamAsync(3))).ShouldBe(new[] { 1, 2, 3 });
    }

    [Fact]
    public async Task Should_Build_A_Request_Context_For_Each_Enumeration()
    {
        var proxy = ServiceProvider.GetRequiredService<RecordingStaticClientProxy>();
        var sequence = proxy.GetStreamAsync(3);

        var results = await Task.WhenAll(ToListAsync(sequence), ToListAsync(sequence));

        results[0].ShouldBe(new[] { 1, 2, 3 });
        results[1].ShouldBe(new[] { 1, 2, 3 });
        proxy.RequestContexts.Distinct().Count().ShouldBe(2);
        proxy.RequestContexts.ShouldAllBe(context => context.CancellationToken == null);
    }

    private static async Task<List<int>> ToListAsync(IAsyncEnumerable<int> source)
    {
        var list = new List<int>();
        await foreach (var item in source)
        {
            list.Add(item);
        }

        return list;
    }

    public class RecordingStaticClientProxy : RegularTestControllerStaticClientProxy
    {
        public ConcurrentQueue<ClientProxyRequestContext> RequestContexts { get; } = new();

        protected override Task<HttpContent> RequestAsync(ClientProxyRequestContext requestContext)
        {
            RequestContexts.Enqueue(requestContext);
            return base.RequestAsync(requestContext);
        }
    }
}
