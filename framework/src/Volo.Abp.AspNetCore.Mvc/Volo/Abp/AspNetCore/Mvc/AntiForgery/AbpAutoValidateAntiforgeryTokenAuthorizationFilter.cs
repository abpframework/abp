using System;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery
{
    public class AbpAutoValidateAntiforgeryTokenAuthorizationFilter : AbpValidateAntiforgeryTokenAuthorizationFilter, ITransientDependency
    {
        private readonly AbpAntiForgeryOptions _options;

        public AbpAutoValidateAntiforgeryTokenAuthorizationFilter(
            IAntiforgery antiforgery,
            AbpAntiForgeryCookieNameProvider antiForgeryCookieNameProvider,
            IOptions<AbpAntiForgeryOptions> options,
            ILogger<AbpValidateAntiforgeryTokenAuthorizationFilter> logger)
            : base(
                antiforgery,
                antiForgeryCookieNameProvider,
                logger)
        {
            _options = options.Value;
        }

        protected override bool ShouldValidate(AuthorizationFilterContext context)
        {
            if (!_options.AutoValidate)
            {
                return false;
            }

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
