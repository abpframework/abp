using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Dapr;

public static class DaprHttpContextExtensions
{
    public static void ValidateDaprAppApiToken(this HttpContext httpContext)
    {
        httpContext
            .RequestServices
            .GetRequiredService<IDaprAppApiTokenValidator>()
            .CheckDaprAppApiToken(httpContext);
    }
    
    public static bool IsValidDaprAppApiToken(this HttpContext httpContext)
    {
        return httpContext
            .RequestServices
            .GetRequiredService<IDaprAppApiTokenValidator>()
            .IsValidDaprAppApiToken(httpContext);
    }
    
    public static string? GetDaprAppApiTokenOrNull(HttpContext httpContext)
    {
        return httpContext
            .RequestServices
            .GetRequiredService<IDaprAppApiTokenValidator>()
            .GetDaprAppApiTokenOrNull(httpContext);
    }
}