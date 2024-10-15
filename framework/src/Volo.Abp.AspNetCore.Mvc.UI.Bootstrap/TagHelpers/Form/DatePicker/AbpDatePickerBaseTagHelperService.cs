using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;
using Volo.Abp.Json;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form.DatePicker;

public abstract class AbpDatePickerBaseTagHelperService<TTagHelper> : AbpTagHelperService<TTagHelper>
    where TTagHelper : AbpDatePickerBaseTagHelper<TTagHelper>
{
    protected readonly Dictionary<Type, Func<object, string>> SupportedInputTypes = new()
    {
        {
            typeof(string), o =>
            {
                if(o is string s && DateTime.TryParse(s, out var dt))
                {
                    return dt.ToString("O");
                }

                return string.Empty;
            }
        },
        {
            typeof(DateTime), o =>
            {
                if(o is DateTime dt && dt != default)
                {
                    return dt.ToString("O");
                }

                return string.Empty;
            }
        },
        {typeof(DateTime?), o => ((DateTime?) o)?.ToString("O")!},
        {
            typeof(DateTimeOffset), o =>
            {
                if(o is DateTimeOffset dto && dto != default)
                {
                    return dto.ToString("O");
                }

                return string.Empty;
            }
        },
        {typeof(DateTimeOffset?), o => ((DateTimeOffset?) o)?.ToString("O")!}
    };

    protected readonly IJsonSerializer JsonSerializer;
    protected readonly IHtmlGenerator Generator;
    protected readonly HtmlEncoder Encoder;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly IAbpTagHelperLocalizer TagHelperLocalizer;
    protected virtual string TagName { get; set; } = "abp-date-picker";
    protected IStringLocalizer<AbpUiResource> L { get; }
    protected abstract TagHelperOutput TagHelperOutput { get; set; }

    protected AbpDatePickerBaseTagHelperService(IJsonSerializer jsonSerializer, IHtmlGenerator generator,
        HtmlEncoder encoder, IServiceProvider serviceProvider, IStringLocalizer<AbpUiResource> l,
        IAbpTagHelperLocalizer tagHelperLocalizer)
    {
        JsonSerializer = jsonSerializer;
        Generator = generator;
        Encoder = encoder;
        ServiceProvider = serviceProvider;
        L = l;
        TagHelperLocalizer = tagHelperLocalizer;
    }

    protected virtual T? GetAttribute<T>() where T : Attribute
    {
        return GetAttributeAndModelExpression<T>(out _);
    }

    protected abstract T? GetAttributeAndModelExpression<T>(out ModelExpression? modelExpression) where T : Attribute;


    public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        TagHelperOutput = TagHelperOutput = new TagHelperOutput("input", GetInputAttributes(context, output), (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        AddDataPickerAttribute(TagHelperOutput);
        AddDisabledAttribute(TagHelperOutput);
        AddAutoFocusAttribute(TagHelperOutput);
        AddFormControls(context, output, TagHelperOutput);
        AddReadOnlyAttribute(TagHelperOutput);
        AddPlaceholderAttribute(TagHelperOutput);
        AddInfoTextId(TagHelperOutput);
        var optionsAttribute = GetAttributeAndModelExpression<DatePickerOptionsAttribute>(out var modelExpression);
        if (optionsAttribute != null)
        {
            TagHelper.SetDatePickerOptions(optionsAttribute.GetDatePickerOptions(modelExpression!.ModelExplorer)!);
        }

        // Open and close button
        var openButtonContent = TagHelper.OpenButton
            ? await ProcessButtonAndGetContentAsync(context, output, "calendar", "open")
            : "";
        var clearButtonContent = TagHelper.ClearButton == true || (!TagHelper.ClearButton.HasValue && TagHelper.AutoUpdateInput != true)
            ? await ProcessButtonAndGetContentAsync(context, output, "times", "clear", visible:!TagHelper.SingleOpenAndClearButton)
            : "";

        var labelContent = await GetLabelAsHtmlAsync(context, output, TagHelperOutput);
        var infoContent = GetInfoAsHtml(context, output, TagHelperOutput);
        var validationContent = await GetValidationAsHtmlAsync(context, TagHelperOutput);

        var inputGroup = new TagHelperOutput("div",
            new TagHelperAttributeList(new[] { new TagHelperAttribute("class", "input-group") }),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
        inputGroup.Content.AppendHtml(
            TagHelperOutput.Render(Encoder) + openButtonContent + clearButtonContent
        );

        var abpDatePickerTag = new TagHelperOutput(TagName, GetBaseTagAttributes(context, output, TagHelper),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
        abpDatePickerTag.Content.AppendHtml(inputGroup.Render(Encoder));
        abpDatePickerTag.Content.AppendHtml(validationContent);
        abpDatePickerTag.Content.AppendHtml(GetExtraInputHtml(context, output));

        var innerHtml = labelContent + abpDatePickerTag.Render(Encoder) + infoContent;

        var order = GetOrder();

        AddGroupToFormGroupContents(
            context,
            GetPropertyName(),
            SurroundInnerHtmlAndGet(context, output, innerHtml),
            order
        );


        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "div";
        LeaveOnlyGroupAttributes(context, output);
        if (TagHelper.AddMarginBottomClass)
        {
            output.Attributes.AddClass("mb-3");
        }

        output.Content.AppendHtml(innerHtml);
    }

    protected virtual void AddReadOnlyAttribute(TagHelperOutput inputTagHelperOutput)
    {
        if (inputTagHelperOutput.Attributes.ContainsName("readonly") == false &&
            (TagHelper.IsReadonly != false || GetAttribute<ReadOnlyInput>() != null))
        {
            inputTagHelperOutput.Attributes.Add("readonly", "");
        }
    }

    protected virtual TagHelperAttributeList GetInputAttributes(TagHelperContext context, TagHelperOutput output)
    {
        var groupPrefix = "group-";

        var tagHelperAttributes = output.Attributes.Where(a => !a.Name.StartsWith(groupPrefix)).ToList();

        var attrList = new TagHelperAttributeList();

        foreach (var tagHelperAttribute in tagHelperAttributes)
        {
            attrList.Add(tagHelperAttribute);
        }

        attrList.Add("type", "text");

        if (attrList.ContainsName("value"))
        {
            attrList.Remove(attrList.First(a => a.Name == "value"));
        }

        if (!TagHelper.Name.IsNullOrEmpty() && !attrList.ContainsName("name"))
        {
            attrList.Add("name", TagHelper.Name);
        }

        if (!attrList.ContainsName("autocomplete"))
        {
            attrList.Add("autocomplete", "off");
        }

        return attrList;
    }
    protected virtual void LeaveOnlyGroupAttributes(TagHelperContext context, TagHelperOutput output)
    {
        var groupPrefix = "group-";
        var tagHelperAttributes = output.Attributes.Where(a => a.Name.StartsWith(groupPrefix)).ToList();

        output.Attributes.Clear();

        foreach (var tagHelperAttribute in tagHelperAttributes)
        {
            var nameWithoutPrefix = tagHelperAttribute.Name.Substring(groupPrefix.Length);
            var newAttribute = new TagHelperAttribute(nameWithoutPrefix, tagHelperAttribute.Value);
            output.Attributes.Add(newAttribute);
        }
    }

    protected virtual string SurroundInnerHtmlAndGet(TagHelperContext context, TagHelperOutput output, string innerHtml)
    {
        var mb = TagHelper.AddMarginBottomClass ? "mb-3" : string.Empty;
        return $"<div class=\"{mb}\">" +
               Environment.NewLine + innerHtml + Environment.NewLine +
               "</div>";
    }

    protected abstract string GetPropertyName();

    protected virtual void AddGroupToFormGroupContents(TagHelperContext context, string propertyName, string html,
        int order)
    {
        var list = context.GetValue<List<FormGroupItem>>(FormGroupContents) ?? new List<FormGroupItem>();

        if (!list.Any(igc => igc.HtmlContent.Contains("id=\"" + propertyName.Replace('.', '_') + "\"")))
        {
            list.Add(new FormGroupItem { HtmlContent = html, Order = order, PropertyName = propertyName });
        }
    }

    protected abstract int GetOrder();
    protected abstract void AddBaseTagAttributes(TagHelperAttributeList attributes);

    protected virtual string GetExtraInputHtml(TagHelperContext context, TagHelperOutput output)
    {
        return string.Empty;
    }

    protected TagHelperAttributeList ConvertDatePickerOptionsToAttributeList(IAbpDatePickerOptions? options)
    {
        var attrList = new TagHelperAttributeList();

        if(options == null)
        {
            return attrList;
        }

        if (options.Locale != null)
        {
            attrList.Add("data-locale", JsonSerializer.Serialize(options.Locale));
        }

        if (options.MinDate != null)
        {
            attrList.Add("data-min-date", options.MinDate?.ToString("O"));
        }

        if (options.MaxDate != null)
        {
            attrList.Add("data-max-date", options.MaxDate?.ToString("O"));
        }

        if (options.MaxSpan != null)
        {
            attrList.Add("data-max-span", JsonSerializer.Serialize(options.MaxSpan));
        }

        if (options.ShowDropdowns == false)
        {
            attrList.Add("data-show-dropdowns", options.ShowDropdowns.ToString()!.ToLowerInvariant());
        }

        if (options.MinYear != null)
        {
            attrList.Add("data-min-year", options.MinYear);
        }

        if (options.MaxYear != null)
        {
            attrList.Add("data-max-year", options.MaxYear);
        }

        switch (options.WeekNumbers)
        {
            case AbpDatePickerWeekNumbers.Normal:
                attrList.Add("data-show-week-numbers", "true");
                break;
            case AbpDatePickerWeekNumbers.Iso:
                attrList.Add("data-show-i-s-o-week-numbers", "true");
                break;
        }

        if (options.TimePicker != null)
        {
            attrList.Add("data-time-picker", options.TimePicker.ToString()!.ToLowerInvariant());
        }

        if (options.TimePickerIncrement != null)
        {
            attrList.Add("data-time-picker-increment", options.TimePickerIncrement);
        }

        if (options.TimePicker24Hour != null)
        {
            attrList.Add("data-time-picker24-hour", options.TimePicker24Hour.ToString()!.ToLowerInvariant());
        }

        if (options.TimePickerSeconds != null)
        {
            attrList.Add("data-time-picker-seconds", options.TimePickerSeconds.ToString()!.ToLowerInvariant());
        }

        if (options.Opens != AbpDatePickerOpens.Center)
        {
            attrList.Add("data-opens", options.Opens.ToString().ToLowerInvariant());
        }

        if (options.Drops != AbpDatePickerDrops.Down)
        {
            attrList.Add("data-drops", options.Drops.ToString().ToLowerInvariant());
        }

        if (!options.ButtonClasses.IsNullOrEmpty())
        {
            attrList.Add("data-button-classes", options.ButtonClasses);
        }

        if (!options.ApplyButtonClasses.IsNullOrEmpty())
        {
            attrList.Add("data-apply-button-classes", options.ApplyButtonClasses);
        }

        if (!options.ClearButtonClasses.IsNullOrEmpty())
        {
            attrList.Add("data-clear-button-classes", options.ClearButtonClasses);
        }

        if (!options.TodayButtonClasses.IsNullOrEmpty())
        {
            attrList.Add("data-today-button-classes", options.TodayButtonClasses);
        }

        if (options.AutoApply != null)
        {
            attrList.Add("data-auto-apply", options.AutoApply.ToString()!.ToLowerInvariant());
        }

        if (options.LinkedCalendars != null)
        {
            attrList.Add("data-linked-calendars", options.LinkedCalendars.ToString()!.ToLowerInvariant());
        }

        if (options.AutoUpdateInput != null)
        {
            attrList.Add("data-auto-update-input", options.AutoUpdateInput.ToString()!.ToLowerInvariant());
        }

        if (!options.ParentEl.IsNullOrEmpty())
        {
            attrList.Add("data-parent-el", options.ParentEl);
        }

#pragma warning disable CS0618 // Type or member is obsolete
        if (!options.DateFormat.IsNullOrEmpty())
        {
            attrList.Add("data-date-format", options.DateFormat);
        }
#pragma warning restore CS0618 // Type or member is obsolete

        if (!options.VisibleDateFormat.IsNullOrEmpty())
        {
            attrList.Add("data-visible-date-format", options.VisibleDateFormat);
        }
        
        if(!options.InputDateFormat.IsNullOrEmpty())
        {
            attrList.Add("data-input-date-format", options.InputDateFormat);
        }

        if(options.Ranges != null && options.Ranges.Any())
        {
            var ranges = options.Ranges.ToDictionary(r => r.Label, r => r.Dates);

            attrList.Add("data-ranges", JsonSerializer.Serialize(ranges));
        }

        if(options.AlwaysShowCalendars != null)
        {
            attrList.Add("data-always-show-calendars", options.AlwaysShowCalendars.ToString()!.ToLowerInvariant());
        }

        if(options.ShowCustomRangeLabel == false)
        {
            attrList.Add("data-show-custom-range-label", options.ShowCustomRangeLabel.ToString()!.ToLowerInvariant());
        }

        if(options.Options != null)
        {
            attrList.Add("data-options", JsonSerializer.Serialize(options.Options));
        }

        if (options.IsUtc != null)
        {
            attrList.Add("data-is-utc", options.IsUtc.ToString()!.ToLowerInvariant());
        }

        if (options.IsIso != null)
        {
            attrList.Add("data-is-iso", options.IsIso.ToString()!.ToLowerInvariant());
        }

        if (!options.PickerId.IsNullOrWhiteSpace())
        {
            attrList.Add("id", options.PickerId);
        }

        if(!options.SingleOpenAndClearButton)
        {
            attrList.Add("data-single-open-and-clear-button", options.SingleOpenAndClearButton.ToString().ToLowerInvariant());
        }

        return attrList;
    }

    protected TagHelperAttributeList GetBaseTagAttributes(TagHelperContext context, TagHelperOutput output, IAbpDatePickerOptions options)
    {
        var groupPrefix = "group-";

        var tagHelperAttributes = output.Attributes.Where(a => !a.Name.StartsWith(groupPrefix)).ToList();

        var attrList = new TagHelperAttributeList();

        foreach (var tagHelperAttribute in tagHelperAttributes)
        {
            attrList.Add(tagHelperAttribute);
        }

        if (attrList.ContainsName("type"))
        {
            attrList.Remove(attrList.First(a => a.Name == "type"));
        }

        if (attrList.ContainsName("name"))
        {
            attrList.Remove(attrList.First(a => a.Name == "name"));
        }

        if (attrList.ContainsName("id"))
        {
            attrList.Remove(attrList.First(a => a.Name == "id"));
        }

        if (attrList.ContainsName("value"))
        {
            attrList.Remove(attrList.First(a => a.Name == "value"));
        }

        foreach (var attr in ConvertDatePickerOptionsToAttributeList(options))
        {
            attrList.Add(attr);
        }

        AddBaseTagAttributes(attrList);

        return attrList;
    }

    protected virtual bool IsOutputHidden(TagHelperOutput inputTag)
    {
        return inputTag.Attributes.Any(a =>
            a.Name.ToLowerInvariant() == "type" && a.Value?.ToString()?.ToLowerInvariant() == "hidden");
    }

    protected virtual string GetInfoAsHtml(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag)
    {
        if (IsOutputHidden(inputTag))
        {
            return string.Empty;
        }

        string text;
        ModelExplorer? modelExplorer = null;

        if (!string.IsNullOrEmpty(TagHelper.InfoText))
        {
            text = TagHelper.InfoText;
        }
        else
        {
            var infoAttribute = GetAttributeAndModelExpression<InputInfoText>(out var modelExpression);
            if (infoAttribute != null)
            {
                modelExplorer = modelExpression!.ModelExplorer;
                text = infoAttribute.Text;
            }
            else
            {
                return string.Empty;
            }
        }

        var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");
        modelExplorer ??= GetModelExpression()!.ModelExplorer;
        var localizedText = TagHelperLocalizer.GetLocalizedText(text, modelExplorer);

        var div = new TagBuilder("div");
        div.Attributes.Add("id", idAttr?.Value + "InfoText");
        div.AddCssClass("form-text");
        div.InnerHtml.Append(localizedText);

        inputTag.Attributes.Add("aria-describedby", idAttr?.Value + "InfoText");

        return div.ToHtmlString();
    }

    protected virtual async Task<string> GetLabelAsHtmlAsync(TagHelperContext context, TagHelperOutput output,
        TagHelperOutput inputTag)
    {
        if (IsOutputHidden(inputTag) || TagHelper.SuppressLabel)
        {
            return string.Empty;
        }
        if (string.IsNullOrEmpty(TagHelper.Label))
        {
            return await GetLabelAsHtmlUsingTagHelperAsync(context, output) + GetRequiredSymbol(context, output);
        }

        var label = new TagBuilder("label");
        label.Attributes.Add("for", GetIdAttributeValue(inputTag));
        label.InnerHtml.AppendHtml(Encoder.Encode(TagHelper.Label));

        label.AddCssClass("form-label");

        if (!TagHelper.LabelTooltip.IsNullOrEmpty())
        {
            label.Attributes.Add("data-bs-toggle", "tooltip");
            label.Attributes.Add("data-bs-placement", TagHelper.LabelTooltipPlacement);
            if (TagHelper.LabelTooltipHtml)
            {
                label.Attributes.Add("data-bs-html", "true");
            }

            label.Attributes.Add("title", TagHelper.LabelTooltip);
            var iconClass = TagHelper.LabelTooltipIcon;
            if (iconClass.StartsWith("bi-"))
            {
                iconClass = "bi " + iconClass;
            }
            else if (iconClass.StartsWith("fa-"))
            {
                iconClass = "fa " + iconClass;
            }
            label.InnerHtml.AppendHtml($" <i class=\"{iconClass}\"></i>");
        }

        return label.ToHtmlString();
    }

    protected virtual string GetIdAttributeValue(TagHelperOutput inputTag)
    {
        var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");

        return idAttr != null ? idAttr.Value.ToString()! : string.Empty;
    }

    protected virtual string GetRequiredSymbol(TagHelperContext context, TagHelperOutput output)
    {
        if (!TagHelper.DisplayRequiredSymbol)
        {
            return "";
        }

        var isHaveRequiredAttribute = context.AllAttributes.Any(a => a.Name == "required");

        return GetAttribute<RequiredAttribute>() != null || isHaveRequiredAttribute ? "<span> * </span>" : "";
    }

    protected abstract ModelExpression? GetModelExpression();

    protected virtual async Task<string> GetLabelAsHtmlUsingTagHelperAsync(TagHelperContext context,
        TagHelperOutput output)
    {
        var modelExpression = GetModelExpression();
        if (modelExpression == null)
        {
            return string.Empty;
        }
        var labelTagHelper = new LabelTagHelper(Generator) {
            ViewContext = TagHelper.ViewContext,
            For = modelExpression
        };

        var attributeList = new TagHelperAttributeList();

        attributeList.AddClass("form-label");

        if (!TagHelper.LabelTooltip.IsNullOrEmpty())
        {
            attributeList.Add("data-bs-toggle", "tooltip");
            attributeList.Add("data-bs-placement", TagHelper.LabelTooltipPlacement);
            if (TagHelper.LabelTooltipHtml)
            {
                attributeList.Add("data-bs-html", "true");
            }

            attributeList.Add("title", TagHelper.LabelTooltip);
        }

        var innerOutput =
            await labelTagHelper.ProcessAndGetOutputAsync(attributeList, context, "label", TagMode.StartTagAndEndTag);
        if (!TagHelper.LabelTooltip.IsNullOrEmpty())
        {
            var iconClass = TagHelper.LabelTooltipIcon;
            if (iconClass.StartsWith("bi-"))
            {
                iconClass = "bi " + iconClass;
            }
            else if (iconClass.StartsWith("fa-"))
            {
                iconClass = "fa " + iconClass;
            }
            innerOutput.Content.AppendHtml($" <i class=\"{iconClass}\"></i>");
        }

        return innerOutput.Render(Encoder);
    }

    protected virtual async Task<string> ProcessButtonAndGetContentAsync(TagHelperContext context,
        TagHelperOutput output, string icon, string type, bool visible = true)
    {
        var abpButtonTagHelper = ServiceProvider.GetRequiredService<AbpButtonTagHelper>();
        var attributes =
            new TagHelperAttributeList { new("type", "button"), new("tabindex", "-1"), new("data-type", type) };
        abpButtonTagHelper.ButtonType = AbpButtonType.Outline_Secondary;
        abpButtonTagHelper.Icon = icon;

        abpButtonTagHelper.Disabled = TagHelper.IsDisabled || GetAttribute<DisabledInput>() != null;

        if (!visible)
        {
            attributes.AddClass("d-none");
        }

        return await abpButtonTagHelper.RenderAsync(attributes, context, Encoder, "button", TagMode.StartTagAndEndTag);
    }

    protected virtual void AddInfoTextId(TagHelperOutput inputTagHelperOutput)
    {
        if (GetAttribute<InputInfoText>() == null)
        {
            return;
        }

        var idAttr = inputTagHelperOutput.Attributes.FirstOrDefault(a => a.Name == "id");

        if (idAttr == null)
        {
            return;
        }

        inputTagHelperOutput.Attributes.Add("aria-describedby", GetInfoText());
    }

    public virtual string GetInfoText()
    {
        var infoAttribute = GetAttributeAndModelExpression<InputInfoText>(out var modelExpression);

        if (infoAttribute != null)
        {
            return TagHelperLocalizer.GetLocalizedText(infoAttribute.Text, modelExpression!.ModelExplorer);
        }

        return string.Empty;
    }

    protected virtual void AddPlaceholderAttribute(TagHelperOutput inputTagHelperOutput)
    {
        if (inputTagHelperOutput.Attributes.ContainsName("placeholder"))
        {
            return;
        }

        var attribute = GetAttributeAndModelExpression<Placeholder>(out var modelExpression);

        if (attribute != null)
        {
            var placeholderLocalized =
                TagHelperLocalizer.GetLocalizedText(attribute.Value, modelExpression!.ModelExplorer);

            inputTagHelperOutput.Attributes.Add("placeholder", placeholderLocalized);
        }
    }

    protected virtual void AddFormControls(TagHelperContext context, TagHelperOutput output,
        TagHelperOutput inputTagHelperOutput)
    {
        inputTagHelperOutput.Attributes.AddClass("form-control");
        var size = GetSize(context, output);
        if (!size.IsNullOrEmpty())
        {
            inputTagHelperOutput.Attributes.AddClass(size);
        }
    }

    protected virtual void AddAutoFocusAttribute(TagHelperOutput inputTagHelperOutput)
    {
        if (TagHelper.AutoFocus && !inputTagHelperOutput.Attributes.ContainsName("data-auto-focus"))
        {
            inputTagHelperOutput.Attributes.Add("data-auto-focus", "true");
        }
    }

    protected virtual void AddDataPickerAttribute(TagHelperOutput inputTagHelperOutput)
    {
        inputTagHelperOutput.Attributes.Add("data-datepicker", "true");
    }

    protected virtual void AddDisabledAttribute(TagHelperOutput inputTagHelperOutput)
    {
        if (inputTagHelperOutput.Attributes.ContainsName("disabled") == false &&
            (TagHelper.IsDisabled || GetAttribute<DisabledInput>() != null))
        {
            inputTagHelperOutput.Attributes.Add("disabled", "");
        }
    }


    protected virtual string GetSize(TagHelperContext context, TagHelperOutput output)
    {
        // TODO: Test this method
        var attribute = GetAttribute<FormControlSize>();

        if (attribute != null)
        {
            TagHelper.Size = attribute.Size;
        }

        return TagHelper.Size switch {
            AbpFormControlSize.Small => "form-control-sm",
            AbpFormControlSize.Medium => "form-control-md",
            AbpFormControlSize.Large => "form-control-lg",
            _ => ""
        };
    }

    protected virtual Task<string> GetValidationAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
    {
        var @for = GetModelExpression();
        if (@for == null)
        {
            return Task.FromResult(string.Empty);
        }
        
        return GetValidationAsHtmlByInputAsync(context, output, @for);
    }

    protected virtual async Task<string> GetValidationAsHtmlByInputAsync(TagHelperContext context,
        TagHelperOutput output,
        [NotNull]ModelExpression @for)
    {
        var validationMessageTagHelper =
            new ValidationMessageTagHelper(Generator) { For = @for, ViewContext = TagHelper.ViewContext };

        var attributeList = new TagHelperAttributeList { { "class", "text-danger" } };
        
        if(!output.Attributes.TryGetAttribute("name", out var nameAttribute) || nameAttribute == null || nameAttribute.Value == null)
        {
            if (nameAttribute != null)
            {
                output.Attributes.Remove(nameAttribute);
            }
            nameAttribute = new TagHelperAttribute("name", "date_" + Guid.NewGuid().ToString("N"));
            output.Attributes.Add(nameAttribute);
        }
        
        attributeList.Add("data-valmsg-for", nameAttribute.Value);

        return await validationMessageTagHelper.RenderAsync(attributeList, context, Encoder, "span",
            TagMode.StartTagAndEndTag);
    }
}
