using Dapr.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Http.Client.Dapr;

public class AbpDaprClientOptions
{
    public Action<string, IHttpClientBuilder, string> DaprHttpClientBuilderAction { get; set; }

    public AbpDaprClientOptions()
    {
        DaprHttpClientBuilderAction = DefaultDaprHttpClientBuilder;
    }
    
    private void DefaultDaprHttpClientBuilder(string serviceName, IHttpClientBuilder clientBuilder, string daprEndpoint)
    {
        Check.NotNull(daprEndpoint, nameof(daprEndpoint));
        
        clientBuilder.AddHttpMessageHandler(() => new InvocationHandler
        {
            DaprEndpoint = daprEndpoint
        });
    }
}