using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators;
using Volo.Abp.Localization;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    internal static class UrlBuilder
    {
        public static string GenerateUrlWithParameters(ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments, ApiVersionInfo apiVersion)
        {
            // The ASP.NET Core route value provider and query string value provider:
            //  Treat values as invariant culture.
            //  Expect that URLs are culture-invariant.
            using (CultureHelper.Use(CultureInfo.InvariantCulture))
            {
                var urlBuilder = new StringBuilder(action.Url);

                ReplacePathVariables(urlBuilder, action.Parameters, methodArguments, apiVersion);
                AddQueryStringParameters(urlBuilder, action.Parameters, methodArguments, apiVersion);

                return urlBuilder.ToString();
            }
        }

        private static void ReplacePathVariables(StringBuilder urlBuilder, IList<ParameterApiDescriptionModel> actionParameters, IReadOnlyDictionary<string, object> methodArguments, ApiVersionInfo apiVersion)
        {
            var pathParameters = actionParameters
                .Where(p => p.BindingSourceId == ParameterBindingSources.Path)
                .ToArray();

            if (!pathParameters.Any())
            {
                return;
            }

            if (pathParameters.Any(p => p.Name == "apiVersion"))
            {
                urlBuilder = urlBuilder.Replace("{apiVersion}", apiVersion.Version);
            }

            foreach (var pathParameter in pathParameters.Where(p => p.Name != "apiVersion")) //TODO: Constant!
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

        private static void AddQueryStringParameters(StringBuilder urlBuilder, IList<ParameterApiDescriptionModel> actionParameters, IReadOnlyDictionary<string, object> methodArguments, ApiVersionInfo apiVersion)
        {
            var queryStringParameters = actionParameters
                .Where(p => p.BindingSourceId.IsIn(ParameterBindingSources.ModelBinding, ParameterBindingSources.Query))
                .ToArray();

            var isFirstParam = true;

            foreach (var queryStringParameter in queryStringParameters)
            {
                var value = HttpActionParameterHelper.FindParameterValue(methodArguments, queryStringParameter);
                if (value == null)
                {
                    continue;
                }

                if (AddQueryStringParameter(urlBuilder, isFirstParam, queryStringParameter.Name, value))
                {
                    isFirstParam = false;
                }
            }

            if (apiVersion.ShouldSendInQueryString())
            {
                AddQueryStringParameter(urlBuilder, isFirstParam, "api-version", apiVersion.Version);  //TODO: Constant!
            }
        }

        private static bool AddQueryStringParameter(
            StringBuilder urlBuilder,
            bool isFirstParam,
            string name,
            [NotNull] object value)
        {
            if (value.GetType().IsArray || (value.GetType().IsGenericType && value is IEnumerable))
            {
                var index = 0;
                foreach (var item in (IEnumerable) value)
                {
                    if (index == 0)
                    {
                        urlBuilder.Append(isFirstParam ? "?" : "&");
                    }
                    urlBuilder.Append(name + $"[{index++}]=" + System.Net.WebUtility.UrlEncode(ConvertValueToString(item)) + "&");
                }

                if (index > 0)
                {
                    //remove & at the end of the urlBuilder.
                    urlBuilder.Remove(urlBuilder.Length - 1, 1);
                    return true;
                }

                return false;
            }

            urlBuilder.Append(isFirstParam ? "?" : "&");
            urlBuilder.Append(name + "=" + System.Net.WebUtility.UrlEncode(ConvertValueToString(value)));
            return true;
        }

        private static string ConvertValueToString([NotNull] object value)
        {
            if (value is DateTime dateTimeValue)
            {
                return dateTimeValue.ToUniversalTime().ToString("O");
            }

            return value?.ToString();
        }
    }
}
