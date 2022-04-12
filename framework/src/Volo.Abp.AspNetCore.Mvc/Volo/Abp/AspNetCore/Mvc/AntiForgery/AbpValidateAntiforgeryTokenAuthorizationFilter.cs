using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery
{
    public class AbpValidateAntiforgeryTokenAuthorizationFilter : IAsyncAuthorizationFilter, IAntiforgeryPolicy, ITransientDependency
    {
        private IAntiforgery _antiforgery;
        private readonly AbpAntiForgeryCookieNameProvider _antiForgeryCookieNameProvider;
        private readonly ILogger<AbpValidateAntiforgeryTokenAuthorizationFilter> _logger;

        public AbpValidateAntiforgeryTokenAuthorizationFilter(
            IAntiforgery antiforgery,
            AbpAntiForgeryCookieNameProvider antiForgeryCookieNameProvider,
            ILogger<AbpValidateAntiforgeryTokenAuthorizationFilter> logger)
        {
            _antiforgery = antiforgery;
            _logger = logger;
            _antiForgeryCookieNameProvider = antiForgeryCookieNameProvider;
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

            if (await ShouldValidate(context))
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

        protected virtual Task<bool> ShouldValidate(AuthorizationFilterContext context)
        {
            var authCookieName = _antiForgeryCookieNameProvider.GetAuthCookieNameOrNull();

            //Always perform antiforgery validation when request contains authentication cookie
            if (authCookieName != null &&
                context.HttpContext.Request.Cookies.ContainsKey(authCookieName))
            {
                return Task.FromResult(true);
            }

            var antiForgeryCookieName = _antiForgeryCookieNameProvider.GetAntiForgeryCookieNameOrNull();

            //No need to validate if antiforgery cookie is not sent.
            //That means the request is sent from a non-browser client.
            //See https://github.com/aspnet/Antiforgery/issues/115
            if (antiForgeryCookieName != null &&
                !context.HttpContext.Request.Cookies.ContainsKey(antiForgeryCookieName))
            {
                return Task.FromResult(false);
            }

            // Anything else requires a token.
            return Task.FromResult(true);
        }
    }
}
