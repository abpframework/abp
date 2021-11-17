using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Proxying;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators;
using Volo.Abp.Json;

namespace Volo.Abp.Http.Client.ClientProxying
{
    public class ClientProxyRequestPayloadBuilder : ITransientDependency
    {
        protected static MethodInfo CallObjectToFormDataAsyncMethod { get; }

        static ClientProxyRequestPayloadBuilder()
        {
            CallObjectToFormDataAsyncMethod = typeof(ClientProxyRequestPayloadBuilder)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(m => m.Name == nameof(ObjectToFormDataAsync) && m.IsGenericMethodDefinition);
        }

        protected IServiceScopeFactory ServiceScopeFactory { get; }

        protected AbpHttpClientProxyingOptions HttpClientProxyingOptions { get; }

        public ClientProxyRequestPayloadBuilder(IServiceScopeFactory serviceScopeFactory, IOptions<AbpHttpClientProxyingOptions> httpClientProxyingOptions)
        {
            ServiceScopeFactory = serviceScopeFactory;
            HttpClientProxyingOptions = httpClientProxyingOptions.Value;
        }

        [CanBeNull]
        public virtual async Task<HttpContent> BuildContentAsync(ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments, IJsonSerializer jsonSerializer, ApiVersionInfo apiVersion)
        {
            var body = await GenerateBodyAsync(action, methodArguments, jsonSerializer);
            if (body != null)
            {
                return body;
            }

            body = await GenerateFormPostDataAsync(action, methodArguments);

            return body;
        }

        protected virtual Task<HttpContent> GenerateBodyAsync(ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments, IJsonSerializer jsonSerializer)
        {
            var parameters = action
                .Parameters
                .Where(p => p.BindingSourceId == ParameterBindingSources.Body)
                .ToArray();

            if (parameters.Length <= 0)
            {
                return Task.FromResult<HttpContent>(null);
            }

            if (parameters.Length > 1)
            {
                throw new AbpException(
                    $"Only one complex type allowed as argument to a controller action that's binding source is 'Body'. But action on URL: {action.Url} contains more than one!"
                );
            }

            var value = HttpActionParameterHelper.FindParameterValue(methodArguments, parameters[0]);
            if (value == null)
            {
                return Task.FromResult<HttpContent>(null);
            }

            return Task.FromResult<HttpContent>(new StringContent(jsonSerializer.Serialize(value), Encoding.UTF8, MimeTypes.Application.Json));
        }

        protected virtual async Task<HttpContent> GenerateFormPostDataAsync(ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments)
        {
            var parameters = action
                .Parameters
                .Where(p => p.BindingSourceId == ParameterBindingSources.Form || p.BindingSourceId == ParameterBindingSources.FormFile)
                .ToArray();

            if (!parameters.Any())
            {
                return null;
            }

            var formData = new MultipartFormDataContent();

            foreach (var parameter in parameters)
            {
                var value = HttpActionParameterHelper.FindParameterValue(methodArguments, parameter);
                if (value == null)
                {
                    continue;
                }

                if (HttpClientProxyingOptions.FormDataConverts.ContainsKey(value.GetType()))
                {
                    using (var scope = ServiceScopeFactory.CreateScope())
                    {
                        var formDataContents = await (Task<List<KeyValuePair<string, HttpContent>>>)CallObjectToFormDataAsyncMethod
                            .MakeGenericMethod(value.GetType())
                            .Invoke(this, new object[]
                            {
                                scope.ServiceProvider.GetRequiredService(HttpClientProxyingOptions.FormDataConverts[value.GetType()]),
                                value
                            });

                        if (formDataContents != null)
                        {
                            foreach (var content in formDataContents)
                            {
                                formData.Add(content.Value, content.Key);
                            }
                            continue;
                        }
                    }
                }

                if (value is IRemoteStreamContent remoteStreamContent)
                {
                    var stream = remoteStreamContent.GetStream();
                    var streamContent = new StreamContent(stream);
                    if (!remoteStreamContent.ContentType.IsNullOrWhiteSpace())
                    {
                        streamContent.Headers.ContentType = new MediaTypeHeaderValue(remoteStreamContent.ContentType);
                    }
                    streamContent.Headers.ContentLength = remoteStreamContent.ContentLength;
                    formData.Add(streamContent, parameter.Name, remoteStreamContent.FileName ?? parameter.Name);
                }
                else if (value is IEnumerable<IRemoteStreamContent> remoteStreamContents)
                {
                    foreach (var content in remoteStreamContents)
                    {
                        var stream = content.GetStream();
                        var streamContent = new StreamContent(stream);
                        if (!content.ContentType.IsNullOrWhiteSpace())
                        {
                            streamContent.Headers.ContentType = new MediaTypeHeaderValue(content.ContentType);
                        }
                        streamContent.Headers.ContentLength = content.ContentLength;
                        formData.Add(streamContent, parameter.Name, content.FileName ?? parameter.Name);
                    }
                }
                else if (value.GetType().IsArray || (value.GetType().IsGenericType && value is IEnumerable))
                {
                    foreach (var item in (IEnumerable) value)
                    {
                        formData.Add(new StringContent(item.ToString(), Encoding.UTF8), parameter.Name);
                    }
                }
                else
                {
                    formData.Add(new StringContent(value.ToString(), Encoding.UTF8), parameter.Name);
                }
            }

            return formData;
        }

        protected virtual async Task<List<KeyValuePair<string, HttpContent>>> ObjectToFormDataAsync<T>(IObjectToFormData<T> converter, T value)
        {
            return await converter.ConvertAsync(value);
        }
    }
}
