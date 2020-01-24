using System;
using System.Collections.Generic;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Views.Error;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Controllers
{
    public class ErrorController : AbpController
    {
        private readonly IExceptionToErrorInfoConverter _errorInfoConverter;
        private readonly IHttpExceptionStatusCodeFinder _statusCodeFinder;
        private readonly IStringLocalizer<AbpUiResource> _localizer;
        private readonly AbpErrorPageOptions _abpErrorPageOptions;

        public ErrorController(
            IExceptionToErrorInfoConverter exceptionToErrorInfoConverter,
            IHttpExceptionStatusCodeFinder httpExceptionStatusCodeFinder,
            IOptions<AbpErrorPageOptions> abpErrorPageOptions,
            IStringLocalizer<AbpUiResource> localizer)
        {
            _errorInfoConverter = exceptionToErrorInfoConverter;
            _statusCodeFinder = httpExceptionStatusCodeFinder;
            _localizer = localizer;
            _abpErrorPageOptions = abpErrorPageOptions.Value;
        }

        public IActionResult Index(int httpStatusCode)
        {
            var exHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            var exception = exHandlerFeature != null
                ? exHandlerFeature.Error
                : new Exception(_localizer["UnhandledException"]);

            var errorInfo = _errorInfoConverter.Convert(exception);

            if (httpStatusCode == 0)
            {
                httpStatusCode = (int)_statusCodeFinder.GetStatusCode(HttpContext, exception);
            }

            HttpContext.Response.StatusCode = httpStatusCode;

            var page = GetErrorPageUrl(httpStatusCode);

            return View(page, new AbpErrorViewModel
            {
                ErrorInfo = errorInfo,
                HttpStatusCode = httpStatusCode
            });
        }

        private string GetErrorPageUrl(int statusCode)
        {
            var page = _abpErrorPageOptions.ErrorViewUrls.GetOrDefault(statusCode.ToString());

            if (string.IsNullOrWhiteSpace(page))
            {
                return "~/Views/Error/Default.cshtml";
            }

            return page;
        }
    }
}
