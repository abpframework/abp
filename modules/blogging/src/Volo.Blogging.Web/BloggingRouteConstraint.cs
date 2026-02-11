using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Volo.Blogging;

public class BloggingRouteConstraint : IRouteConstraint
{
    protected BloggingUrlOptions BloggingUrlOptions { get; }

    public BloggingRouteConstraint(IOptions<BloggingUrlOptions> bloggingUrlOptions)
    {
        BloggingUrlOptions = bloggingUrlOptions.Value;
    }
    
    public virtual bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (BloggingUrlOptions.RoutePrefix != "/" || !BloggingUrlOptions.IgnoredPaths.Any())
        {
            return true;
        }

        if (!values.TryGetValue(routeKey, out var routeValue) || routeValue is not string routeValueString)
        {
            return true;
        }

        return !BloggingUrlOptions.IgnoredPaths.Any(path => routeValueString.StartsWith(path, StringComparison.InvariantCultureIgnoreCase));
    }
}