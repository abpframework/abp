using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using JetBrains.Annotations;
using Volo.Abp.Content;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators;
using Volo.Abp.Json;

namespace Volo.Abp.Http.Client.DynamicProxying
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

            if (value is IRemoteStreamContent remoteStreamContent)
            {
                var content = new StreamContent(remoteStreamContent.GetStream());
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(remoteStreamContent.ContentType);
                content.Headers.ContentLength = remoteStreamContent.ContentLength;
                return content;
            }
            else
            {
                return new StringContent(jsonSerializer.Serialize(value), Encoding.UTF8, MimeTypes.Application.Json);
            }
        }

        private static HttpContent GenerateFormPostData(ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments)
        {
            var parameters = action
                .Parameters
                .Where(p => p.BindingSourceId == ParameterBindingSources.Form)
                .ToArray();

            if (!parameters.Any())
            {
                return null;
            }

            var postDataBuilder = new StringBuilder();

            var isFirstParam = true;
            foreach (var queryStringParameter in parameters)
            {
                var value = HttpActionParameterHelper.FindParameterValue(methodArguments, queryStringParameter);
                if (value == null)
                {
                    continue;
                }

                postDataBuilder.Append(isFirstParam ? "?" : "&");
                postDataBuilder.Append(queryStringParameter.Name + "=" + System.Net.WebUtility.UrlEncode(value.ToString()));

                isFirstParam = false;
            }

            return new StringContent(postDataBuilder.ToString(), Encoding.UTF8, MimeTypes.Application.XWwwFormUrlencoded);
        }
    }
}
