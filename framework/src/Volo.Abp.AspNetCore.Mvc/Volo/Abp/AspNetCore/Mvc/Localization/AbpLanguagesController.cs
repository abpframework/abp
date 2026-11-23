using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RequestLocalization;
using Volo.Abp.Auditing;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

[Area("Abp")]
[Route("Abp/Languages/[action]")]
[DisableAuditing]
[RemoteService(false)]
[ApiExplorerSettings(IgnoreApi = true)]
public class AbpLanguagesController : AbpController
{
    protected IQueryStringCultureReplacement QueryStringCultureReplacement { get; }
    protected AbpRequestCultureResolver RequestCultureResolver { get; }

    public AbpLanguagesController(
        IQueryStringCultureReplacement queryStringCultureReplacement,
        AbpRequestCultureResolver requestCultureResolver)
    {
        QueryStringCultureReplacement = queryStringCultureReplacement;
        RequestCultureResolver = requestCultureResolver;
    }

    [HttpGet]
    public virtual async Task<IActionResult> Switch(string culture, string uiCulture = "", string returnUrl = "")
    {
        var resolvedCulture = await RequestCultureResolver.ResolveAsync(culture, uiCulture: false);
        var resolvedUiCulture = await RequestCultureResolver.ResolveAsync(uiCulture, uiCulture: true);
        if (resolvedCulture == null || resolvedUiCulture == null)
        {
            throw new AbpValidationException(new[]
            {
                new ValidationResult("The selected culture is not supported.",
                    new[] { resolvedCulture == null ? nameof(culture) : nameof(uiCulture) })
            });
        }

        var requestCulture = new RequestCulture(resolvedCulture, resolvedUiCulture);
        AbpRequestCultureCookieHelper.SetCultureCookie(
            HttpContext,
            requestCulture
        );

        HttpContext.Items[AbpRequestLocalizationMiddleware.HttpContextItemName] = true;

        var context = new QueryStringCultureReplacementContext(
            HttpContext,
            requestCulture,
            returnUrl,
            GetCurrentCultureFromRequestCookie());

        await QueryStringCultureReplacement.ReplaceAsync(context);

        if (!string.IsNullOrWhiteSpace(context.ReturnUrl))
        {
            return Redirect(GetRedirectUrl(context.ReturnUrl));
        }

        return Redirect("~/");
    }

    protected virtual string? GetCurrentCultureFromRequestCookie()
    {
        var cookieValue = HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
        if (cookieValue == null)
        {
            return null;
        }

        var result = CookieRequestCultureProvider.ParseCookieValue(cookieValue);
        return result?.Cultures.FirstOrDefault().Value;
    }

    protected virtual string GetRedirectUrl(string returnUrl)
    {
        if (returnUrl.IsNullOrEmpty())
        {
            return "~/";
        }

        if (Url.IsLocalUrl(returnUrl))
        {
            return returnUrl;
        }

        return "~/";
    }
}
