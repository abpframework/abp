using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using JetBrains.Annotations;

namespace Volo.Abp.Http;

public static class HttpMethodHelper
{
    public const string DefaultHttpVerb = "POST";

    public static Dictionary<string, string[]> ConventionalPrefixes { get; set; } = new Dictionary<string, string[]>
        {
            {"GET", new[] {"GetList", "GetAll", "Get"}},
            {"PUT", new[] {"Put", "Update"}},
            {"DELETE", new[] {"Delete", "Remove"}},
            {"POST", new[] {"Create", "Add", "Insert", "Post"}},
            {"PATCH", new[] {"Patch"}}
        };

    public static string GetConventionalVerbForMethodName(string methodName)
    {
        foreach (var conventionalPrefix in ConventionalPrefixes)
        {
            if (conventionalPrefix.Value.Any(prefix => methodName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
            {
                return conventionalPrefix.Key;
            }
        }

        return DefaultHttpVerb;
    }

    public static string RemoveHttpMethodPrefix([NotNull] string methodName, [NotNull] string httpMethod)
    {
        Check.NotNull(methodName, nameof(methodName));
        Check.NotNull(httpMethod, nameof(httpMethod));

        var prefixes = ConventionalPrefixes.GetOrDefault(httpMethod);
        if (prefixes.IsNullOrEmpty())
        {
            return methodName;
        }

        return methodName.RemovePreFix(prefixes);
    }

    public static HttpMethod ConvertToHttpMethod(string httpMethod)
    {
        switch (httpMethod.ToUpperInvariant())
        {
            case "GET":
                return HttpMethod.Get;
            case "POST":
                return HttpMethod.Post;
            case "PUT":
                return HttpMethod.Put;
            case "DELETE":
                return HttpMethod.Delete;
            case "OPTIONS":
                return HttpMethod.Options;
            case "TRACE":
                return HttpMethod.Trace;
            case "HEAD":
                return HttpMethod.Head;
            case "PATCH":
                return new HttpMethod("PATCH");
            default:
                throw new AbpException("Unknown HTTP METHOD: " + httpMethod);
        }
    }
}
