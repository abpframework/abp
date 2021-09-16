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
using Volo.Abp.Http.Client.Proxying;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Tracing;

namespace Volo.Abp.Http.Client.ClientProxying
{
    public class ClientProxyBase<TService> : ITransientDependency
    {
        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

        protected IClientProxyApiDescriptionFinder ClientProxyApiDescriptionFinder => LazyServiceProvider.LazyGetRequiredService<IClientProxyApiDescriptionFinder>();
        protected ICancellationTokenProvider CancellationTokenProvider => LazyServiceProvider.LazyGetRequiredService<ICancellationTokenProvider>();
        protected ICorrelationIdProvider CorrelationIdProvider => LazyServiceProvider.LazyGetRequiredService<ICorrelationIdProvider>();
        protected ICurrentTenant CurrentTenant  => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();
        protected IOptions<AbpCorrelationIdOptions> AbpCorrelationIdOptions  => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpCorrelationIdOptions>>();
        protected IProxyHttpClientFactory HttpClientFactory  => LazyServiceProvider.LazyGetRequiredService<IProxyHttpClientFactory>();
        protected IRemoteServiceConfigurationProvider RemoteServiceConfigurationProvider  => LazyServiceProvider.LazyGetRequiredService<IRemoteServiceConfigurationProvider>();
        protected IOptions<AbpHttpClientOptions> ClientOptions  => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpHttpClientOptions>>();
        protected IJsonSerializer JsonSerializer  => LazyServiceProvider.LazyGetRequiredService<IJsonSerializer>();
        protected IRemoteServiceHttpClientAuthenticator ClientAuthenticator  => LazyServiceProvider.LazyGetRequiredService<IRemoteServiceHttpClientAuthenticator>();
        protected ClientProxyRequestPayloadBuilder ClientProxyRequestPayloadBuilder  => LazyServiceProvider.LazyGetRequiredService<ClientProxyRequestPayloadBuilder>();
        protected ClientProxyUrlBuilder ClientProxyUrlBuilder  => LazyServiceProvider.LazyGetRequiredService<ClientProxyUrlBuilder>();

        protected virtual async Task RequestAsync(string methodName, params object[] arguments)
        {
            await RequestAsync(BuildHttpProxyClientProxyContext(methodName, arguments));
        }

        protected virtual async Task<T> RequestAsync<T>(string methodName, params object[] arguments)
        {
            return await RequestAsync<T>(BuildHttpProxyClientProxyContext(methodName, arguments));
        }

        protected virtual ClientProxyRequestContext BuildHttpProxyClientProxyContext(string methodName, params object[] arguments)
        {
            var methodUniqueName = $"{typeof(TService).FullName}.{methodName}.{string.Join("-", arguments.Select(x => x.GetType().FullName))}";
            var action = ClientProxyApiDescriptionFinder.FindAction(methodUniqueName);
            return new ClientProxyRequestContext(action, BuildActionArguments(action, arguments), typeof(TService));
        }

        protected virtual Dictionary<string, object> BuildActionArguments(ActionApiDescriptionModel action, object[] arguments)
        {
            var parameters = action.Parameters.GroupBy(x => x.NameOnMethod).Select(x => x.Key).ToList();
            var dict = new Dictionary<string, object>();

            for (var i = 0; i < parameters.Count; i++)
            {
                dict[parameters[i]] = arguments[i];
            }

            return dict;
        }

