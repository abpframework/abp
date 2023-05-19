using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Views.Error;
using Volo.Abp.ExceptionHandling;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Controllers;

public class ErrorController : AbpController
{
    protected readonly IExceptionToErrorInfoConverter ErrorInfoConverter;
    protected readonly IHttpExceptionStatusCodeFinder StatusCodeFinder;
    protected readonly IStringLocalizer<AbpUiResource> Localizer;
    protected readonly AbpErrorPageOptions AbpErrorPageOptions;
    protected readonly IExceptionNotifier ExceptionNotifier;
    protected readonly AbpExceptionHandlingOptions ExceptionHandlingOptions;

    public ErrorController(
        IExceptionToErrorInfoConverter exceptionToErrorInfoConverter,
        IHttpExceptionStatusCodeFinder httpExceptionStatusCodeFinder,
        IOptions<AbpErrorPageOptions> abpErrorPageOptions,
        IStringLocalizer<AbpUiResource> localizer,
        IExceptionNotifier exceptionNotifier,
        IOptions<AbpExceptionHandlingOptions> exceptionHandlingOptions)
    {
        ErrorInfoConverter = exceptionToErrorInfoConverter;
        StatusCodeFinder = httpExceptionStatusCodeFinder;
        Localizer = localizer;
        ExceptionNotifier = exceptionNotifier;
        ExceptionHandlingOptions = exceptionHandlingOptions.Value;
        AbpErrorPageOptions = abpErrorPageOptions.Value;
    }

    public virtual async Task<IActionResult> Index(int httpStatusCode)
    {
        var exHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

        var exception = exHandlerFeature != null
            ? exHandlerFeature.Error
            : new Exception(Localizer["UnhandledException"]);

        await ExceptionNotifier.NotifyAsync(new ExceptionNotificationContext(exception));

        var errorInfo = ErrorInfoConverter.Convert(exception, options =>
        {
            options.SendExceptionsDetailsToClients = ExceptionHandlingOptions.SendExceptionsDetailsToClients;
            options.SendStackTraceToClients = ExceptionHandlingOptions.SendStackTraceToClients;
        });

        if (httpStatusCode == 0)
        {
            httpStatusCode = (int)StatusCodeFinder.GetStatusCode(HttpContext, exception);
        }

        HttpContext.Response.StatusCode = httpStatusCode;

        var page = GetErrorPageUrl(httpStatusCode);

        return View(page, new AbpErrorViewModel
        {
            ErrorInfo = errorInfo,
            HttpStatusCode = httpStatusCode
        });
    }

    protected virtual string GetErrorPageUrl(int statusCode)
    {
        var page = AbpErrorPageOptions.ErrorViewUrls.GetOrDefault(statusCode.ToString());

        if (string.IsNullOrWhiteSpace(page))
        {
            return "~/Views/Error/Default.cshtml";
        }

        return page;
    }
}
