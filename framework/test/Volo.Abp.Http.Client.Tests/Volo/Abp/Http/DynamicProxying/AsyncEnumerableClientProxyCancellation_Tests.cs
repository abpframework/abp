using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Http.Client.DynamicProxying;
using Volo.Abp.Http.Client.Proxying;
using Xunit;

namespace Volo.Abp.Http.DynamicProxying;

public abstract class AsyncEnumerableClientProxyCancellation_Tests : AbpHttpClientTestBase
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

    protected IRegularTestController Controller => ServiceProvider.GetRequiredService<IRegularTestController>();

    protected abstract bool BlockApiDefinitionRequest { get; }

    /// <summary>
    /// Released when a request reaches the blocking handler, so a test can cancel exactly at that stage.
    /// </summary>
    protected SemaphoreSlim BlockedRequests { get; } = new(0);

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Singleton<IProxyHttpClientFactory>(serviceProvider =>
            new BlockingProxyHttpClientFactory(serviceProvider.GetRequiredService<ITestServerAccessor>(), BlockApiDefinitionRequest, BlockedRequests)));
    }

    protected async Task WaitForBlockedRequestsAsync(int count)
    {
        for (var i = 0; i < count; i++)
        {
            (await BlockedRequests.WaitAsync(Timeout)).ShouldBeTrue("The request did not reach the blocking handler.");
        }
    }

    protected async Task ShouldBeCancelledWhenBlockedAsync(Func<Task> enumerate, CancellationTokenSource cancellationTokenSource)
    {
        var enumeration = enumerate();
        await WaitForBlockedRequestsAsync(1);
        cancellationTokenSource.Cancel();
        await ShouldBeCancelledAsync(() => enumeration);
    }

    protected static async Task ShouldBeCancelledAsync(Func<Task> enumerate)
    {
        var exception = await Should.ThrowAsync<Exception>(() => enumerate().WaitAsync(Timeout));

        exception.ShouldNotBeOfType<TimeoutException>();
        (exception is OperationCanceledException || exception.InnerException is OperationCanceledException).ShouldBeTrue(exception.ToString());
    }

    private class BlockingProxyHttpClientFactory : IProxyHttpClientFactory
    {
        private readonly ITestServerAccessor _testServerAccessor;
        private readonly bool _blockApiDefinitionRequest;
        private readonly SemaphoreSlim _blockedRequests;
        private int _count;

        public BlockingProxyHttpClientFactory(ITestServerAccessor testServerAccessor, bool blockApiDefinitionRequest, SemaphoreSlim blockedRequests)
        {
            _testServerAccessor = testServerAccessor;
            _blockApiDefinitionRequest = blockApiDefinitionRequest;
            _blockedRequests = blockedRequests;
        }

        public HttpClient Create(string name) => Create();

        public HttpClient Create()
        {
            if (Interlocked.Increment(ref _count) == 1 && !_blockApiDefinitionRequest)
            {
                return _testServerAccessor.Server.CreateClient();
            }

            return new HttpClient(new BlockingHttpMessageHandler(_blockedRequests))
            {
                BaseAddress = new Uri("http://localhost/")
            };
        }
    }

    private class BlockingHttpMessageHandler : HttpMessageHandler
    {
        private readonly SemaphoreSlim _blockedRequests;

        public BlockingHttpMessageHandler(SemaphoreSlim blockedRequests)
        {
            _blockedRequests = blockedRequests;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _blockedRequests.Release();
            await Task.Delay(System.Threading.Timeout.Infinite, cancellationToken);
            throw new InvalidOperationException("Unreachable");
        }
    }
}

public class AsyncEnumerableClientProxy_Request_Cancellation_Tests : AsyncEnumerableClientProxyCancellation_Tests
{
    protected override bool BlockApiDefinitionRequest => false;

    [Fact]
    public async Task Should_Cancel_Request_Before_Response_Headers_When_Method_Has_Its_Own_Token()
    {
        using (var methodCancellationTokenSource = new CancellationTokenSource())
        using (var enumerationCancellationTokenSource = new CancellationTokenSource())
        {
            await ShouldBeCancelledWhenBlockedAsync(async () =>
            {
                await foreach (var _ in Controller.GetStreamWithCancellationTokenAsync(methodCancellationTokenSource.Token)
                                   .WithCancellation(enumerationCancellationTokenSource.Token))
                {
                }
            }, enumerationCancellationTokenSource);
        }
    }
}

