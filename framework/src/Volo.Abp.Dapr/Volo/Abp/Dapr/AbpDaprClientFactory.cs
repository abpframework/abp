using Dapr.Client;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Dapr;

public class AbpDaprClientFactory : ITransientDependency
{
    protected AbpDaprOptions Options { get; }

    public AbpDaprClientFactory(IOptions<AbpDaprOptions> options)
    {
        Options = options.Value;
    }

    public virtual Task<DaprClient> CreateAsync()
    {
        var daprClient = new DaprClientBuilder()
            .UseHttpEndpoint(Options.HttpEndpoint)
            //.UseJsonSerializationOptions()//TODO: Use abp JsonSerializerOptions
            .Build();

        return Task.FromResult(daprClient);
    }
}
