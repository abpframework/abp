using Swashbuckle.AspNetCore.SwaggerGen;
using Volo.Abp.Swashbuckle;

namespace Microsoft.Extensions.DependencyInjection;

public static class SwaggerGenOptionsExtensions
{
    public static void HideAbpEndpoints(this SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.DocumentFilter<AbpSwashbuckleDocumentFilter>();
    }
}