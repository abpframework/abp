using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Tracing;

namespace Volo.Abp.Http.Client.Proxying
{
    public class HttpProxyExecuter : IHttpProxyExecuter, ITransientDependency
    {
        protected ICancellationTokenProvider CancellationTokenProvider { get; }
        protected ICorrelationIdProvider CorrelationIdProvider { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected AbpCorrelationIdOptions AbpCorrelationIdOptions { get; }
        protected IProxyHttpClientFactory HttpClientFactory { get; }
        protected IRemoteServiceConfigurationProvider RemoteServiceConfigurationProvider { get; }
        protected AbpHttpClientOptions ClientOptions { get; }
        protected IJsonSerializer JsonSerializer { get; }
        protected IRemoteServiceHttpClientAuthenticator ClientAuthenticator { get; }

        public HttpProxyExecuter(
            ICancellationTokenProvider cancellationTokenProvider,
            ICorrelationIdProvider correlationIdProvider,
            ICurrentTenant currentTenant,
            IOptions<AbpCorrelationIdOptions> abpCorrelationIdOptions,
            IProxyHttpClientFactory httpClientFactory,
            IRemoteServiceConfigurationProvider remoteServiceConfigurationProvider,
            IOptions<AbpHttpClientOptions> clientOptions,
            IRemoteServiceHttpClientAuthenticator clientAuthenticator,
            IJsonSerializer jsonSerializer)
        {
            CancellationTokenProvider = cancellationTokenProvider;
            CorrelationIdProvider = correlationIdProvider;
            CurrentTenant = currentTenant;
            AbpCorrelationIdOptions = abpCorrelationIdOptions.Value;
            HttpClientFactory = httpClientFactory;
            RemoteServiceConfigurationProvider = remoteServiceConfigurationProvider;
            ClientOptions = clientOptions.Value;
            ClientAuthenticator = clientAuthenticator;
            JsonSerializer = jsonSerializer;
        }

        public virtual async Task<T> MakeRequestAndGetResultAsync<T>(HttpProxyExecuterContext context)
        {
            var responseContent = await MakeRequestAsync(context);

            if (typeof(T) == typeof(IRemoteStreamContent) ||
                typeof(T) == typeof(RemoteStreamContent))
            {
                /* returning a class that holds a reference to response
                 * content just to be sure that GC does not dispose of
                 * it before we finish doing our work with the stream */
                return (T)(object)new RemoteStreamContent(await responseContent.ReadAsStreamAsync())
                {
                    ContentType = responseContent.Headers.ContentType?.ToString(),
                    FileName = responseContent.Headers?.ContentDisposition?.FileNameStar ??
                               RemoveQuotes(responseContent.Headers?.ContentDisposition?.FileName).ToString()
                };
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

        public virtual async Task<HttpContent> MakeRequestAsync(HttpProxyExecuterContext context)
        {
            var clientConfig = ClientOptions.HttpClientProxies.GetOrDefault(context.ServiceType) ?? throw new AbpException($"Could not get HttpClientProxyConfig for {context.ServiceType.FullName}.");
            var remoteServiceConfig = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync(clientConfig.RemoteServiceName);

            var client = HttpClientFactory.Create(clientConfig.RemoteServiceName);

            var apiVersion = await GetApiVersionInfoAsync(context);
            var url = remoteServiceConfig.BaseUrl.EnsureEndsWith('/') + UrlBuilder.GenerateUrlWithParameters(context.Action, context.Arguments, apiVersion);

            var requestMessage = new HttpRequestMessage(context.Action.GetHttpMethod(), url)
            {
                Content = RequestPayloadBuilder.BuildContent(context.Action, context.Arguments, JsonSerializer, apiVersion)
            };

            AddHeaders(context.Arguments, context.Action, requestMessage, apiVersion);

            if (context.Action.AllowAnonymous != true)
            {
                await ClientAuthenticator.Authenticate(
                    new RemoteServiceHttpClientAuthenticateContext(
                        client,
                        requestMessage,
                        remoteServiceConfig,
                        clientConfig.RemoteServiceName
                    )
                );
            }

            var response = await client.SendAsync(
                requestMessage,
                HttpCompletionOption.ResponseHeadersRead /*this will buffer only the headers, the content will be used as a stream*/,
                GetCancellationToken(context.Arguments)
            );

            if (!response.IsSuccessStatusCode)
            {
                await ThrowExceptionForResponseAsync(response);
            }

            return response.Content;
        }

        private async Task<ApiVersionInfo> GetApiVersionInfoAsync(HttpProxyExecuterContext context)
        {
            var apiVersion = await FindBestApiVersionAsync(context);

            //TODO: Make names configurable?
            var versionParam = context.Action.Parameters.FirstOrDefault(p => p.Name == "apiVersion" && p.BindingSourceId == ParameterBindingSources.Path) ??
                               context.Action.Parameters.FirstOrDefault(p => p.Name == "api-version" && p.BindingSourceId == ParameterBindingSources.Query);

            return new ApiVersionInfo(versionParam?.BindingSourceId, apiVersion);
        }

        private async Task<string> FindBestApiVersionAsync(HttpProxyExecuterContext context)
        {
            var configuredVersion = await GetConfiguredApiVersionAsync(context);

            if (context.Action.SupportedVersions.IsNullOrEmpty())
            {
                return configuredVersion ?? "1.0";
            }

            if (context.Action.SupportedVersions.Contains(configuredVersion))
            {
                return configuredVersion;
            }

            return context.Action.SupportedVersions.Last(); //TODO: Ensure to get the latest version!
        }

        private async Task<string> GetConfiguredApiVersionAsync(HttpProxyExecuterContext context)
        {
            var clientConfig = ClientOptions.HttpClientProxies.GetOrDefault(context.ServiceType)
                               ?? throw new AbpException($"Could not get DynamicHttpClientProxyConfig for {context.ServiceType.FullName}.");

            return (await RemoteServiceConfigurationProvider
                .GetConfigurationOrDefaultOrNullAsync(clientConfig.RemoteServiceName))?.Version;
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

        protected virtual void AddHeaders(
            IReadOnlyDictionary<string, object> argumentsDictionary,
            ActionApiDescriptionModel action,
            HttpRequestMessage requestMessage,
            ApiVersionInfo apiVersion)
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
                var value = HttpActionParameterHelper.FindParameterValue(argumentsDictionary, headerParameter);
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

        protected virtual StringSegment RemoveQuotes(StringSegment input)
        {
            if (!StringSegment.IsNullOrEmpty(input) && input.Length >= 2 && input[0] == '"' && input[input.Length - 1] == '"')
            {
                input = input.Subsegment(1, input.Length - 2);
            }

            return input;
        }

        protected virtual CancellationToken GetCancellationToken(IReadOnlyDictionary<string, object> arguments)
        {
            var cancellationTokenArg = arguments.LastOrDefault();

            if (cancellationTokenArg.Value is CancellationToken cancellationToken)
            {
                if (cancellationToken != default)
                {
                    return cancellationToken;
                }
            }

            return CancellationTokenProvider.Token;
        }
    }
}
