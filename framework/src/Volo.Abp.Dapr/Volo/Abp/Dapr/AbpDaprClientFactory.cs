using System.Collections.Concurrent;
using System.Text.Json;
using Dapr.Client;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json.SystemTextJson;

namespace Volo.Abp.Dapr;

public class AbpDaprClientFactory : ITransientDependency
{
    protected AbpDaprOptions Options { get; }
    protected AbpSystemTextJsonSerializerOptions SystemTextJsonSerializerOptions { get; }

    public AbpDaprClientFactory(
        IOptions<AbpDaprOptions> options,
        IOptions<AbpSystemTextJsonSerializerOptions> systemTextJsonSerializerOptions)
    {
        Options = options.Value;
        SystemTextJsonSerializerOptions = systemTextJsonSerializerOptions.Value;
    }

    public virtual async Task<DaprClient> CreateAsync()
    {
        var builder = new DaprClientBuilder()
            .UseJsonSerializationOptions(await CreateJsonSerializerOptions());

        if (!Options.HttpEndpoint.IsNullOrWhiteSpace())
        {
            builder.UseHttpEndpoint(Options.HttpEndpoint);
        }

        if (!Options.GrpcEndpoint.IsNullOrWhiteSpace())
        {
            builder.UseGrpcEndpoint(Options.GrpcEndpoint);
        }

        return builder.Build();
    }

    private readonly static ConcurrentDictionary<string, JsonSerializerOptions> JsonSerializerOptionsCache = new ConcurrentDictionary<string, JsonSerializerOptions>();

    protected virtual Task<JsonSerializerOptions> CreateJsonSerializerOptions()
    {
        return Task.FromResult(JsonSerializerOptionsCache.GetOrAdd(nameof(AbpDaprClientFactory),
            _ => new JsonSerializerOptions(SystemTextJsonSerializerOptions.JsonSerializerOptions)));
    }
}
