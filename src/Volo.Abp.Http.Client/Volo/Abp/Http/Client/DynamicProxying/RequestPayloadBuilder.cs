using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using JetBrains.Annotations;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Json;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public static class RequestPayloadBuilder
    {
        [CanBeNull]
        public static HttpContent BuildContent(ActionApiDescriptionModel action,IReadOnlyDictionary<string, object> methodArguments, IJsonSerializer jsonSerializer)
        {
            var body = GenerateBody(action, methodArguments, jsonSerializer);

            if (body != null)
            {
                return new StringContent(body, Encoding.UTF8, "application/json"); //TODO: application/json to a constant
            }

            body = GenerateFormPostData(action, methodArguments);
            if (body != null)
            {
                return new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded"); //TODO: application/x-www-form-urlencoded to a constant
            }

            return null;
        }

        private static string GenerateBody(ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments, IJsonSerializer jsonSerializer)
        {
            var parameters = action
                .Parameters
                .Where(p => p.BindingSourceId == "Body")
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

            return jsonSerializer.Serialize(value);
        }

        private static string GenerateFormPostData(ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments)
        {
            var parameters = action
                .Parameters
                .Where(p => p.BindingSourceId == "Form")
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

            return postDataBuilder.ToString();
        }
    }
}
