using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Volo.Abp.Swashbuckle;

public class AbpSwashbuckleDocumentFilter : IDocumentFilter
{
    protected virtual string[] ActionUrlPrefixes { get; set; } = new[] { "Volo." };

    protected virtual string RegexConstraintPattern => @":regex\(([^()]*)\)";
    
    public virtual void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var actionUrls = context.ApiDescriptions
            .Select(apiDescription => apiDescription.ActionDescriptor)
            .Where(actionDescriptor => !string.IsNullOrEmpty(actionDescriptor.DisplayName) &&
                                       ActionUrlPrefixes.Any(actionUrlPrefix => !actionDescriptor.DisplayName.Contains(actionUrlPrefix)))
            .DistinctBy(actionDescriptor => actionDescriptor.AttributeRouteInfo?.Template)
            .Select(RemoveRouteParameterConstraints)
            .Where(actionUrl => !string.IsNullOrEmpty(actionUrl))
            .ToList();

        swaggerDoc
            .Paths
            .RemoveAll(path => !actionUrls.Contains(path.Key));
    }

    protected virtual string RemoveRouteParameterConstraints(ActionDescriptor actionDescriptor)
    {
        var route = actionDescriptor.AttributeRouteInfo?.Template?.EnsureStartsWith('/').Replace("?", "");
        if (string.IsNullOrWhiteSpace(route))
        {
            return route;
        }

        route = Regex.Replace(route, RegexConstraintPattern, "");

        while (route.Contains(':'))
        {
            var startIndex = route.IndexOf(":", StringComparison.Ordinal);
            var endIndex = route.IndexOf("}", startIndex);

            if (endIndex == -1)
            {
                break;
            }
            
            route = route.Remove(startIndex, (endIndex - startIndex));
        }

        return route;
    }
}