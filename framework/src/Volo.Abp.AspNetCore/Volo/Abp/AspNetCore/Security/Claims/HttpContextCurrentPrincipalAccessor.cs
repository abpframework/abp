using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Security.Claims
{
    public class HttpContextCurrentPrincipalAccessor : ThreadCurrentPrincipalAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCurrentPrincipalAccessor(IHttpContextAccessor httpContextAccessor, ILogger<HttpContextCurrentPrincipalAccessor> logger) : base(logger)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override ClaimsPrincipal GetClaimsPrincipal()
        {
            Logger.LogInformation($"HttpContextCurrentPrincipalAccessor:ManagedThreadId={System.Threading.Thread.CurrentThread.ManagedThreadId}");
            return _httpContextAccessor.HttpContext?.User ?? base.GetClaimsPrincipal();
        }
    }
}
