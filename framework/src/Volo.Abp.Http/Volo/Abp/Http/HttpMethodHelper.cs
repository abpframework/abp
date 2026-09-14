using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using JetBrains.Annotations;

namespace Volo.Abp.Http;

public static class HttpMethodHelper
{
    public const string Get = "GET";
    public const string Post = "POST";
    public const string Put = "PUT";
    public const string Delete = "DELETE";
    public const string Patch = "PATCH";
    public const string Head = "HEAD";
    public const string Options = "OPTIONS";
    public const string Trace = "TRACE";
    public const string Query = "QUERY";

    public const string DefaultHttpVerb = Post;

    public static Dictionary<string, string[]> ConventionalPrefixes { get; set; } = new Dictionary<string, string[]>
        {
            {Get, new[] {"GetList", "GetAll", "Get"}},
            {Put, new[] {"Put", "Update"}},
            {Delete, new[] {"Delete", "Remove"}},
            {Post, new[] {"Create", "Add", "Insert", "Post"}},
            {Patch, new[] {"Patch"}}
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

        return methodName.RemovePreFix(prefixes!);
    }

    public static HttpMethod ConvertToHttpMethod(string? httpMethod)
    {
        switch (httpMethod?.ToUpperInvariant())
        {
            case Get:
                return HttpMethod.Get;
            case Post:
                return HttpMethod.Post;
            case Put:
                return HttpMethod.Put;
            case Delete:
                return HttpMethod.Delete;
            case Options:
                return HttpMethod.Options;
            case Trace:
                return HttpMethod.Trace;
            case Head:
                return HttpMethod.Head;
            case Patch:
                return new HttpMethod(Patch);
            case Query:
                return new HttpMethod(Query);
            default:
                throw new AbpException("Unknown HTTP METHOD: " + httpMethod);
        }
    }

    public static bool IsGet(string? httpMethod) => string.Equals(httpMethod, Get, StringComparison.OrdinalIgnoreCase);

    public static bool IsPost(string? httpMethod) => string.Equals(httpMethod, Post, StringComparison.OrdinalIgnoreCase);

    public static bool IsPut(string? httpMethod) => string.Equals(httpMethod, Put, StringComparison.OrdinalIgnoreCase);

    public static bool IsDelete(string? httpMethod) => string.Equals(httpMethod, Delete, StringComparison.OrdinalIgnoreCase);

    public static bool IsPatch(string? httpMethod) => string.Equals(httpMethod, Patch, StringComparison.OrdinalIgnoreCase);

    public static bool IsHead(string? httpMethod) => string.Equals(httpMethod, Head, StringComparison.OrdinalIgnoreCase);

    public static bool IsOptions(string? httpMethod) => string.Equals(httpMethod, Options, StringComparison.OrdinalIgnoreCase);

    public static bool IsTrace(string? httpMethod) => string.Equals(httpMethod, Trace, StringComparison.OrdinalIgnoreCase);

    public static bool IsQuery(string? httpMethod) => string.Equals(httpMethod, Query, StringComparison.OrdinalIgnoreCase);
}
