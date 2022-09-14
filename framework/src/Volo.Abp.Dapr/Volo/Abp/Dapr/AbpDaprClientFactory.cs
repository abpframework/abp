using System.Text.Json;
using Dapr.Client;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json.SystemTextJson;

namespace Volo.Abp.Dapr;

public class AbpDaprClientFactory : IAbpDaprClientFactory, ISingletonDependency
{
    protected AbpDaprOptions DaprOptions { get; }
    protected JsonSerializerOptions JsonSerializerOptions { get; }

    public AbpDaprClientFactory(
        IOptions<AbpDaprOptions> options,
        IOptions<AbpSystemTextJsonSerializerOptions> systemTextJsonSerializerOptions)
    {
        DaprOptions = options.Value;
        JsonSerializerOptions = CreateJsonSerializerOptions(systemTextJsonSerializerOptions.Value);
    }

    protected virtual JsonSerializerOptions CreateJsonSerializerOptions(AbpSystemTextJsonSerializerOptions systemTextJsonSerializerOptions)
    {
        return new JsonSerializerOptions(systemTextJsonSerializerOptions.JsonSerializerOptions);
    }

    public virtual Task<DaprClient> CreateAsync()
    {
        var builder = new DaprClientBuilder()
            .UseJsonSerializationOptions(JsonSerializerOptions);

        if (!DaprOptions.HttpEndpoint.IsNullOrWhiteSpace())
        {
            builder.UseHttpEndpoint(DaprOptions.HttpEndpoint);
        }

        if (!DaprOptions.GrpcEndpoint.IsNullOrWhiteSpace())
        {
            builder.UseGrpcEndpoint(DaprOptions.GrpcEndpoint);
        }

        return Task.FromResult(builder.Build());
    }
}