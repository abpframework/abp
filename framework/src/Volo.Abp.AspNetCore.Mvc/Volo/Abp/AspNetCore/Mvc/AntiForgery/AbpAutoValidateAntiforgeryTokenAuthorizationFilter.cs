using System;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Abstractions;
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

            if(context.ActionDescriptor.IsControllerAction())
            {
                var controllerType = context.ActionDescriptor
                    .AsControllerActionDescriptor()
                    .ControllerTypeInfo
                    .AsType();

                if (!_options.AutoValidateFilter(controllerType))
                {
                    return false;
                }
            }

            if (IsIgnoredHttpMethod(context))
            {
                return false;
            }

            return base.ShouldValidate(context);
        }

        protected virtual bool IsIgnoredHttpMethod(AuthorizationFilterContext context)
        {
            return context.HttpContext
                .Request
                .Method
                .ToUpperInvariant()
                .IsIn(_options.AutoValidateIgnoredHttpMethods);
        }
    }
}
