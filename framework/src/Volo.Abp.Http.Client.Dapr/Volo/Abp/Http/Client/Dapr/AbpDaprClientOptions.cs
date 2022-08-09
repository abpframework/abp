using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Http.Client.Dapr;

public class AbpDaprClientOptions
{
    public Action<string, IHttpClientBuilder> DaprHttpClientBuilderAction { get; set; }

    public AbpDaprClientOptions()
    {
        DaprHttpClientBuilderAction = DefaultDaprHttpClientBuilder;
    }
    
    private void DefaultDaprHttpClientBuilder(string serviceName, IHttpClientBuilder clientBuilder)
    {
        clientBuilder.AddHttpMessageHandler<AbpInvocationHandler>();
    }
}