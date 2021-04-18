using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Fody;
using Volo.Abp.AspNetCore.Components.Web.Extensibility;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.Http.Client;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Extensibility
{
    public class WebAssemblyLookupApiRequestService : ILookupApiRequestService, ITransientDependency
    {
        public IHttpClientFactory HttpClientFactory { get; }
        public IRemoteServiceHttpClientAuthenticator HttpClientAuthenticator { get; }

        public AbpRemoteServiceOptions RemoteServiceOptions { get; }

        public ICurrentTenant CurrentTenant { get; }

        public WebAssemblyLookupApiRequestService(IHttpClientFactory httpClientFactory,
            IRemoteServiceHttpClientAuthenticator httpClientAuthenticator,
            ICurrentTenant currentTenant,
            IOptions<AbpRemoteServiceOptions> remoteServiceOptions)
        {
            HttpClientFactory = httpClientFactory;
            HttpClientAuthenticator = httpClientAuthenticator;
            RemoteServiceOptions = remoteServiceOptions.Value;
            CurrentTenant = currentTenant;
        }

        public async Task<string> SendAsync(string url)
        {
            var client = HttpClientFactory.CreateClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            AddHeaders(requestMessage);

            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            if (!uri.IsAbsoluteUri)
            {
                var remoteServiceConfig = RemoteServiceOptions.RemoteServices.GetConfigurationOrDefault("Default");
                client.BaseAddress = new Uri(remoteServiceConfig.BaseUrl);
                await HttpClientAuthenticator.Authenticate(new RemoteServiceHttpClientAuthenticateContext(client, requestMessage, new RemoteServiceConfiguration(remoteServiceConfig.BaseUrl), string.Empty));
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
                requestMessage.Headers.AcceptLanguage.Add(new (currentCulture));
            }
        }
    }
}