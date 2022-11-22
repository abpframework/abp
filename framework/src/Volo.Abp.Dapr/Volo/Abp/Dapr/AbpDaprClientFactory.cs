using System;
using System.Net.Http;
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
    protected IDaprApiTokenProvider DaprApiTokenProvider { get; }

    public AbpDaprClientFactory(
        IOptions<AbpDaprOptions> options,
        IOptions<AbpSystemTextJsonSerializerOptions> systemTextJsonSerializerOptions,
        IDaprApiTokenProvider daprApiTokenProvider)
    {
        DaprApiTokenProvider = daprApiTokenProvider;
        DaprOptions = options.Value;
        JsonSerializerOptions = CreateJsonSerializerOptions(systemTextJsonSerializerOptions.Value);
    }

    public virtual DaprClient Create(Action<DaprClientBuilder> builderAction = null)
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

        var apiToken = DaprApiTokenProvider.GetDaprApiToken();
        if (!apiToken.IsNullOrWhiteSpace())
        {
            builder.UseDaprApiToken(apiToken);
        }

        builderAction?.Invoke(builder);

        return builder.Build();
    }

    public virtual HttpClient CreateHttpClient(
        string appId = null,
        string daprEndpoint = null,
        string daprApiToken = null)
    {
        if(daprEndpoint.IsNullOrWhiteSpace() &&
           !DaprOptions.HttpEndpoint.IsNullOrWhiteSpace())
        {
            daprEndpoint = DaprOptions.HttpEndpoint;
        }

        return DaprClient.CreateInvokeHttpClient(
            appId,
            daprEndpoint,
            daprApiToken ?? DaprApiTokenProvider.GetDaprApiToken()
        );
    }

    protected virtual JsonSerializerOptions CreateJsonSerializerOptions(AbpSystemTextJsonSerializerOptions systemTextJsonSerializerOptions)
    {
        return new JsonSerializerOptions(systemTextJsonSerializerOptions.JsonSerializerOptions);
    }
}
