using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

/// <summary>
/// Adds a {culture}-prefixed route selector to every Razor Page.
/// Automatically registered when <c>UseRouteBasedCulture</c> is <c>true</c>.
/// </summary>
public class AbpCultureRoutePagesConvention : IPageRouteModelConvention
{
    /// <summary>
    /// Route parameter template for culture with a regex constraint matching IETF BCP 47 language tags
    /// (e.g. "en", "zh-Hans", "sr-Latn-RS"). The double braces are required by the route template
    /// parser to represent literal { } characters inside the regex constraint.
    /// </summary>
    internal const string CultureRouteTemplate = "{culture:regex(^[a-zA-Z]{{2,8}}(-[a-zA-Z0-9]{{1,8}})*$)}";

    public void Apply(PageRouteModel model)
    {
        var selectorsToAdd = new List<SelectorModel>();

        foreach (var selector in model.Selectors.ToList())
        {
            var originalTemplate = selector.AttributeRouteModel?.Template?.TrimStart('/');
            if (originalTemplate == null)
            {
                continue;
            }

            selectorsToAdd.Add(new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel
                {
                    Template = AttributeRouteModel.CombineTemplates(CultureRouteTemplate, originalTemplate),
                    Order = -1
                }
            });
        }

        foreach (var selector in selectorsToAdd)
        {
            model.Selectors.Add(selector);
        }
    }
}
