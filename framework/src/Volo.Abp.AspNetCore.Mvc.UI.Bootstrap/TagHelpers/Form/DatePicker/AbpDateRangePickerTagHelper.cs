using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form.DatePicker;

[HtmlTargetElement("abp-date-range-picker", TagStructure = TagStructure.NormalOrSelfClosing)]
public class AbpDateRangePickerTagHelper : AbpDatePickerBaseTagHelper<AbpDateRangePickerTagHelper>
{
    public ModelExpression? AspForStart { get; set; }

    public ModelExpression? AspForEnd { get; set; }

    public AbpDateRangePickerTagHelper(
        AbpDateRangePickerTagHelperService service,
        IOptionsFactory<AbpDatePickerOptions> optionsFactory)
        : base(service, optionsFactory)
    {
    }
}