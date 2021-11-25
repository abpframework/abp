using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Localization;

namespace Volo.Abp.Http.ProxyScripting.Generators;

internal static class ProxyScriptingHelper
{
    public const string DefaultHttpVerb = "POST";

    public static string GenerateUrlWithParameters(ActionApiDescriptionModel action)
    {
        // The ASP.NET Core route value provider and query string value provider:
        //  Treat values as invariant culture.
        //  Expect that URLs are culture-invariant.
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            //TODO: Can be optimized using StringBuilder?
            var url = ReplacePathVariables(action.Url, action.Parameters);
            url = AddQueryStringParameters(url, action.Parameters);
            return url;
        }
    }

    public static string GenerateHeaders(ActionApiDescriptionModel action, int indent = 0)
    {
        var parameters = action
            .Parameters
            .Where(p => p.BindingSourceId == ParameterBindingSources.Header)
            .ToArray();

        if (!parameters.Any())
        {
            return null;
        }

        return ProxyScriptingJsFuncHelper.CreateJsObjectLiteral(parameters, indent);
    }

    public static string GenerateBody(ActionApiDescriptionModel action)
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
                $"Only one complex type allowed as argument to a controller action that's binding source is 'Body'. But {action.Name} ({action.Url}) contains more than one!"
            );
        }

        return ProxyScriptingJsFuncHelper.GetParamNameInJsFunc(parameters[0]);
    }

    public static string GenerateFormPostData(ActionApiDescriptionModel action, int indent = 0)
    {
        var parameters = action
            .Parameters
            .Where(p => p.BindingSourceId == ParameterBindingSources.Form)
            .ToArray();

        if (!parameters.Any())
        {
            return null;
        }

        return ProxyScriptingJsFuncHelper.CreateJsFormPostData(parameters, indent);
    }

    private static string ReplacePathVariables(string url, IList<ParameterApiDescriptionModel> actionParameters)
    {
        var pathParameters = actionParameters
            .Where(p => p.BindingSourceId == ParameterBindingSources.Path)
            .ToArray();

        if (!pathParameters.Any())
        {
            return url;
        }

        foreach (var pathParameter in pathParameters)
        {
            url = url.Replace($"{{{pathParameter.Name}}}", $"' + {ProxyScriptingJsFuncHelper.GetParamNameInJsFunc(pathParameter)} + '");
        }

        return url;
    }

    private static string AddQueryStringParameters(string url, IList<ParameterApiDescriptionModel> actionParameters)
    {
        var queryStringParameters = actionParameters
            .Where(p => p.BindingSourceId.IsIn(ParameterBindingSources.ModelBinding, ParameterBindingSources.Query))
            .ToArray();

        if (!queryStringParameters.Any())
        {
            return url;
        }

        var qsBuilderParams = queryStringParameters
            .Select(p => $"{{ name: '{p.Name.ToCamelCase()}', value: {ProxyScriptingJsFuncHelper.GetParamNameInJsFunc(p)} }}")
            .JoinAsString(", ");

        return url + $"' + abp.utils.buildQueryString([{qsBuilderParams}]) + '";
    }

    public static string GetConventionalVerbForMethodName(string methodName)
    {
        if (methodName.StartsWith("Get", StringComparison.OrdinalIgnoreCase))
        {
            return "GET";
        }

        if (methodName.StartsWith("Put", StringComparison.OrdinalIgnoreCase) ||
            methodName.StartsWith("Update", StringComparison.OrdinalIgnoreCase))
        {
            return "PUT";
        }

        if (methodName.StartsWith("Delete", StringComparison.OrdinalIgnoreCase) ||
            methodName.StartsWith("Remove", StringComparison.OrdinalIgnoreCase))
        {
            return "DELETE";
        }

        if (methodName.StartsWith("Patch", StringComparison.OrdinalIgnoreCase))
        {
            return "PATCH";
        }

        if (methodName.StartsWith("Post", StringComparison.OrdinalIgnoreCase) ||
            methodName.StartsWith("Create", StringComparison.OrdinalIgnoreCase) ||
            methodName.StartsWith("Insert", StringComparison.OrdinalIgnoreCase))
        {
            return "POST";
        }

        //Default
        return DefaultHttpVerb;
    }
}
