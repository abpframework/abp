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
    /// Route parameter template using the custom "culture" route constraint registered in
    /// <see cref="AbpCultureRouteConstraint"/>. The constraint only matches culture values
    /// configured in <see cref="Volo.Abp.Localization.AbpLocalizationOptions.Languages"/>.
    /// </summary>
    internal const string CultureRouteTemplate = "{culture:culture}";

    public virtual void Apply(PageRouteModel model)
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
