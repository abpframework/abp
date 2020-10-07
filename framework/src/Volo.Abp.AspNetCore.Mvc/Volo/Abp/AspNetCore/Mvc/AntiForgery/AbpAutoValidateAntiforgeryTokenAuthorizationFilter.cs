using System;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery
{
    public class AbpAutoValidateAntiforgeryTokenAuthorizationFilter : AbpValidateAntiforgeryTokenAuthorizationFilter, ITransientDependency
    {
        public AbpAutoValidateAntiforgeryTokenAuthorizationFilter(
            IAntiforgery antiforgery,
            AbpAntiForgeryCookieNameProvider antiForgeryCookieNameProvider,
            ILogger<AbpValidateAntiforgeryTokenAuthorizationFilter> logger)
            : base(
                antiforgery,
                antiForgeryCookieNameProvider,
                logger)
        {

        }

        protected override bool ShouldValidate(AuthorizationFilterContext context)
        {
            if (IsIgnoredHttpMethod(context))
            {
                return false;
            }

            return base.ShouldValidate(context);
        }

        protected virtual bool IsIgnoredHttpMethod(AuthorizationFilterContext context)
        {
            var method = context.HttpContext.Request.Method;
            return string.Equals("GET", method, StringComparison.OrdinalIgnoreCase) ||
                   string.Equals("HEAD", method, StringComparison.OrdinalIgnoreCase) ||
                   string.Equals("TRACE", method, StringComparison.OrdinalIgnoreCase) ||
                   string.Equals("OPTIONS", method, StringComparison.OrdinalIgnoreCase);
        }
    }
}
