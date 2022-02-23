using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Volo.Abp.Swashbuckle;

public class AbpSwashbuckleDocumentFilter : IDocumentFilter
{
    protected string[] ActionUrlPrefixes = new[] {"Volo.Abp"};
    
    public virtual void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var actionUrls = context.ApiDescriptions
            .Select(apiDescription => apiDescription.ActionDescriptor)
            .Where(actionDescriptor => !string.IsNullOrEmpty(actionDescriptor.DisplayName) &&
                                       ActionUrlPrefixes.Any(actionUrlPrefix => !actionDescriptor.DisplayName.Contains(actionUrlPrefix)))
            .DistinctBy(actionDescriptor => actionDescriptor.AttributeRouteInfo?.Template)
            .Select(actionDescriptor => actionDescriptor.AttributeRouteInfo?.Template.EnsureStartsWith('/'))
            .Where(actionUrl => !string.IsNullOrEmpty(actionUrl))
            .ToList();

        swaggerDoc
            .Paths
            .RemoveAll(path => !actionUrls.Contains(path.Key));
    }
}