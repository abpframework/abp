using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
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
    public async Task Should_Send_The_Arguments_After_A_Leading_CancellationToken()
    {
        var proxy = ServiceProvider.GetRequiredService<RegularTestControllerStaticClientProxy>();

        (await ToListAsync(proxy.GetStreamWithLeadingCancellationTokenAsync(CancellationToken.None, 3))).ShouldBe(new[] { 1, 2, 3 });
    }

    [Fact]
    public async Task Should_Send_The_Request_With_The_CancellationToken_Argument()
    {
        var proxy = ServiceProvider.GetRequiredService<RecordingStaticClientProxy>();
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            (await ToListAsync(proxy.GetStreamWithLeadingCancellationTokenAsync(cancellationTokenSource.Token, 3))).ShouldBe(new[] { 1, 2, 3 });

            proxy.RequestContexts.ShouldHaveSingleItem().CancellationToken.ShouldBe(cancellationTokenSource.Token);
        }
    }

    [Fact]
    public async Task Should_Not_Stream_Items_When_The_CancellationToken_Argument_Is_Canceled()
    {
        var proxy = ServiceProvider.GetRequiredService<RegularTestControllerStaticClientProxy>();

        var items = new List<int>();

        var exception = await Should.ThrowAsync<Exception>(async () =>
        {
            await foreach (var item in proxy.GetStreamWithLeadingCancellationTokenAsync(new CancellationToken(canceled: true), 3))
            {
                items.Add(item);
            }
        });

        IsCanceled(exception).ShouldBeTrue(exception.ToString());
        items.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Send_The_Arguments_In_The_Order_Of_The_Request_Parameters()
    {
        var proxy = ServiceProvider.GetRequiredService<RegularTestControllerStaticClientProxy>();

        (await proxy.GetWithSwappedQueryNamesAsync(10, 20)).ShouldBe("10-20");
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

    // The test server reports a request canceled after it is sent as an aborted response.
    private static bool IsCanceled(Exception? exception)
    {
        for (; exception != null; exception = exception.InnerException)
        {
            if (exception is OperationCanceledException || exception is IOException && exception.Message.Contains("aborted"))
            {
                return true;
            }
        }

        return false;
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
