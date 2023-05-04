using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Volo.Abp.AspNetCore.Security;

public static class NonceHtmlHelper
{
    public static IHtmlContent ScriptWithNonce(this HtmlHelper htmlHelper, string scriptUrl)
    {
        var tagBuilder = new TagBuilder("script");
        tagBuilder.Attributes.Add("src", scriptUrl);
        if(htmlHelper.ViewContext.HttpContext.Items.TryGetValue(AbpAspNetCoreConsts.ScriptNonceKey, out var nonce) && nonce is string nonceString && !string.IsNullOrEmpty(nonceString))
        {
            tagBuilder.Attributes.Add("nonce", nonceString);
        }

        return tagBuilder;
    }
}