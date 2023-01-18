using System;
using System.Net.Http;
using Dapr.Client;

namespace Volo.Abp.Dapr;

public interface IAbpDaprClientFactory
{
    DaprClient Create(Action<DaprClientBuilder> builderAction = null);

    HttpClient CreateHttpClient(
        string appId = null,
        string daprEndpoint = null,
        string daprApiToken = null
    );
}
