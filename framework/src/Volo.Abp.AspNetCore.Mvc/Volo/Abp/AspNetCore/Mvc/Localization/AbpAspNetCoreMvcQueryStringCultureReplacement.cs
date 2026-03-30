using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

public partial class AbpAspNetCoreMvcQueryStringCultureReplacement : IQueryStringCultureReplacement, ITransientDependency
{
    private static readonly Regex CultureQueryStringRegex = GetCultureQueryStringRegex();
    private static readonly Regex UiCultureQueryStringRegex = GetUiCultureQueryStringRegex();

    [GeneratedRegex("culture=[A-Za-z-]+", RegexOptions.IgnoreCase)]
    private static partial Regex GetCultureQueryStringRegex();

    [GeneratedRegex("ui-culture=[A-Za-z-]+", RegexOptions.IgnoreCase)]
    private static partial Regex GetUiCultureQueryStringRegex();

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
            context.ReturnUrl = CultureQueryStringRegex.Replace(
                context.ReturnUrl,
                $"culture={context.RequestCulture.Culture}");

            context.ReturnUrl = UiCultureQueryStringRegex.Replace(
                context.ReturnUrl,
                $"ui-culture={context.RequestCulture.UICulture}");
        }

        return Task.CompletedTask;
    }
}
