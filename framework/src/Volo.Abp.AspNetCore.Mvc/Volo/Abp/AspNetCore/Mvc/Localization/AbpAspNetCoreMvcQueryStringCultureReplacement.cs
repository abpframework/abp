using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

public class AbpAspNetCoreMvcQueryStringCultureReplacement : IQueryStringCultureReplacement, ITransientDependency
{
    public virtual Task ReplaceAsync(QueryStringCultureReplacementContext context)
    {
        if (string.IsNullOrWhiteSpace(context.ReturnUrl))
        {
            return Task.CompletedTask;
        }

        var currentCulture = context.CurrentCulture
            ?? context.HttpContext.GetRouteValue("culture")?.ToString();

        if (!string.IsNullOrEmpty(currentCulture))
        {
            var escapedCulture = Regex.Escape(currentCulture);
            var pattern = $"/{escapedCulture}(?=/|$|\\?|#)";
            context.ReturnUrl = Regex.Replace(
                context.ReturnUrl,
                pattern,
                "/" + context.RequestCulture.Culture.Name,
                RegexOptions.IgnoreCase);
        }

        if (context.ReturnUrl.Contains("culture=", StringComparison.OrdinalIgnoreCase) &&
            context.ReturnUrl.Contains("ui-Culture=", StringComparison.OrdinalIgnoreCase))
        {
            context.ReturnUrl = Regex.Replace(
                context.ReturnUrl,
                "culture=[A-Za-z-]+",
                $"culture={context.RequestCulture.Culture}",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            context.ReturnUrl = Regex.Replace(
                context.ReturnUrl,
                "ui-culture=[A-Za-z-]+",
                $"ui-culture={context.RequestCulture.UICulture}",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        return Task.CompletedTask;
    }
}
