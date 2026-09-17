using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection;
internal static class MinioHttpClientFactoryServiceCollectionExtensions
{
    private const string HttpClientName = "__MinioApiClient";
    public static IServiceCollection AddMinioHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient(HttpClientName);

        return services;
    }

    public static HttpClient CreateMinioHttpClient(this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient(HttpClientName);
    }
}
