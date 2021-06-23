using System;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
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
        public IRemoteServiceConfigurationProvider RemoteServiceConfigurationProvider { get; }
        public ICurrentTenant CurrentTenant { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public NavigationManager NavigationManager { get; }

        public BlazorServerLookupApiRequestService(IHttpClientFactory httpClientFactory,
            IRemoteServiceHttpClientAuthenticator httpClientAuthenticator,
            ICurrentTenant currentTenant,
            IHttpContextAccessor httpContextAccessor,
            NavigationManager navigationManager,
            IRemoteServiceConfigurationProvider remoteServiceConfigurationProvider)
        {
            HttpClientFactory = httpClientFactory;
            HttpClientAuthenticator = httpClientAuthenticator;
            CurrentTenant = currentTenant;
            HttpContextAccessor = httpContextAccessor;
            NavigationManager = navigationManager;
            RemoteServiceConfigurationProvider = remoteServiceConfigurationProvider;
        }

        public async Task<string> SendAsync(string url)
        {
            var client = HttpClientFactory.CreateClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            if (!uri.IsAbsoluteUri)
            {
                var baseUrl = string.Empty;
                try
                {
                    //Blazor tiered -- mode
                    var remoteServiceConfig = RemoteServiceConfigurationProvider.GetConfigurationOrDefault("Default");
                    baseUrl = remoteServiceConfig.BaseUrl;
                    client.BaseAddress = new Uri(baseUrl);
                    AddHeaders(requestMessage);
                    await HttpClientAuthenticator.Authenticate(new RemoteServiceHttpClientAuthenticateContext(client,
                        requestMessage, new RemoteServiceConfiguration(baseUrl), string.Empty));
                }
                catch (AbpException) // Blazor-Server mode.
                {
                    baseUrl = NavigationManager.BaseUri;
                    client.BaseAddress = new Uri(baseUrl);
                    foreach (var header in HttpContextAccessor.HttpContext.Request.Headers)
                    {
                        requestMessage.Headers.Add(header.Key, header.Value.ToArray());
                    }
                }
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