using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using JetBrains.Annotations;
using Volo.Abp.Content;
using Volo.Abp.Http.Client.DynamicProxying;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators;
using Volo.Abp.Json;

namespace Volo.Abp.Http.Client.Proxying
{
    public static class RequestPayloadBuilder
    {
        [CanBeNull]
        public static HttpContent BuildContent(ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments, IJsonSerializer jsonSerializer, ApiVersionInfo apiVersion)
        {
            var body = GenerateBody(action, methodArguments, jsonSerializer);
            if (body != null)
            {
                return body;
            }

            body = GenerateFormPostData(action, methodArguments);

            return body;
        }

        private static HttpContent GenerateBody(ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments, IJsonSerializer jsonSerializer)
        {
            var parameters = action
                .Parameters
                .Where(p => p.BindingSourceId == ParameterBindingSources.Body)
                .ToArray();

            if (parameters.Length <= 0)
            {
                return null;
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
                return null;
            }

            return new StringContent(jsonSerializer.Serialize(value), Encoding.UTF8, MimeTypes.Application.Json);
        }

        private static HttpContent GenerateFormPostData(ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments)
        {
            var parameters = action
                .Parameters
                .Where(p => p.BindingSourceId == ParameterBindingSources.Form || p.BindingSourceId == ParameterBindingSources.FormFile)
                .ToArray();

            if (!parameters.Any())
            {
                return null;
            }

            if (parameters.Any(x => x.BindingSourceId == ParameterBindingSources.FormFile))
            {
                var formData = new MultipartFormDataContent();
                foreach (var parameter in parameters)
                {
                    var value = HttpActionParameterHelper.FindParameterValue(methodArguments, parameter);
                    if (value == null)
                    {
                        continue;
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
                    else
                    {
                        formData.Add(new StringContent(value.ToString(), Encoding.UTF8), parameter.Name);
                    }
                }

                return formData;
            }
            else
            {
                var postDataBuilder = new StringBuilder();

                var isFirstParam = true;
                foreach (var parameter in parameters.Where(p => p.BindingSourceId == ParameterBindingSources.Form))
                {
                    var value = HttpActionParameterHelper.FindParameterValue(methodArguments, parameter);
                    if (value == null)
                    {
                        continue;
                    }

                    postDataBuilder.Append(isFirstParam ? "?" : "&");
                    postDataBuilder.Append(parameter.Name + "=" + System.Net.WebUtility.UrlEncode(value.ToString()));

                    isFirstParam = false;
                }

                return new StringContent(postDataBuilder.ToString(), Encoding.UTF8, MimeTypes.Application.XWwwFormUrlencoded);
            }
        }
    }
}
