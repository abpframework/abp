using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Components.Web.Extensibility;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.Components.Server.Extensibility
{
    public class BlazorServerLookupApiRequestService : ILookupApiRequestService, ITransientDependency
    {
        public IHttpClientFactory HttpClientFactory { get; }
        public IRemoteServiceHttpClientAuthenticator HttpClientAuthenticator { get; }

        public AbpRemoteServiceOptions RemoteServiceOptions { get; }

        public ICurrentTenant CurrentTenant { get; }
        public NavigationManager NavigationManager { get; }

        public BlazorServerLookupApiRequestService(IHttpClientFactory httpClientFactory,
            IRemoteServiceHttpClientAuthenticator httpClientAuthenticator,
            ICurrentTenant currentTenant,
            IOptions<AbpRemoteServiceOptions> remoteServiceOptions,
            NavigationManager navigationManager)
        {
            HttpClientFactory = httpClientFactory;
            HttpClientAuthenticator = httpClientAuthenticator;
            RemoteServiceOptions = remoteServiceOptions.Value;
            CurrentTenant = currentTenant;
            NavigationManager = navigationManager;
        }

        public async Task<string> SendAsync(string url)
        {
            var client = HttpClientFactory.CreateClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            AddHeaders(requestMessage);

            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            if (!uri.IsAbsoluteUri)
            {
                var baseUrl = string.Empty;
                try
                {
                    var remoteServiceConfig = RemoteServiceOptions.RemoteServices.GetConfigurationOrDefault("Default");
                    baseUrl = remoteServiceConfig.BaseUrl;
                }
                catch (AbpException)
                {
                    baseUrl = NavigationManager.BaseUri;
                }

                client.BaseAddress = new Uri(baseUrl);
                await HttpClientAuthenticator.Authenticate(new RemoteServiceHttpClientAuthenticateContext(client,
                    requestMessage, new RemoteServiceConfiguration(baseUrl), string.Empty));
            }

            var response = await client.SendAsync(requestMessage);

            return await response.Content.ReadAsStringAsync();
        }

        protected virtual void AddHeaders(HttpRequestMessage requestMessage)
        {
            if (CurrentTenant.Id.HasValue)
            {
                requestMessage.Headers.Add(TenantResolverConsts.DefaultTenantKey, CurrentTenant.Id.Value.ToString());
            }

            var currentCulture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if (!currentCulture.IsNullOrEmpty())
            {
                requestMessage.Headers.AcceptLanguage.Add(new(currentCulture));
            }
        }
    }
}