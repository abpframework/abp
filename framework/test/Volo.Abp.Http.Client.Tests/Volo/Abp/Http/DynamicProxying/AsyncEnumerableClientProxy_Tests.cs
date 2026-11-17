using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.DynamicProxying;
using Xunit;

namespace Volo.Abp.Http.DynamicProxying;

public class AsyncEnumerableClientProxy_Tests : AbpHttpClientTestBase
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

    private readonly IRegularTestController _controller;

    public AsyncEnumerableClientProxy_Tests()
    {
        _controller = ServiceProvider.GetRequiredService<IRegularTestController>();
        AsyncEnumerableTestGate.Reset();
    }

    [Fact]
    public void Should_Not_Use_A_Token_That_Can_Not_Be_Cancelled()
    {
        var clientProxy = ServiceProvider.GetRequiredService<DynamicHttpProxyInterceptorClientProxy<IRegularTestController>>();

        clientProxy.UseCancellationToken(CancellationToken.None).ShouldBeSameAs(NullDisposable.Instance);
    }

    [Fact]
    public async Task Should_Send_The_Arguments_After_A_Leading_CancellationToken()
    {
        var items = new List<int>();
        await foreach (var item in _controller.GetStreamWithLeadingCancellationTokenAsync(CancellationToken.None, 3))
        {
            items.Add(item);
        }

        items.ShouldBe(new[] { 1, 2, 3 });
    }

    [Fact]
    public async Task Should_Stream_Items()
    {
        var items = new List<int>();
        await foreach (var item in _controller.GetStreamAsync(3))
        {
            items.Add(item);
        }

        items.ShouldBe(new[] { 1, 2, 3 });
    }

    [Fact]
    public async Task Should_Receive_Items_Before_Server_Finishes()
    {
        var items = new List<string>();

        try
        {
            // The server waits for the gate after the first item, so this only completes if the first item is streamed.
            await ConsumeAsync(_controller.GetGatedStreamAsync(false), item =>
            {
                items.Add(item);
                AsyncEnumerableTestGate.Open();
            }).WaitAsync(Timeout);
        }
        finally
        {
            AsyncEnumerableTestGate.Open();
        }

        items.ShouldBe(new[] { "first", "second" });
    }

    [Fact]
    public async Task Should_Throw_Remote_Exception_When_Server_Fails_Before_Response_Started()
    {
        var items = new List<int>();

        var exception = await Should.ThrowAsync<AbpRemoteCallException>(
            ConsumeAsync(_controller.GetStreamWithErrorAsync(), items.Add));

        exception.Error.Message.ShouldBe("Stream failed");
        items.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Throw_When_Server_Fails_After_Response_Started()
    {
        var items = new List<string>();

        Exception exception;
        try
        {
            exception = await Should.ThrowAsync<Exception>(
                ConsumeAsync(_controller.GetGatedStreamAsync(true), item =>
                {
                    items.Add(item);
                    AsyncEnumerableTestGate.Open();
                }).WaitAsync(Timeout));
        }
        finally
        {
            AsyncEnumerableTestGate.Open();
        }

        exception.ShouldNotBeOfType<TimeoutException>();
        items.ShouldBe(new[] { "first" });
    }

    [Fact]
    public async Task Should_Stop_When_Cancelled_While_Waiting_For_Next_Item()
    {
        var items = new List<string>();
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            try
            {
                await Should.ThrowAsync<OperationCanceledException>(
                    ConsumeAsync(_controller.GetGatedStreamAsync(false).WithCancellation(cancellationTokenSource.Token), item =>
                    {
                        items.Add(item);
                        cancellationTokenSource.Cancel();
                    }).WaitAsync(Timeout));
            }
            finally
            {
                AsyncEnumerableTestGate.Open();
            }

            items.ShouldBe(new[] { "first" });
        }
    }

    private static async Task ConsumeAsync<T>(IAsyncEnumerable<T> source, Action<T> onItem)
    {
        await foreach (var item in source)
        {
            onItem(item);
        }
    }

    private static async Task ConsumeAsync<T>(ConfiguredCancelableAsyncEnumerable<T> source, Action<T> onItem)
    {
        await foreach (var item in source)
        {
            onItem(item);
        }
    }
}
