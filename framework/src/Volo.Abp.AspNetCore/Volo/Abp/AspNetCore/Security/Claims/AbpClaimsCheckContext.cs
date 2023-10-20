using Microsoft.AspNetCore.Http;

namespace Volo.Abp.AspNetCore.Security.Claims;

public class AbpClaimsCheckContext
{
    public HttpContext HttpContext { get; }

    public bool CancelAuthentication { get; set; }

    public AbpClaimsCheckContext(HttpContext httpContext)
    {
        HttpContext = httpContext;
        CancelAuthentication = false;
    }
}
