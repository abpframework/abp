using System;
using System.Collections.Generic;
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

public class AsyncEnumerableClientProxy_NestedRequest_Tests : AbpHttpClientTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient<DynamicHttpProxyInterceptorClientProxy<IRegularTestController>, NestedRequestClientProxy>());
        services.Replace(ServiceDescriptor.Singleton<IProxyHttpClientFactory>(serviceProvider =>
            new TokenCheckingProxyHttpClientFactory(serviceProvider.GetRequiredService<ITestServerAccessor>())));
    }

    [Fact]
    public async Task Should_Keep_The_Token_Of_Another_Request_Sent_While_Sending_A_Stream()
    {
        NestedRequestClientProxy.NestedRequestExceptions.Clear();
        var controller = ServiceProvider.GetRequiredService<IRegularTestController>();

        var items = new List<int>();
        await foreach (var item in controller.GetStreamAsync(2))
        {
            items.Add(item);
        }

        items.ShouldBe(new[] { 1, 2 });
        var exception = NestedRequestClientProxy.NestedRequestExceptions.ShouldHaveSingleItem();
        exception.ShouldNotBeNull();
        (exception is OperationCanceledException || exception.InnerException is OperationCanceledException).ShouldBeTrue(exception.ToString());
    }

    public class NestedRequestClientProxy : DynamicHttpProxyInterceptorClientProxy<IRegularTestController>
    {
        public static List<Exception?> NestedRequestExceptions { get; } = new();

        protected override async Task<HttpContent> RequestAsync(ClientProxyRequestContext requestContext)
        {
            if (requestContext.Action.Name == nameof(IRegularTestController.GetStreamAsync))
            {
                // Another request with its own (cancelled) token, sent before the stream request.
                var nestedRequestContext = new ClientProxyRequestContext(requestContext.Action, requestContext.Arguments, requestContext.ServiceType)
                {
                    CancellationToken = new CancellationToken(canceled: true)
                };

                try
                {
                    using (await base.RequestAsync(nestedRequestContext))
                    {
                    }

                    NestedRequestExceptions.Add(null);
                }
                catch (Exception exception)
                {
                    NestedRequestExceptions.Add(exception);
                }
            }

            return await base.RequestAsync(requestContext);
        }
    }

    private class TokenCheckingProxyHttpClientFactory : IProxyHttpClientFactory
    {
        private readonly ITestServerAccessor _testServerAccessor;

        public TokenCheckingProxyHttpClientFactory(ITestServerAccessor testServerAccessor)
        {
            _testServerAccessor = testServerAccessor;
        }

        public HttpClient Create(string name) => Create();

        public HttpClient Create()
        {
            return new HttpClient(new TokenCheckingHandler(_testServerAccessor.Server.CreateHandler()))
            {
                BaseAddress = _testServerAccessor.Server.BaseAddress
            };
        }
    }

    // The test server does not observe a token that is already cancelled when the request is sent.
    private class TokenCheckingHandler : DelegatingHandler
    {
        public TokenCheckingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return base.SendAsync(request, cancellationToken);
        }
    }
}
