using Microsoft.AspNetCore.Http;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Security.Claims
{
    public class HttpContextCurrentPrincipalAccessor : ThreadCurrentPrincipalAccessor
    {
        public HttpContextCurrentPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            CurrentScope.Value = () => httpContextAccessor.HttpContext?.User ?? GetThreadClaimsPrincipal();
        }
    }
}
