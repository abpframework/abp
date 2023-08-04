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

        var actionSchemas = new HashSet<string>();
        actionSchemas.UnionWith(swaggerDoc.Paths.Select(path => new { path = path.Value, schemas = swaggerDoc.Components.Schemas }).SelectMany(x => GetSchemaIdList(x.path, x.schemas)));

        swaggerDoc.Components.Schemas.RemoveAll(schema => !actionSchemas.Contains(schema.Key));
    }

    protected virtual HashSet<string> GetSchemaIdList(OpenApiPathItem pathItem, IDictionary<string, OpenApiSchema> schemas)
    {
        var selectedSchemas = new HashSet<OpenApiSchema>();
        var schemaIds = new HashSet<string>();

        selectedSchemas.UnionWith(pathItem.Parameters.Select(parameter => parameter.Schema));
        selectedSchemas.UnionWith(pathItem.Parameters.SelectMany(parameter => parameter.Content.Select(x => x.Value.Schema)));
        selectedSchemas.UnionWith(pathItem.Operations.Values.SelectMany(c => c.Responses.SelectMany(x => x.Value.Content.Values.Select(x => x.Schema))));
        selectedSchemas.UnionWith(pathItem.Operations.Values.SelectMany(c => c.Parameters.Select(x => x.Schema)));

        foreach (var schema in selectedSchemas)
        {
            schemaIds.UnionWith(MakeListSchemaId(schema, schemas));
        }
        return schemaIds;
    }
    protected virtual string? RemoveRouteParameterConstraints(ActionDescriptor actionDescriptor)
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

            route = route.Remove(startIndex, endIndex - startIndex);
        }

        return route;
    }

    private HashSet<string> MakeListSchemaId(OpenApiSchema schema, IDictionary<string, OpenApiSchema> contextSchemas)
    {
        var schemasId = new HashSet<string>();
        if (schema != null)
        {
            if (schema.Reference != null)
            {
                schemasId.Add(schema.Reference.Id);
                foreach (var sc in contextSchemas.Where(x => x.Key == schema.Reference.Id).Select(x => x.Value))
                {
                    foreach (var property in sc.Properties.Values)
                    {
                        schemasId.UnionWith(MakeListSchemaId(property, contextSchemas));
                    }
                }
            }
            if (schema.Items != null)
            {
                schemasId.UnionWith(MakeListSchemaId(schema.Items, contextSchemas));
            }
        }
        return schemasId;
    }
}