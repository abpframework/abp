using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Http.Client.DynamicProxying;
using Xunit;

namespace Volo.Abp.Http.DynamicProxying;

public class AsyncEnumerableClientProxy_RequestOverride_Tests : AbpHttpClientTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient<DynamicHttpProxyInterceptorClientProxy<IRegularTestController>, TrackingClientProxy>());
    }

    [Fact]
    public async Task Should_Use_Overridden_RequestAsync_For_Streams()
    {
        TrackingClientProxy.RequestedActions.Clear();
        var controller = ServiceProvider.GetRequiredService<IRegularTestController>();

        var items = new List<int>();
        await foreach (var item in controller.GetStreamAsync(2))
        {
            items.Add(item);
        }

        items.ShouldBe(new[] { 1, 2 });
        TrackingClientProxy.RequestedActions.ShouldContain(nameof(IRegularTestController.GetStreamAsync));
    }

    [Fact]
    public async Task Should_Pass_The_Same_Request_Context_To_RequestAsync()
    {
        TrackingClientProxy.RequestContexts.Clear();
        var controller = ServiceProvider.GetRequiredService<IRegularTestController>();
        await foreach (var _ in controller.GetStreamAsync(1))
        {
        }

        var capturedContext = TrackingClientProxy.RequestContexts.Last();
        var requestContext = new CustomRequestContext(capturedContext.Action, capturedContext.Arguments, capturedContext.ServiceType);

        var clientProxy = ServiceProvider.GetRequiredService<DynamicHttpProxyInterceptorClientProxy<IRegularTestController>>();
        var items = new List<int>();
        await foreach (var item in clientProxy.CallRequestAsyncEnumerable<int>(requestContext))
        {
            items.Add(item);
        }

        items.ShouldBe(new[] { 1 });
        TrackingClientProxy.RequestContexts.Last().ShouldBeSameAs(requestContext);
        requestContext.CancellationToken.ShouldBeNull();
    }

    public class CustomRequestContext : ClientProxyRequestContext
    {
        public CustomRequestContext(Volo.Abp.Http.Modeling.ActionApiDescriptionModel action, IReadOnlyDictionary<string, object?> arguments, System.Type serviceType)
            : base(action, arguments, serviceType)
        {
        }
    }

    public class TrackingClientProxy : DynamicHttpProxyInterceptorClientProxy<IRegularTestController>
    {
        public static List<string> RequestedActions { get; } = new();

        public static List<ClientProxyRequestContext> RequestContexts { get; } = new();

        protected override Task<HttpContent> RequestAsync(ClientProxyRequestContext requestContext)
        {
            RequestedActions.Add(requestContext.Action.Name);
            RequestContexts.Add(requestContext);
            return base.RequestAsync(requestContext);
        }
    }
}
