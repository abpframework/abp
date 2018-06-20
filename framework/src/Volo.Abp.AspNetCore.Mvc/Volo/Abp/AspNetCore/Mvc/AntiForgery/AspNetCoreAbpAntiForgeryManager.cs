using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery
{
    public class AspNetCoreAbpAntiForgeryManager : IAbpAntiForgeryManager, ITransientDependency
    {
        public AbpAntiForgeryOptions Options { get; }

        public HttpContext HttpContext => _httpContextAccessor.HttpContext;

        private readonly IAntiforgery _antiforgery;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetCoreAbpAntiForgeryManager(
            IAntiforgery antiforgery,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AbpAntiForgeryOptions> options)
        {
            _antiforgery = antiforgery;
            _httpContextAccessor = httpContextAccessor;
            Options = options.Value;
        }

        public void SetCookie()
        {
            HttpContext.Response.Cookies.Append(Options.TokenCookieName, GenerateToken());
        }

        public string GenerateToken()
        {
            return _antiforgery.GetAndStoreTokens(_httpContextAccessor.HttpContext).RequestToken;
        }
    }
}