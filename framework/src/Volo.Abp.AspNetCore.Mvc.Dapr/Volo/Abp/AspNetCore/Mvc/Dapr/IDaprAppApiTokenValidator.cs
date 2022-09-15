using Microsoft.AspNetCore.Http;

namespace Volo.Abp.AspNetCore.Mvc.Dapr;

public interface IDaprAppApiTokenValidator
{
    void CheckDaprAppApiToken(HttpContext httpContext);
    bool IsValidDaprAppApiToken(HttpContext httpContext);
    string? GetDaprAppApiTokenOrNull(HttpContext httpContext);
}