using System;
using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Volo.Abp.Swashbuckle;

public class AbpSwashbuckleEnumSchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is OpenApiSchema openApiScheme && context.Type.IsEnum)
        {
            openApiScheme.Enum?.Clear();
            openApiScheme.Type = JsonSchemaType.String;
            openApiScheme.Format = null;
            foreach (var name in Enum.GetNames(context.Type))
            {
                openApiScheme.Enum?.Add(JsonNode.Parse($"\"{name}\"")!);
            }
        }
    }
}
