using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.Http.Client.Content;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Tracing;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public class DynamicHttpProxyInterceptor<TService> : AbpInterceptor, ITransientDependency
    {
        // ReSharper disable once StaticMemberInGenericType
        protected static MethodInfo MakeRequestAndGetResultAsyncMethod { get; }

        protected ICancellationTokenProvider CancellationTokenProvider { get; }
        protected ICorrelationIdProvider CorrelationIdProvider { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected AbpCorrelationIdOptions AbpCorrelationIdOptions { get; }
        protected IDynamicProxyHttpClientFactory HttpClientFactory { get; }
        protected IApiDescriptionFinder ApiDescriptionFinder { get; }
        protected AbpRemoteServiceOptions AbpRemoteServiceOptions { get; }
        protected AbpHttpClientOptions ClientOptions { get; }
        protected IJsonSerializer JsonSerializer { get; }
        protected IRemoteServiceHttpClientAuthenticator ClientAuthenticator { get; }

        public ILogger<DynamicHttpProxyInterceptor<TService>> Logger { get; set; }

        static DynamicHttpProxyInterceptor()
        {
            MakeRequestAndGetResultAsyncMethod = typeof(DynamicHttpProxyInterceptor<TService>)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(m => m.Name == nameof(MakeRequestAndGetResultAsync) && m.IsGenericMethodDefinition);
        }

        public DynamicHttpProxyInterceptor(
            IDynamicProxyHttpClientFactory httpClientFactory,
            IOptions<AbpHttpClientOptions> clientOptions,
            IOptionsSnapshot<AbpRemoteServiceOptions> remoteServiceOptions,
            IApiDescriptionFinder apiDescriptionFinder,
            IJsonSerializer jsonSerializer,
            IRemoteServiceHttpClientAuthenticator clientAuthenticator,
            ICancellationTokenProvider cancellationTokenProvider,
            ICorrelationIdProvider correlationIdProvider,
            IOptions<AbpCorrelationIdOptions> correlationIdOptions,
            ICurrentTenant currentTenant)
        {
            CancellationTokenProvider = cancellationTokenProvider;
            CorrelationIdProvider = correlationIdProvider;
            CurrentTenant = currentTenant;
            AbpCorrelationIdOptions = correlationIdOptions.Value;
            HttpClientFactory = httpClientFactory;
            ApiDescriptionFinder = apiDescriptionFinder;
            JsonSerializer = jsonSerializer;
            ClientAuthenticator = clientAuthenticator;
            ClientOptions = clientOptions.Value;
            AbpRemoteServiceOptions = remoteServiceOptions.Value;

            Logger = NullLogger<DynamicHttpProxyInterceptor<TService>>.Instance;
        }

        public async override Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            if (invocation.Method.ReturnType.GenericTypeArguments.IsNullOrEmpty())
            {
                await MakeRequestAsync(invocation);
            }
            else
            {
                var result = (Task)MakeRequestAndGetResultAsyncMethod
                    .MakeGenericMethod(invocation.Method.ReturnType.GenericTypeArguments[0])
                    .Invoke(this, new object[] { invocation });

                invocation.ReturnValue = await GetResultAsync(
                    result,
                    invocation.Method.ReturnType.GetGenericArguments()[0]
                );
            }
        }

        private async Task<object> GetResultAsync(Task task, Type resultType)
        {
            await task;
            return typeof(Task<>)
                .MakeGenericType(resultType)
                .GetProperty(nameof(Task<object>.Result), BindingFlags.Instance | BindingFlags.Public)
                .GetValue(task);
        }

        private async Task<T> MakeRequestAndGetResultAsync<T>(IAbpMethodInvocation invocation)
        {
            var responseContent = await MakeRequestAsync(invocation);

            if (typeof(T) == typeof(IRemoteStreamContent))
            {
                /* returning a class that holds a reference to response
                 * content just to be sure that GC does not dispose of
                 * it before we finish doing our work with the stream */
                return (T)((object)new ReferencedRemoteStreamContent(await responseContent.ReadAsStreamAsync(), responseContent));
            }

            var stringContent = await responseContent.ReadAsStringAsync();
            if (typeof(T) == typeof(string))
            {
                return (T)(object)stringContent;
            }

            if (stringContent.IsNullOrWhiteSpace())
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(stringContent);
        }

        private async Task<HttpContent> MakeRequestAsync(IAbpMethodInvocation invocation)
        {
            var clientConfig = ClientOptions.HttpClientProxies.GetOrDefault(typeof(TService)) ?? throw new AbpException($"Could not get DynamicHttpClientProxyConfig for {typeof(TService).FullName}.");
            var remoteServiceConfig = AbpRemoteServiceOptions.RemoteServices.GetConfigurationOrDefault(clientConfig.RemoteServiceName);

            var client = HttpClientFactory.Create(clientConfig.RemoteServiceName);

            var action = await ApiDescriptionFinder.FindActionAsync(client, remoteServiceConfig.BaseUrl, typeof(TService), invocation.Method);
            var apiVersion = GetApiVersionInfo(action);
            var url = remoteServiceConfig.BaseUrl.EnsureEndsWith('/') + UrlBuilder.GenerateUrlWithParameters(action, invocation.ArgumentsDictionary, apiVersion);

            var requestMessage = new HttpRequestMessage(action.GetHttpMethod(), url)
            {
                Content = RequestPayloadBuilder.BuildContent(action, invocation.ArgumentsDictionary, JsonSerializer, apiVersion)
            };

            AddHeaders(invocation, action, requestMessage, apiVersion);

            await ClientAuthenticator.Authenticate(
                new RemoteServiceHttpClientAuthenticateContext(
                    client,
                    requestMessage,
                    remoteServiceConfig,
                    clientConfig.RemoteServiceName
                )
            );

            var response = await client.SendAsync(requestMessage,
                HttpCompletionOption.ResponseHeadersRead /*this will buffer only the headers, the content will be used as a stream*/,
                GetCancellationToken());

            if (!response.IsSuccessStatusCode)
            {
                await ThrowExceptionForResponseAsync(response);
            }

            return response.Content;
        }

        private ApiVersionInfo GetApiVersionInfo(ActionApiDescriptionModel action)
        {
            var apiVersion = FindBestApiVersion(action);

            //TODO: Make names configurable?
            var versionParam = action.Parameters.FirstOrDefault(p => p.Name == "apiVersion" && p.BindingSourceId == ParameterBindingSources.Path) ??
                               action.Parameters.FirstOrDefault(p => p.Name == "api-version" && p.BindingSourceId == ParameterBindingSources.Query);

            return new ApiVersionInfo(versionParam?.BindingSourceId, apiVersion);
        }

        private string FindBestApiVersion(ActionApiDescriptionModel action)
        {
            var configuredVersion = GetConfiguredApiVersion();

            if (action.SupportedVersions.IsNullOrEmpty())
            {
                return configuredVersion ?? "1.0";
            }

            if (action.SupportedVersions.Contains(configuredVersion))
            {
                return configuredVersion;
            }

            return action.SupportedVersions.Last(); //TODO: Ensure to get the latest version!
        }

        protected virtual void AddHeaders(IAbpMethodInvocation invocation, ActionApiDescriptionModel action, HttpRequestMessage requestMessage, ApiVersionInfo apiVersion)
        {
            //API Version
            if (!apiVersion.Version.IsNullOrEmpty())
            {
                //TODO: What about other media types?
                requestMessage.Headers.Add("accept", $"{MimeTypes.Text.Plain}; v={apiVersion.Version}");
                requestMessage.Headers.Add("accept", $"{MimeTypes.Application.Json}; v={apiVersion.Version}");
                requestMessage.Headers.Add("api-version", apiVersion.Version);
            }

            //Header parameters
            var headers = action.Parameters.Where(p => p.BindingSourceId == ParameterBindingSources.Header).ToArray();
            foreach (var headerParameter in headers)
            {
                var value = HttpActionParameterHelper.FindParameterValue(invocation.ArgumentsDictionary, headerParameter);
                if (value != null)
                {
                    requestMessage.Headers.Add(headerParameter.Name, value.ToString());
                }
            }

            //CorrelationId
            requestMessage.Headers.Add(AbpCorrelationIdOptions.HttpHeaderName, CorrelationIdProvider.Get());

            //TenantId
            if (CurrentTenant.Id.HasValue)
            {
                //TODO: Use AbpAspNetCoreMultiTenancyOptions to get the key
                requestMessage.Headers.Add(TenantResolverConsts.DefaultTenantKey, CurrentTenant.Id.Value.ToString());
            }

            //Culture
            //TODO: Is that the way we want? Couldn't send the culture (not ui culture)
            var currentCulture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if (!currentCulture.IsNullOrEmpty())
            {
                requestMessage.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(currentCulture));
            }

            //X-Requested-With
            requestMessage.Headers.Add("X-Requested-With", "XMLHttpRequest");
        }

        private string GetConfiguredApiVersion()
        {
            var clientConfig = ClientOptions.HttpClientProxies.GetOrDefault(typeof(TService))
                               ?? throw new AbpException($"Could not get DynamicHttpClientProxyConfig for {typeof(TService).FullName}.");

            return AbpRemoteServiceOptions.RemoteServices.GetOrDefault(clientConfig.RemoteServiceName)?.Version
                   ?? AbpRemoteServiceOptions.RemoteServices.Default?.Version;
        }

        private async Task ThrowExceptionForResponseAsync(HttpResponseMessage response)
        {
            if (response.Headers.Contains(AbpHttpConsts.AbpErrorFormat))
            {
                var errorResponse = JsonSerializer.Deserialize<RemoteServiceErrorResponse>(
                    await response.Content.ReadAsStringAsync()
                );

                throw new AbpRemoteCallException(errorResponse.Error)
                {
                    HttpStatusCode = (int) response.StatusCode
                };
            }

            throw new AbpRemoteCallException(
                new RemoteServiceErrorInfo
                {
                    Message = response.ReasonPhrase,
                    Code = response.StatusCode.ToString()
                }
            )
            {
                HttpStatusCode = (int) response.StatusCode
            };
        }

        protected virtual CancellationToken GetCancellationToken()
        {
            return CancellationTokenProvider.Token;
        }
    }
}
