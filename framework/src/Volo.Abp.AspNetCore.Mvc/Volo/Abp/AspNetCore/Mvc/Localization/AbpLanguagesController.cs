﻿using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.RequestLocalization;
using Volo.Abp.Auditing;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

[Area("Abp")]
[Route("Abp/Languages/[action]")]
[DisableAuditing]
[RemoteService(false)]
[ApiExplorerSettings(IgnoreApi = true)]
public class AbpLanguagesController : AbpController
{
    protected IQueryStringCultureReplacement QueryStringCultureReplacement { get; }

    public AbpLanguagesController(IQueryStringCultureReplacement queryStringCultureReplacement)
    {
        QueryStringCultureReplacement = queryStringCultureReplacement;
    }

    [HttpGet]
    public virtual async Task<IActionResult> Switch(string culture, string uiCulture = "", string returnUrl = "")
    {
        if (!CultureHelper.IsValidCultureCode(culture))
        {
            throw new AbpException("The selected culture is not valid! Make sure you enter a valid culture code.");
        }

        AbpRequestCultureCookieHelper.SetCultureCookie(
            HttpContext,
            new RequestCulture(culture, uiCulture)
        );

        HttpContext.Items[AbpRequestLocalizationMiddleware.HttpContextItemName] = true;

        var context = new QueryStringCultureReplacementContext(HttpContext, new RequestCulture(culture, uiCulture), returnUrl);
        await QueryStringCultureReplacement.ReplaceAsync(context);

        if (!string.IsNullOrWhiteSpace(context.ReturnUrl))
        {
            return Redirect(GetRedirectUrl(context.ReturnUrl));
        }

        return Redirect("~/");
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