        public virtual async Task<T> RequestAsync<T>(ClientProxyRequestContext requestContext)
        {
            var responseContent = await RequestAsync(requestContext);

             if (typeof(T) == typeof(IRemoteStreamContent) ||
                 typeof(T) == typeof(RemoteStreamContent))
            {
                /* returning a class that holds a reference to response
                 * content just to be sure that GC does not dispose of
                 * it before we finish doing our work with the stream */
                return (T)(object)new RemoteStreamContent(
                    await responseContent.ReadAsStreamAsync(),
                    responseContent.Headers?.ContentDisposition?.FileNameStar ??
                    RemoveQuotes(responseContent.Headers?.ContentDisposition?.FileName).ToString(),
                    responseContent.Headers?.ContentType?.ToString(),
                    responseContent.Headers?.ContentLength);
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

        public virtual async Task<HttpContent> RequestAsync(ClientProxyRequestContext requestContext)
        {
            var clientConfig = ClientOptions.Value.HttpClientProxies.GetOrDefault(requestContext.ServiceType) ?? throw new AbpException($"Could not get HttpClientProxyConfig for {requestContext.ServiceType.FullName}.");
            var remoteServiceConfig = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync(clientConfig.RemoteServiceName);

            var client = HttpClientFactory.Create(clientConfig.RemoteServiceName);

            var apiVersion = await GetApiVersionInfoAsync(requestContext);
            var url = remoteServiceConfig.BaseUrl.EnsureEndsWith('/') + await GetUrlWithParametersAsync(requestContext, apiVersion);

            var requestMessage = new HttpRequestMessage(requestContext.Action.GetHttpMethod(), url)
            {
                Content = ClientProxyRequestPayloadBuilder.BuildContent(requestContext.Action, requestContext.Arguments, JsonSerializer, apiVersion)
            };

            AddHeaders(requestContext.Arguments, requestContext.Action, requestMessage, apiVersion);

            if (requestContext.Action.AllowAnonymous != true)
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
                GetCancellationToken(requestContext.Arguments)
            );

            if (!response.IsSuccessStatusCode)
            {
                await ThrowExceptionForResponseAsync(response);
            }

            return response.Content;
        }

        protected virtual async Task<ApiVersionInfo> GetApiVersionInfoAsync(ClientProxyRequestContext requestContext)
        {
            var apiVersion = await FindBestApiVersionAsync(requestContext);

            //TODO: Make names configurable?
            var versionParam = requestContext.Action.Parameters.FirstOrDefault(p => p.Name == "apiVersion" && p.BindingSourceId == ParameterBindingSources.Path) ??
                               requestContext.Action.Parameters.FirstOrDefault(p => p.Name == "api-version" && p.BindingSourceId == ParameterBindingSources.Query);

            return new ApiVersionInfo(versionParam?.BindingSourceId, apiVersion);
        }

        protected virtual Task<string> GetUrlWithParametersAsync(ClientProxyRequestContext requestContext, ApiVersionInfo apiVersion)
        {
            return Task.FromResult(ClientProxyUrlBuilder.GenerateUrlWithParameters(requestContext.Action, requestContext.Arguments, apiVersion));
        }

        protected virtual Task<HttpContent> GetHttpContentAsync(ClientProxyRequestContext requestContext, ApiVersionInfo apiVersion)
        {
            return Task.FromResult(ClientProxyRequestPayloadBuilder.BuildContent(requestContext.Action, requestContext.Arguments, JsonSerializer, apiVersion));
        }

        protected virtual async Task<string> FindBestApiVersionAsync(ClientProxyRequestContext requestContext)
        {
            var configuredVersion = await GetConfiguredApiVersionAsync(requestContext);

            if (requestContext.Action.SupportedVersions.IsNullOrEmpty())
            {
                return configuredVersion ?? "1.0";
            }

            if (requestContext.Action.SupportedVersions.Contains(configuredVersion))
            {
                return configuredVersion;
            }

            return requestContext.Action.SupportedVersions.Last(); //TODO: Ensure to get the latest version!
        }

        protected virtual async Task<string> GetConfiguredApiVersionAsync(ClientProxyRequestContext requestContext)
        {
            var clientConfig = ClientOptions.Value.HttpClientProxies.GetOrDefault(requestContext.ServiceType)
                               ?? throw new AbpException($"Could not get DynamicHttpClientProxyConfig for {requestContext.ServiceType.FullName}.");

            return (await RemoteServiceConfigurationProvider
                .GetConfigurationOrDefaultOrNullAsync(clientConfig.RemoteServiceName))?.Version;
        }

        protected virtual async Task ThrowExceptionForResponseAsync(HttpResponseMessage response)
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
            requestMessage.Headers.Add(AbpCorrelationIdOptions.Value.HttpHeaderName, CorrelationIdProvider.Get());

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
