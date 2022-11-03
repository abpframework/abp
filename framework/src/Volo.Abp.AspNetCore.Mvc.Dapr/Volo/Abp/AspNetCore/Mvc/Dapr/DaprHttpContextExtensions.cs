using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Dapr;

public static class DaprHttpContextExtensions
{
    public static async Task ValidateDaprAppApiTokenAsync(this HttpContext httpContext)
    {
        await httpContext
            .RequestServices
            .GetRequiredService<IDaprAppApiTokenValidator>()
            .CheckDaprAppApiTokenAsync();
    }

    public static async Task<bool> IsValidDaprAppApiTokenAsync(this HttpContext httpContext)
    {
        return await httpContext
            .RequestServices
            .GetRequiredService<IDaprAppApiTokenValidator>()
            .IsValidDaprAppApiTokenAsync();
    }

    public static async Task<string> GetDaprAppApiTokenOrNullAsync(HttpContext httpContext)
    {
        return await httpContext
            .RequestServices
            .GetRequiredService<IDaprAppApiTokenValidator>()
            .GetDaprAppApiTokenOrNullAsync();
    }
}
