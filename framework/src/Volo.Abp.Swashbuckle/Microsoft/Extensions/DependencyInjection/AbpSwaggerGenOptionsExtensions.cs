using Swashbuckle.AspNetCore.SwaggerGen;
using Volo.Abp.Swashbuckle;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpSwaggerGenOptionsExtensions
{
    public static void HideAbpEndpoints(this SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.DocumentFilter<AbpSwashbuckleDocumentFilter>();
    }
}