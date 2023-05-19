using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;

namespace Volo.Abp.AspNetCore.Authentication.OpenIdConnect;

public class OpenIdLocalUserCreationClient : IOpenIdLocalUserCreationClient, ITransientDependency
{
    protected OpenIdLocalUserCreationClientOptions Options { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IRemoteServiceConfigurationProvider RemoteServiceConfigurationProvider { get; }

    public OpenIdLocalUserCreationClient(
        IOptions<OpenIdLocalUserCreationClientOptions> options,
        IHttpClientFactory httpClientFactory, 
        IRemoteServiceConfigurationProvider remoteServiceConfigurationProvider)
    {
        HttpClientFactory = httpClientFactory;
        RemoteServiceConfigurationProvider = remoteServiceConfigurationProvider;
        Options = options.Value;
    }

    public virtual async Task CreateOrUpdateAsync(TokenValidatedContext context)
    {
        if (!Options.IsEnabled)
        {
            return;
        }

        using (var httpClient = HttpClientFactory.CreateClient(Options.HttpClientName))
        {
            if (!Options.RemoteServiceName.IsNullOrWhiteSpace())
            {
                var configuration = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync(Options.RemoteServiceName);
                if (configuration.BaseUrl != null)
                {
                    httpClient.BaseAddress = new Uri(configuration.BaseUrl);    
                }
            }

            httpClient.DefaultRequestHeaders.Add(
                HeaderNames.Authorization,
                "Bearer " + context.SecurityToken.RawData
            );
            
            var response = await httpClient.PostAsync(
                Options.Url,
                new StringContent(string.Empty)
            );

            response.EnsureSuccessStatusCode();
        }
    }
}