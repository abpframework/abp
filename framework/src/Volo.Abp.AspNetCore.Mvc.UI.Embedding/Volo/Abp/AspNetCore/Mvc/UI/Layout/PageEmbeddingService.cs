using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Layout;

public class PageEmbeddingService : IPageEmbeddingService, ITransientDependency
{
    protected PageEmbeddingOptions Options { get; }

    public PageEmbeddingService(IOptions<PageEmbeddingOptions> options)
    {
        Options = options.Value;
    }

    public virtual bool IsEmbeddingRequest(HttpContext httpContext)
    {
        // Check if iframe detection is enabled and this is an iframe request
        if (Options.AlwaysEmbedIFrameRequests && IsIFrameRequest(httpContext))
        {
            return true;
        }

        // Check query parameter
        if (IsEmbeddingQueryParameterPresent(httpContext))
        {
            return true;
        }

        // Check exact paths
        if (IsEmbeddedPath(httpContext))
        {
            return true;
        }

        // Check path patterns
        if (IsEmbeddedPathPattern(httpContext))
        {
            return true;
        }

        return false;
    }

    protected virtual bool IsEmbeddingQueryParameterPresent(HttpContext httpContext)
    {
        if (!httpContext.Request.Query.ContainsKey(Options.QueryParameterName))
        {
            return false;
        }

        var value = httpContext.Request.Query[Options.QueryParameterName].ToString().ToLowerInvariant();
        return Options.QueryParameterValues.Contains(value);
    }

    protected virtual bool IsEmbeddedPath(HttpContext httpContext)
    {
        var path = httpContext.Request.Path.Value?.ToLowerInvariant();
        if (path == null)
        {
            return false;
        }

        return Options.EmbeddedPaths.Any(embeddedPath => 
            path.Equals(embeddedPath.ToLowerInvariant()));
    }

    protected virtual bool IsEmbeddedPathPattern(HttpContext httpContext)
    {
        var path = httpContext.Request.Path.Value?.ToLowerInvariant();
        if (path == null)
        {
            return false;
        }

        return Options.EmbeddedPathPatterns.Any(pattern =>
            IsPathMatchingPattern(path, pattern.ToLowerInvariant()));
    }

    protected virtual bool IsPathMatchingPattern(string path, string pattern)
    {
        // Simple wildcard matching - can be enhanced for more complex patterns
        if (pattern.EndsWith("*"))
        {
            var prefix = pattern[..^1];
            return path.StartsWith(prefix);
        }

        if (pattern.StartsWith("*"))
        {
            var suffix = pattern[1..];
            return path.EndsWith(suffix);
        }

        return path.Equals(pattern);
    }

    protected virtual bool IsIFrameRequest(HttpContext httpContext)
    {
        // Method 1: Check Sec-Fetch-Dest header (most reliable for modern browsers)
        if (IsIFrameRequestBySecFetchDest(httpContext))
        {
            return true;
        }

        // Method 2: Check Sec-Fetch-Site and Sec-Fetch-Mode headers
        if (IsIFrameRequestBySecFetchHeaders(httpContext))
        {
            return true;
        }

        // Method 3: Check for custom iframe headers
        if (IsIFrameRequestByCustomHeaders(httpContext))
        {
            return true;
        }

        // Method 4: Check X-Requested-With header
        if (IsIFrameRequestByXRequestedWith(httpContext))
        {
            return true;
        }

        // Method 5: Check referer header (least reliable)
        if (IsIFrameRequestByReferer(httpContext))
        {
            return true;
        }

        return false;
    }

    protected virtual bool IsIFrameRequestBySecFetchDest(HttpContext httpContext)
    {
        var secFetchDest = httpContext.Request.Headers["Sec-Fetch-Dest"].FirstOrDefault();
        return string.Equals(secFetchDest, "iframe", StringComparison.OrdinalIgnoreCase);
    }

    protected virtual bool IsIFrameRequestBySecFetchHeaders(HttpContext httpContext)
    {
        var secFetchSite = httpContext.Request.Headers["Sec-Fetch-Site"].FirstOrDefault();
        var secFetchMode = httpContext.Request.Headers["Sec-Fetch-Mode"].FirstOrDefault();

        // Check for cross-site or same-site requests with navigate mode (common for iframes)
        return !string.IsNullOrEmpty(secFetchSite) && 
               (string.Equals(secFetchSite, "cross-site", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(secFetchSite, "same-site", StringComparison.OrdinalIgnoreCase)) &&
               string.Equals(secFetchMode, "navigate", StringComparison.OrdinalIgnoreCase);
    }

    protected virtual bool IsIFrameRequestByCustomHeaders(HttpContext httpContext)
    {
        // Check for common custom headers that indicate iframe requests
        var customHeaders = new[]
        {
            "X-Frame-Request",
            "X-Iframe-Request", 
            "X-Embedded-Request"
        };

        return customHeaders.Any(header =>
        {
            var value = httpContext.Request.Headers[header].FirstOrDefault();
            return string.Equals(value, "true", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(value, "1", StringComparison.OrdinalIgnoreCase);
        });
    }

    protected virtual bool IsIFrameRequestByXRequestedWith(HttpContext httpContext)
    {
        var xRequestedWith = httpContext.Request.Headers["X-Requested-With"].FirstOrDefault();
        
        // Some iframe libraries use this header
        return string.Equals(xRequestedWith, "iframe", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(xRequestedWith, "embedded", StringComparison.OrdinalIgnoreCase);
    }

    protected virtual bool IsIFrameRequestByReferer(HttpContext httpContext)
    {
        var referer = httpContext.Request.Headers["Referer"].FirstOrDefault();
        var host = httpContext.Request.Host.Host;

        if (string.IsNullOrEmpty(referer) || string.IsNullOrEmpty(host))
        {
            return false;
        }

        try
        {
            var refererUri = new Uri(referer);
            var refererHost = refererUri.Host;

            // If referer host is different from current host, it might be an iframe request
            // This is the least reliable method as it can have false positives
            return !string.Equals(refererHost, host, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            // Invalid referer URI
            return false;
        }
    }
} 