using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery
{
    public class AbpValidateAntiforgeryTokenAuthorizationFilter : IAsyncAuthorizationFilter, IAntiforgeryPolicy, ITransientDependency
    {
        private IAntiforgery _antiforgery;
        private readonly AntiforgeryOptions _antiforgeryOptions;
        private readonly AbpAntiForgeryAuthCookieNameProvider _antiForgeryAuthCookieNameProvider;
        private readonly ILogger<AbpValidateAntiforgeryTokenAuthorizationFilter> _logger;

        public AbpValidateAntiforgeryTokenAuthorizationFilter(
            IAntiforgery antiforgery,
            IOptions<AntiforgeryOptions> antiforgeryOptions,
            AbpAntiForgeryAuthCookieNameProvider antiForgeryAuthCookieNameProvider,
            ILogger<AbpValidateAntiforgeryTokenAuthorizationFilter> logger)
        {
            _antiforgery = antiforgery;
            _antiforgeryOptions = antiforgeryOptions.Value;
            _logger = logger;
            _antiForgeryAuthCookieNameProvider = antiForgeryAuthCookieNameProvider;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.IsEffectivePolicy<IAntiforgeryPolicy>(this))
            {
                _logger.LogInformation("Skipping the execution of current filter as its not the most effective filter implementing the policy " + typeof(IAntiforgeryPolicy));
                return;
            }

            if (ShouldValidate(context))
            {
                try
                {
                    await _antiforgery.ValidateRequestAsync(context.HttpContext);
                }
                catch (AntiforgeryValidationException exception)
                {
                    _logger.LogError(exception.Message, exception);
                    context.Result = new AntiforgeryValidationFailedResult();
                }
            }
        }

        protected virtual bool ShouldValidate(AuthorizationFilterContext context)
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

            return true;
        }
    }
}
