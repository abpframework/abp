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
        private readonly AntiforgeryOptions _antiforgeryOptions;
        private readonly AbpAntiForgeryAuthCookieNameProvider _antiForgeryAuthCookieNameProvider;

        public AbpAutoValidateAntiforgeryTokenAuthorizationFilter(
            IAntiforgery antiforgery,
            IOptions<AntiforgeryOptions> antiforgeryOptions,
            AbpAntiForgeryAuthCookieNameProvider antiForgeryAuthCookieNameProvider,
            ILogger<AbpValidateAntiforgeryTokenAuthorizationFilter> logger)
            : base(
                antiforgery,
                antiforgeryOptions,
                antiForgeryAuthCookieNameProvider,
                logger)
        {
            _antiForgeryAuthCookieNameProvider = antiForgeryAuthCookieNameProvider;
            _antiforgeryOptions = antiforgeryOptions.Value;
        }

        protected override bool ShouldValidate(AuthorizationFilterContext context)
        {
            if (!ShouldValidateInternal(context))
            {
                return false;
            }

            var authCookieName = _antiForgeryAuthCookieNameProvider.GetNameOrNull();

            //Always perform antiforgery validation when request contains authentication cookie
            if (authCookieName != null &&
                context.HttpContext.Request.Cookies.ContainsKey(authCookieName))
            {
                return true;
            }

            //No need to validate if antiforgery cookie is not sent.
            //That means the request is sent from a non-browser client.
            //See https://github.com/aspnet/Antiforgery/issues/115
            if (!context.HttpContext.Request.Cookies.ContainsKey(_antiforgeryOptions.Cookie.Name))
            {
                return false;
            }

            // Anything else requires a token.
            return true;
        }

        private static bool ShouldValidateInternal(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var method = context.HttpContext.Request.Method;
            if (string.Equals("GET", method, StringComparison.OrdinalIgnoreCase) ||
                string.Equals("HEAD", method, StringComparison.OrdinalIgnoreCase) ||
                string.Equals("TRACE", method, StringComparison.OrdinalIgnoreCase) ||
                string.Equals("OPTIONS", method, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Anything else requires a token.
            return true;
        }
    }
}
