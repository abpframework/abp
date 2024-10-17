using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form.DatePicker;

[HtmlTargetElement("abp-date-picker", TagStructure = TagStructure.NormalOrSelfClosing)]
public class AbpDatePickerTagHelper : AbpDatePickerBaseTagHelper<AbpDatePickerTagHelper>
{
    public ModelExpression? AspFor { get; set; }
    
    public AbpDatePickerTagHelper(AbpDatePickerTagHelperService service) : base(service)
    {
    }
}