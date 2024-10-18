using System;
using System.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using Volo.Abp.Swashbuckle;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpSwaggerGenOptionsExtensions
{
    public static void HideAbpEndpoints(this SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.DocumentFilter<AbpSwashbuckleDocumentFilter>();
    }

    public static void UserFriendlyEnums(this SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.SchemaFilter<AbpSwashbuckleEnumSchemaFilter>();
    }

    public static void CustomAbpSchemaIds(this SwaggerGenOptions options)
    {
        string SchemaIdSelector(Type modelType)
        {
            if (!modelType.IsConstructedGenericType)
            {
                return modelType.FullName!.Replace("[]", "Array");
            }

            var prefix = modelType.GetGenericArguments()
                .Select(SchemaIdSelector)
                .Aggregate((previous, current) => previous + current);
            return modelType.FullName!.Split('`').First() + "Of" + prefix;
        }

        options.CustomSchemaIds(SchemaIdSelector);
    }
}
