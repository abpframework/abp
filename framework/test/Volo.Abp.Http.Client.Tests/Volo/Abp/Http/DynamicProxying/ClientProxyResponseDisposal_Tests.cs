using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.Proxying;
using Xunit;

namespace Volo.Abp.Http.DynamicProxying;

public class ClientProxyResponseDisposal_Tests : AbpHttpClientTestBase
{
    private readonly IRegularTestController _controller;
    private readonly StubProxyHttpClientFactory _factory;

    public ClientProxyResponseDisposal_Tests()
    {
        _controller = ServiceProvider.GetRequiredService<IRegularTestController>();
        _factory = (StubProxyHttpClientFactory)ServiceProvider.GetRequiredService<IProxyHttpClientFactory>();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Singleton<IProxyHttpClientFactory, StubProxyHttpClientFactory>());
    }

    [Fact]
    public async Task Non_Abp_Error_Response_Should_Dispose_HttpContent()
    {
        var content = new TrackingHttpContent("Bad Gateway from upstream");
        _factory.SetStubResponse(() => new HttpResponseMessage(HttpStatusCode.BadGateway)
        {
            Content = content
        });

        var exception = await Assert.ThrowsAsync<AbpRemoteCallException>(
            () => _controller.IncrementValueAsync(1));

        exception.HttpStatusCode.ShouldBe((int)HttpStatusCode.BadGateway);
        content.Disposed.ShouldBeTrue();
    }

    [Fact]
    public async Task Successful_Response_With_Json_Return_Should_Dispose_HttpContent()
    {
        var content = new TrackingHttpContent("42");
        _factory.SetStubResponse(() => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = content
        });

        var result = await _controller.IncrementValueAsync(0);

        result.ShouldBe(42);
        content.Disposed.ShouldBeTrue();
    }

    [Fact]
    public async Task Successful_Response_With_Void_Return_Should_Dispose_HttpContent()
    {
        var content = new TrackingHttpContent(string.Empty);
        _factory.SetStubResponse(() => new HttpResponseMessage(HttpStatusCode.NoContent)
        {
            Content = content
        });

        await _controller.GetException1Async();

        content.Disposed.ShouldBeTrue();
    }

    class StubProxyHttpClientFactory : IProxyHttpClientFactory
    {
        private readonly ITestServerAccessor _testServerAccessor;

        private int _count;
        private Func<HttpResponseMessage> _responseFactory;

        public StubProxyHttpClientFactory(ITestServerAccessor testServerAccessor)
        {
            _testServerAccessor = testServerAccessor;
        }

        public void SetStubResponse(Func<HttpResponseMessage> responseFactory)
        {
            _responseFactory = responseFactory;
        }

        public HttpClient Create(string name) => Create();

        public HttpClient Create()
        {
            if (_count++ == 0)
            {
                return _testServerAccessor.Server.CreateClient();
            }

            return new HttpClient(new StubHttpMessageHandler(() =>
                _responseFactory?.Invoke()
                ?? throw new InvalidOperationException("Stub response is not configured.")))
            {
                BaseAddress = new Uri("http://localhost/")
            };
        }
    }

    class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpResponseMessage> _responseFactory;

        public StubHttpMessageHandler(Func<HttpResponseMessage> responseFactory)
        {
            _responseFactory = responseFactory;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_responseFactory());
        }
    }

    class TrackingHttpContent : HttpContent
    {
        private readonly byte[] _data;

        public bool Disposed { get; private set; }

        public TrackingHttpContent(string text)
        {
            _data = Encoding.UTF8.GetBytes(text);
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return stream.WriteAsync(_data, 0, _data.Length);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _data.Length;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            Disposed = true;
            base.Dispose(disposing);
        }
    }
}
