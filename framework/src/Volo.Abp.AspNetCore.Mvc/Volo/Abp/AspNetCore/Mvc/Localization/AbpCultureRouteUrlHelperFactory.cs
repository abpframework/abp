using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

/// <summary>
/// Wraps the default <see cref="UrlHelperFactory"/> to automatically inject the culture
/// route value into all URL generation calls when the current request has a {culture} route value.
/// Only activates when <see cref="AbpRequestLocalizationOptions.UseRouteBasedCulture"/> is <c>true</c>.
/// </summary>
public class AbpCultureRouteUrlHelperFactory : IUrlHelperFactory
{
    protected UrlHelperFactory Inner { get; }
    protected IOptions<AbpRequestLocalizationOptions> LocalizationOptions { get; }

    public AbpCultureRouteUrlHelperFactory(
        UrlHelperFactory inner,
        IOptions<AbpRequestLocalizationOptions> localizationOptions)
    {
        Inner = inner;
        LocalizationOptions = localizationOptions;
    }

    public virtual IUrlHelper GetUrlHelper(ActionContext context)
    {
        var urlHelper = Inner.GetUrlHelper(context);

        if (!LocalizationOptions.Value.UseRouteBasedCulture)
        {
            return urlHelper;
        }

        if (context.RouteData.Values.TryGetValue("culture", out var culture) &&
            culture != null)
        {
            return CreateCultureAwareUrlHelper(urlHelper, culture.ToString()!);
        }

        return urlHelper;
    }

    protected virtual AbpCultureAwareUrlHelper CreateCultureAwareUrlHelper(IUrlHelper urlHelper, string culture)
    {
        return new AbpCultureAwareUrlHelper(urlHelper, culture);
    }
}
