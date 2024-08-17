using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;
using Volo.Abp.Json;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form.DatePicker;

public class AbpDatePickerTagHelperService : AbpDatePickerBaseTagHelperService<AbpDatePickerTagHelper>
{
    public AbpDatePickerTagHelperService(IJsonSerializer jsonSerializer, IHtmlGenerator generator, HtmlEncoder encoder, IServiceProvider serviceProvider, IStringLocalizer<AbpUiResource> l, IAbpTagHelperLocalizer tagHelperLocalizer) : base(jsonSerializer, generator, encoder, serviceProvider, l, tagHelperLocalizer)
    {

    }

    protected override TagHelperOutput TagHelperOutput { get; set; } = default!;

    protected virtual InputTagHelper? DateTagHelper { get; set; }

    protected virtual TagHelperOutput? DateTagHelperOutput { get; set; }
    protected override string GetPropertyName()
    {
        return TagHelper.AspFor?.Name ?? string.Empty;
    }

    protected override T? GetAttributeAndModelExpression<T>(out ModelExpression? modelExpression) where T : class
    {
        modelExpression = TagHelper.AspFor;
        return modelExpression?.ModelExplorer.GetAttribute<T>();
    }

    public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.AspFor != null)
        {
            DateTagHelper = new InputTagHelper(Generator)
            {
                InputTypeName = "hidden",
                ViewContext = TagHelper.ViewContext,
                For = TagHelper.AspFor,
            };

            var attributes = new TagHelperAttributeList { { "data-date", "true" }, { "type", "hidden" } };
            DateTagHelperOutput = await DateTagHelper.ProcessAndGetOutputAsync(attributes, context, "input");
        }

        await base.ProcessAsync(context, output);
    }


    protected override int GetOrder()
    {
        return TagHelper.AspFor?.Metadata.Order ?? 0;
    }

    protected override void AddBaseTagAttributes(TagHelperAttributeList attributes)
    {
        if (TagHelper.AspFor?.Model != null &&
            SupportedInputTypes.TryGetValue(TagHelper.AspFor.Metadata.ModelType, out var convertFunc))
        {
            var convert = convertFunc(TagHelper.AspFor.Model);
            if(!convert.IsNullOrWhiteSpace())
            {
                attributes.Add("data-date", convert);
            }
        }
    }

    protected override ModelExpression? GetModelExpression()
    {
        return TagHelper.AspFor;
    }

    protected override string GetExtraInputHtml(TagHelperContext context, TagHelperOutput output)
    {
        return DateTagHelperOutput?.Render(Encoder)!;
    }
}