public class AsyncEnumerableClientProxy_RequestContext_Cancellation_Tests : AsyncEnumerableClientProxyCancellation_Tests
{
    protected override bool BlockApiDefinitionRequest => false;

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddSingleton<RequestContextRecorder>();
        services.Replace(ServiceDescriptor.Transient<DynamicHttpProxyInterceptorClientProxy<IRegularTestController>, RecordingClientProxy>());
    }

    [Fact]
    public async Task Should_Cancel_Request_With_The_Token_Of_The_Request_Context()
    {
        var action = await GetActionAsync();
        using (var contextCancellationTokenSource = new CancellationTokenSource())
        {
            var requestContext = new ClientProxyRequestContext(action.Action, action.Arguments, action.ServiceType)
            {
                CancellationToken = contextCancellationTokenSource.Token
            };

            await ShouldBeCancelledWhenBlockedAsync(async () =>
            {
                await foreach (var _ in ClientProxy.CallRequestAsyncEnumerable<int>(requestContext))
                {
                }
            }, contextCancellationTokenSource);
        }
    }

    [Fact]
    public async Task Should_Isolate_Cancellation_Of_Concurrent_Enumerations_Of_The_Same_Request_Context()
    {
        var action = await GetActionAsync();
        var requestContext = new ClientProxyRequestContext(action.Action, action.Arguments, action.ServiceType);
        var sequence = ClientProxy.CallRequestAsyncEnumerable<int>(requestContext);
        using (var firstCancellationTokenSource = new CancellationTokenSource())
        using (var secondCancellationTokenSource = new CancellationTokenSource())
        {
            var first = EnumerateAsync(sequence, firstCancellationTokenSource.Token);
            var second = EnumerateAsync(sequence, secondCancellationTokenSource.Token);
            await WaitForBlockedRequestsAsync(2);

            firstCancellationTokenSource.Cancel();
            await ShouldBeCancelledAsync(() => first);
            await Task.Delay(200);
            second.IsCompleted.ShouldBeFalse();

            secondCancellationTokenSource.Cancel();
            await ShouldBeCancelledAsync(() => second);
            requestContext.CancellationToken.ShouldBeNull();
        }
    }

    [Fact]
    public async Task Should_Cancel_Request_With_Method_Token_That_Is_Not_The_Last_Argument()
    {
        using (var methodCancellationTokenSource = new CancellationTokenSource())
        {
            await ShouldBeCancelledWhenBlockedAsync(async () =>
            {
                await foreach (var _ in Controller.GetStreamWithLeadingCancellationTokenAsync(methodCancellationTokenSource.Token, 3))
                {
                }
            }, methodCancellationTokenSource);
        }
    }

    private DynamicHttpProxyInterceptorClientProxy<IRegularTestController> ClientProxy =>
        ServiceProvider.GetRequiredService<DynamicHttpProxyInterceptorClientProxy<IRegularTestController>>();

    private async Task<ClientProxyRequestContext> GetActionAsync()
    {
        // The business request is blocked, but RequestAsync records its context before sending it.
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            await ShouldBeCancelledWhenBlockedAsync(async () =>
            {
                await foreach (var _ in Controller.GetStreamAsync(1).WithCancellation(cancellationTokenSource.Token))
                {
                }
            }, cancellationTokenSource);

            return ServiceProvider.GetRequiredService<RequestContextRecorder>().Contexts.Last();
        }
    }

    private static async Task EnumerateAsync(IAsyncEnumerable<int> sequence, CancellationToken cancellationToken)
    {
        await foreach (var _ in sequence.WithCancellation(cancellationToken))
        {
        }
    }

    public class RequestContextRecorder
    {
        public ConcurrentQueue<ClientProxyRequestContext> Contexts { get; } = new();
    }

    public class RecordingClientProxy : DynamicHttpProxyInterceptorClientProxy<IRegularTestController>
    {
        protected override Task<HttpContent> RequestAsync(ClientProxyRequestContext requestContext)
        {
            LazyServiceProvider.LazyGetRequiredService<RequestContextRecorder>().Contexts.Enqueue(requestContext);
            return base.RequestAsync(requestContext);
        }
    }
}

public class AsyncEnumerableClientProxy_ApiDefinition_Cancellation_Tests : AsyncEnumerableClientProxyCancellation_Tests
{
    protected override bool BlockApiDefinitionRequest => true;

    [Fact]
    public async Task Should_Cancel_Api_Definition_Request_With_Method_Token()
    {
        using (var methodCancellationTokenSource = new CancellationTokenSource())
        {
            await ShouldBeCancelledWhenBlockedAsync(async () =>
            {
                await foreach (var _ in Controller.GetStreamWithCancellationTokenAsync(methodCancellationTokenSource.Token))
                {
                }
            }, methodCancellationTokenSource);
        }
    }

    [Fact]
    public async Task Should_Cancel_Api_Definition_Request_With_Method_Token_That_Is_Not_The_Last_Argument()
    {
        using (var methodCancellationTokenSource = new CancellationTokenSource())
        {
            await ShouldBeCancelledWhenBlockedAsync(async () =>
            {
                await foreach (var _ in Controller.GetStreamWithLeadingCancellationTokenAsync(methodCancellationTokenSource.Token, 3))
                {
                }
            }, methodCancellationTokenSource);
        }
    }

    [Fact]
    public async Task Should_Cancel_Api_Definition_Request()
    {
        using (var enumerationCancellationTokenSource = new CancellationTokenSource())
        {
            await ShouldBeCancelledWhenBlockedAsync(async () =>
            {
                await foreach (var _ in Controller.GetStreamAsync(3).WithCancellation(enumerationCancellationTokenSource.Token))
                {
                }
            }, enumerationCancellationTokenSource);
        }
    }
}
