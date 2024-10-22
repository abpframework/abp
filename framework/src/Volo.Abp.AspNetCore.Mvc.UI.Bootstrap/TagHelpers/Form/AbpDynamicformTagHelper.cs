using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

[HtmlTargetElement("abp-dynamic-form", TagStructure = TagStructure.NormalOrSelfClosing)]
public class AbpDynamicFormTagHelper : AbpTagHelper<AbpDynamicFormTagHelper, AbpDynamicFormTagHelperService>
{
    [HtmlAttributeName("abp-model")]
    public ModelExpression Model { get; set; } = default!;

    [HtmlAttributeName("column-size")]
    public ColumnSize ColumnSize { get; set; }

    public bool? SubmitButton { get; set; }

    public bool? RequiredSymbols { get; set; } = true;

    #region MvcFormTagHelperAttiributes
    
    private IDictionary<string, string>? _routeValues;

    private const string ActionAttributeName = "asp-action";
    private const string AreaAttributeName = "asp-area";
    private const string PageAttributeName = "asp-page";
    private const string PageHandlerAttributeName = "asp-page-handler";
    private const string FragmentAttributeName = "asp-fragment";
    private const string ControllerAttributeName = "asp-controller";
    private const string RouteAttributeName = "asp-route";
    private const string RouteValuesDictionaryName = "asp-all-route-data";
    private const string RouteValuesPrefix = "asp-route-";

    [HtmlAttributeName(ActionAttributeName)]
    public string Action { get; set; } = default!;

    [HtmlAttributeName(ControllerAttributeName)]
    public string Controller { get; set; } = default!;

    [HtmlAttributeName(AreaAttributeName)]
    public string Area { get; set; } = default!;

    [HtmlAttributeName(PageAttributeName)]
    public string Page { get; set; } = default!;

    [HtmlAttributeName(PageHandlerAttributeName)]
    public string PageHandler { get; set; } = default!;

    [HtmlAttributeName(FragmentAttributeName)]
    public string Fragment { get; set; } = default!;

    [HtmlAttributeName(RouteAttributeName)]
    public string Route { get; set; } = default!;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public string Method { get; set; } = default!;

    [HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
    public IDictionary<string, string> RouteValues
    {
        get
        {
            if (_routeValues == null)
            {
                _routeValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            return _routeValues;
        }
        set
        {
            _routeValues = value;
        }
    }

    #endregion

    public AbpDynamicFormTagHelper(AbpDynamicFormTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
