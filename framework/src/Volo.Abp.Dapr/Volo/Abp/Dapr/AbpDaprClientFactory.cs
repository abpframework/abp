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

    public virtual Task<DaprClient> CreateAsync(Action<DaprClientBuilder>? builderAction = null)
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

        builderAction?.Invoke(builder);

        return Task.FromResult(builder.Build());
    }

    public Task<HttpClient> CreateHttpClientAsync(
        string? appId = null,
        string? daprEndpoint = null,
        string? daprApiToken = null)
    {
        if(daprEndpoint.IsNullOrWhiteSpace() &&
           !DaprOptions.HttpEndpoint.IsNullOrWhiteSpace())
        {
            daprEndpoint = DaprOptions.HttpEndpoint;
        }
        
        return Task.FromResult(DaprClient.CreateInvokeHttpClient(appId, daprEndpoint, daprApiToken));
    }
}