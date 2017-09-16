using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Reflection;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    internal static class UrlBuilder
    {
        public static string GenerateUrlWithParameters(ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments)
        {
            var urlBuilder = new StringBuilder(action.Url);

            ReplacePathVariables(urlBuilder, action.Parameters, methodArguments);
            AddQueryStringParameters(urlBuilder, action.Parameters, methodArguments);

            return urlBuilder.ToString();
        }

        private static void ReplacePathVariables(StringBuilder urlBuilder, IList<ParameterApiDescriptionModel> actionParameters, IReadOnlyDictionary<string, object> methodArguments)
        {
            var pathParameters = actionParameters
                .Where(p => p.BindingSourceId == "Path")
                .ToArray();

            if (!pathParameters.Any())
            {
                return;
            }

            foreach (var pathParameter in pathParameters)
            {
                urlBuilder = urlBuilder.Replace($"{{{pathParameter.Name}}}", FindParameterValue(methodArguments, pathParameter));
            }
        }

        private static void AddQueryStringParameters(StringBuilder urlBuilder, IList<ParameterApiDescriptionModel> actionParameters, IReadOnlyDictionary<string, object> methodArguments)
        {
            var queryStringParameters = actionParameters
                .Where(p => p.BindingSourceId.IsIn("ModelBinding", "Query"))
                .ToArray();

            if (!queryStringParameters.Any())
            {
                return;
            }

            var isFirstParam = true;
            foreach (var queryStringParameter in queryStringParameters)
            {
                var value = FindParameterValue(methodArguments, queryStringParameter);
                if (value == null)
                {
                    continue;
                }

                urlBuilder.Append(isFirstParam ? "?" : "&");
                urlBuilder.Append(queryStringParameter.Name + "=" + System.Net.WebUtility.UrlEncode(value));

                isFirstParam = false;
            }
        }

        private static string FindParameterValue(IReadOnlyDictionary<string, object> methodArguments, ParameterApiDescriptionModel apiParameter)
        {
            var value = methodArguments[apiParameter.NameOnMethod];
            if (value == null)
            {
                return null;
            }

            if (apiParameter.Name == apiParameter.NameOnMethod)
            {
                return value.ToString();
            }

            return ReflectionHelper.GetValueByPath(value, value.GetType(), apiParameter.Name)?.ToString();
        }
    }
}
