using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;
using Volo.Abp.Json;
using Volo.Abp.Timing;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form.DatePicker;

public class AbpDateRangePickerTagHelperService : AbpDatePickerBaseTagHelperService<AbpDateRangePickerTagHelper>
{
    public AbpDateRangePickerTagHelperService(
        IJsonSerializer jsonSerializer, IHtmlGenerator generator,
        HtmlEncoder encoder,
        IServiceProvider serviceProvider,
        IStringLocalizer<AbpUiResource> l,
        IAbpTagHelperLocalizer tagHelperLocalizer,
        IClock clock) :
        base(jsonSerializer, generator, encoder, serviceProvider, l, tagHelperLocalizer, clock)
    {
    }

    protected override string TagName { get; set; } = "abp-date-range-picker";

    protected override T? GetAttributeAndModelExpression<T>(out ModelExpression? modelExpression) where T : class
    {
        modelExpression = new[] { TagHelper.AspForStart, TagHelper.AspForEnd }.FirstOrDefault(x => x?.ModelExplorer?.GetAttribute<T>() != null);
        return modelExpression?.ModelExplorer.GetAttribute<T>();
    }

    public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.AspForStart != null)
        {
            var startDateAttributes = new TagHelperAttributeList { { "data-hidden-datepicker", "true" }, { "data-start-date", "true" }, { "type", "hidden" } };
            StartDateTagHelper = new InputTagHelper(Generator)
            {
                ViewContext = TagHelper.ViewContext,
                For = TagHelper.AspForStart,
                InputTypeName = "hidden"
            };

            if (Clock.SupportsMultipleTimezone)
            {
                if (TagHelper.AspForStart.Model is DateTime dateTime)
                {
                    StartDateTagHelper.Format = "{0:O}";
                    StartDateTagHelper.Value = Clock.ConvertToUserTime(dateTime).ToString("O");
                    startDateAttributes.Add("value", StartDateTagHelper.Value);
                }

                if (TagHelper.AspForStart.Model is DateTimeOffset dateTimeOffset)
                {
                    StartDateTagHelper.Format = "{0:O}";
                    StartDateTagHelper.Value = Clock.ConvertToUserTime(dateTimeOffset).UtcDateTime.ToString("O");
                    startDateAttributes.Add("value", StartDateTagHelper.Value);
                }
            }

            StartDateTagHelperOutput = await StartDateTagHelper.ProcessAndGetOutputAsync(startDateAttributes, context, "input");
        }

        if (TagHelper.AspForEnd != null)
        {
            var endDateAttributes = new TagHelperAttributeList { { "data-hidden-datepicker", "true" }, { "data-end-date", "true" }, { "type", "hidden" } };
            EndDateTagHelper = new InputTagHelper(Generator)
            {
                ViewContext = TagHelper.ViewContext,
                For = TagHelper.AspForEnd,
                InputTypeName = "hidden"
            };

            if (Clock.SupportsMultipleTimezone)
            {
                if (TagHelper.AspForEnd.Model is DateTime dateTime)
                {
                    EndDateTagHelper.Format = "{0:O}";
                    EndDateTagHelper.Value = Clock.ConvertToUserTime(dateTime).ToString("O");
                    endDateAttributes.Add("value", EndDateTagHelper.Value);
                }

                if (TagHelper.AspForEnd.Model is DateTimeOffset dateTimeOffset)
                {
                    EndDateTagHelper.Format = "{0:O}";
                    EndDateTagHelper.Value = Clock.ConvertToUserTime(dateTimeOffset).UtcDateTime.ToString("O");
                    endDateAttributes.Add("value", EndDateTagHelper.Value);
                }
            }

            EndDateTagHelperOutput = await EndDateTagHelper.ProcessAndGetOutputAsync(endDateAttributes, context, "input");
        }

        await base.ProcessAsync(context, output);
    }

    protected override TagHelperOutput TagHelperOutput { get; set; } = default!;

    protected virtual InputTagHelper? StartDateTagHelper { get; set; }

    protected virtual TagHelperOutput? StartDateTagHelperOutput { get; set; }

    protected virtual InputTagHelper? EndDateTagHelper { get; set; }

    protected virtual TagHelperOutput? EndDateTagHelperOutput { get; set; }

    protected override string GetPropertyName()
    {
        return TagHelper.AspForStart?.Name ?? string.Empty;
    }

    protected override int GetOrder()
    {
        return TagHelper.AspForStart?.Metadata.Order ?? 0;
    }

    protected override void AddBaseTagAttributes(TagHelperAttributeList attributes)
    {
        if (TagHelper.AspForStart?.Model != null &&
            SupportedInputTypes.TryGetValue(TagHelper.AspForStart.Metadata.ModelType, out var convertFuncStart))
        {
            var convert = convertFuncStart(TagHelper.AspForStart.Model);
            if(!convert.IsNullOrWhiteSpace())
            {
                attributes.Add("data-start-date", convert);
            }
        }

        if (TagHelper.AspForEnd?.Model != null &&
            SupportedInputTypes.TryGetValue(TagHelper.AspForEnd.Metadata.ModelType, out var convertFuncEnd))
        {
            var convert = convertFuncEnd(TagHelper.AspForEnd.Model);
            if(!convert.IsNullOrWhiteSpace())
            {
                attributes.Add("data-end-date", convert);
            }
        }
    }

    protected override string GetExtraInputHtml(TagHelperContext context, TagHelperOutput output)
    {
        return StartDateTagHelperOutput?.Render(Encoder) + EndDateTagHelperOutput?.Render(Encoder);
    }

    protected override ModelExpression? GetModelExpression()
    {
        return TagHelper.AspForStart ?? TagHelper.AspForEnd;
    }
}
