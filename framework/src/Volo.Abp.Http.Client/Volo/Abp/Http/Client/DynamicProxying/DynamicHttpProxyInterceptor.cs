using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators;
using Volo.Abp.Json;
using Volo.Abp.Reflection;
using Volo.Abp.Threading;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    //TODO: Somehow capture cancellationtoken and pass to other methods...?

    public class DynamicHttpProxyInterceptor<TService> : AbpInterceptor, ITransientDependency
    {
        private static MethodInfo GenericInterceptAsyncMethod { get; }

        private readonly IDynamicProxyHttpClientFactory _httpClientFactory;
        private readonly IApiDescriptionFinder _apiDescriptionFinder;
        private readonly RemoteServiceOptions _remoteServiceOptions;
        private readonly AbpHttpClientOptions _clientOptions;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IRemoteServiceHttpClientAuthenticator _clientAuthenticator;

        public ILogger<DynamicHttpProxyInterceptor<TService>> Logger { get; set; }

        static DynamicHttpProxyInterceptor()
        {
            GenericInterceptAsyncMethod = typeof(DynamicHttpProxyInterceptor<TService>)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(m => m.Name == nameof(MakeRequestAndGetResultAsync) && m.IsGenericMethodDefinition);
        }

        public DynamicHttpProxyInterceptor(
            IDynamicProxyHttpClientFactory httpClientFactory,
            IOptions<AbpHttpClientOptions> clientOptions,
            IOptionsSnapshot<RemoteServiceOptions> remoteServiceOptions,
            IApiDescriptionFinder apiDescriptionFinder,
            IJsonSerializer jsonSerializer,
            IRemoteServiceHttpClientAuthenticator clientAuthenticator)
        {
            _httpClientFactory = httpClientFactory;
            _apiDescriptionFinder = apiDescriptionFinder;
            _jsonSerializer = jsonSerializer;
            _clientAuthenticator = clientAuthenticator;
            _clientOptions = clientOptions.Value;
            _remoteServiceOptions = remoteServiceOptions.Value;

            Logger = NullLogger<DynamicHttpProxyInterceptor<TService>>.Instance;
        }

        public override void Intercept(IAbpMethodInvocation invocation)
        {
            if (invocation.Method.ReturnType == typeof(void))
            {
                AsyncHelper.RunSync(() => MakeRequest(invocation));
            }
            else
            {
                var responseAsString = AsyncHelper.RunSync(() => MakeRequest(invocation));

                //TODO: Think on that
                if (TypeHelper.IsPrimitiveExtended(invocation.Method.ReturnType, true))
                {
                    invocation.ReturnValue = Convert.ChangeType(responseAsString, invocation.Method.ReturnType);
                }
                else
                {
                    invocation.ReturnValue = _jsonSerializer.Deserialize(
                        invocation.Method.ReturnType,
                        responseAsString
                    );
                }
            }
        }

        public override Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            if (invocation.Method.ReturnType.GenericTypeArguments.IsNullOrEmpty())
            {
                return MakeRequest(invocation);
            }

            invocation.ReturnValue = GenericInterceptAsyncMethod
                .MakeGenericMethod(invocation.Method.ReturnType.GenericTypeArguments[0])
                .Invoke(this, new object[] { invocation });

            return Task.CompletedTask;
        }

        private async Task<T> MakeRequestAndGetResultAsync<T>(IAbpMethodInvocation invocation)
        {
            var responseAsString = await MakeRequest(invocation);

            //TODO: Think on that
            if (TypeHelper.IsPrimitiveExtended(typeof(T), true))
            {
                return (T)Convert.ChangeType(responseAsString, typeof(T));
            }

            return _jsonSerializer.Deserialize<T>(responseAsString);
        }

        private async Task<string> MakeRequest(IAbpMethodInvocation invocation)
        {
            using (var client = _httpClientFactory.Create())
            {
                var clientConfig = _clientOptions.HttpClientProxies.GetOrDefault(typeof(TService)) ?? throw new AbpException($"Could not get DynamicHttpClientProxyConfig for {typeof(TService).FullName}.");
                var remoteServiceConfig = _remoteServiceOptions.RemoteServices.GetConfigurationOrDefault(clientConfig.RemoteServiceName);

                var action = await _apiDescriptionFinder.FindActionAsync(remoteServiceConfig.BaseUrl, typeof(TService), invocation.Method);
                var apiVersion = GetApiVersionInfo(action);
                var url = remoteServiceConfig.BaseUrl + UrlBuilder.GenerateUrlWithParameters(action, invocation.ArgumentsDictionary, apiVersion);

                var requestMessage = new HttpRequestMessage(action.GetHttpMethod(), url)
                {
                    Content = RequestPayloadBuilder.BuildContent(action, invocation.ArgumentsDictionary, _jsonSerializer, apiVersion)
                };

                AddHeaders(invocation, action, requestMessage, apiVersion);

                await _clientAuthenticator.Authenticate(
                    new RemoteServiceHttpClientAuthenticateContext(
                        client,
                        requestMessage,
                        remoteServiceConfig,
                        clientConfig.RemoteServiceName
                    )
                );

                var response = await client.SendAsync(requestMessage);

                if (!response.IsSuccessStatusCode)
                {
                    await ThrowExceptionForResponseAsync(response);
                }

                return await response.Content.ReadAsStringAsync();
            }
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

        private static void AddHeaders(IAbpMethodInvocation invocation, ActionApiDescriptionModel action, HttpRequestMessage requestMessage, ApiVersionInfo apiVersion)
        {
            if (!apiVersion.Version.IsNullOrEmpty())
            {
                //TODO: What about other media types?
                requestMessage.Headers.Add("accept", $"{MimeTypes.Text.Plain}; v={apiVersion.Version}");
                requestMessage.Headers.Add("accept", $"{MimeTypes.Application.Json}; v={apiVersion.Version}");
                requestMessage.Headers.Add("api-version", apiVersion.Version);
            }

            var headers = action.Parameters.Where(p => p.BindingSourceId == ParameterBindingSources.Header).ToArray();

            foreach (var headerParameter in headers)
            {
                var value = HttpActionParameterHelper.FindParameterValue(invocation.ArgumentsDictionary, headerParameter);
                if (value != null)
                {
                    requestMessage.Headers.Add(headerParameter.Name, value.ToString());
                }
            }
        }

        private string GetConfiguredApiVersion()
        {
            var clientConfig = _clientOptions.HttpClientProxies.GetOrDefault(typeof(TService))
                               ?? throw new AbpException($"Could not get DynamicHttpClientProxyConfig for {typeof(TService).FullName}.");

            return _remoteServiceOptions.RemoteServices.GetOrDefault(clientConfig.RemoteServiceName)?.Version
                   ?? _remoteServiceOptions.RemoteServices.Default?.Version;
        }

        private async Task ThrowExceptionForResponseAsync(HttpResponseMessage response)
        {
            if (response.Headers.Contains(AbpHttpConsts.AbpErrorFormat))
            {
                var errorResponse = _jsonSerializer.Deserialize<RemoteServiceErrorResponse>(
                    await response.Content.ReadAsStringAsync()
                );

                throw new AbpRemoteCallException(errorResponse.Error);
            }

            throw new AbpException($"Remote service returns error! HttpStatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}");
        }
    }
}