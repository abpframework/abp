using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

[HtmlTargetElement("abp-dynamic-text", TagStructure = TagStructure.NormalOrSelfClosing)]
public class AbpDynamicTextTagHelper : AbpTagHelper<AbpDynamicTextTagHelper, AbpDynamicTextTagHelperService>
{
    [HtmlAttributeName("abp-model")]
    public ModelExpression Model { get; set; } = default!;

    [HtmlAttributeName("column-size")]
    public ColumnSize ColumnSize { get; set; }

    [HtmlAttributeName("label-width")]
    public ColumnSize LabelWidth { get; set; } = ColumnSize._4;

    public AbpDynamicTextTagHelper(AbpDynamicTextTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
