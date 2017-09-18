using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators;

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
                .Where(p => p.BindingSourceId == ParameterBindingSources.Path)
                .ToArray();

            if (!pathParameters.Any())
            {
                return;
            }

            foreach (var pathParameter in pathParameters)
            {
                var value = HttpActionParameterHelper.FindParameterValue(methodArguments, pathParameter);
                if (value == null)
                {
                    if (pathParameter.IsOptional)
                    {
                        urlBuilder = urlBuilder.Replace($"{{{pathParameter.Name}}}", "");
                    }
                    else if (pathParameter.DefaultValue != null)
                    {
                        urlBuilder = urlBuilder.Replace($"{{{pathParameter.Name}}}", pathParameter.DefaultValue.ToString());
                    }
                    else
                    {
                        throw new AbpException($"Missing path parameter value for {pathParameter.Name} ({pathParameter.NameOnMethod})");
                    }
                }
                else
                {
                    urlBuilder = urlBuilder.Replace($"{{{pathParameter.Name}}}", value.ToString());
                }
            }
        }

        private static void AddQueryStringParameters(StringBuilder urlBuilder, IList<ParameterApiDescriptionModel> actionParameters, IReadOnlyDictionary<string, object> methodArguments)
        {
            var queryStringParameters = actionParameters
                .Where(p => p.BindingSourceId.IsIn(ParameterBindingSources.ModelBinding, ParameterBindingSources.Query))
                .ToArray();

            if (!queryStringParameters.Any())
            {
                return;
            }

            var isFirstParam = true;
            foreach (var queryStringParameter in queryStringParameters)
            {
                var value = HttpActionParameterHelper.FindParameterValue(methodArguments, queryStringParameter);
                if (value == null)
                {
                    continue;
                }

                urlBuilder.Append(isFirstParam ? "?" : "&");
                urlBuilder.Append(queryStringParameter.Name + "=" + System.Net.WebUtility.UrlEncode(value.ToString()));

                isFirstParam = false;
            }
        }
    }
}
