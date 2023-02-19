using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.DatePicker;

public abstract class
    AbpDatePickerBaseTagHelper<TTagHelper> : AbpTagHelper<TTagHelper, AbpDatePickerBaseTagHelperService<TTagHelper>>
    where TTagHelper : AbpDatePickerBaseTagHelper<TTagHelper>

{
    public string Label { get; set; }

    public string LabelTooltip { get; set; }

    public string LabelTooltipIcon { get; set; } = "bi-info-circle";

    public string LabelTooltipPlacement { get; set; } = "right";

    public bool LabelTooltipHtml { get; set; } = false;

    [HtmlAttributeName("info")] 
    public string InfoText { get; set; }

    [HtmlAttributeName("disabled")] 
    public bool IsDisabled { get; set; } = false;

    [HtmlAttributeName("readonly")] 
    public bool? IsReadonly { get; set; } = false;

    public bool AutoFocus { get; set; }

    [HtmlAttributeName("type")] 
    public string InputTypeName { get; set; } = "text";

    public AbpFormControlSize Size { get; set; } = AbpFormControlSize.Default;

    [HtmlAttributeName("required-symbol")] 
    public bool DisplayRequiredSymbol { get; set; } = true;

    [HtmlAttributeName("asp-format")] 
    public string Format { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    public bool SuppressLabel { get; set; }


    // Min and Max date
    public DateTime? MinDate { get; set; }

    public DateTime? MaxDate { get; set; }

    // Max span between start and end date
    public object? MaxSpan { get; set; }

    // Show dropdowns
    public bool ShowDropdowns { get; set; } = true;

    // Min and Max year
    public int? MinYear { get; set; }

    public int? MaxYear { get; set; }

    // Show week numbers
    public AbpDatePickerWeekNumbers WeekNumbers { get; set; } = AbpDatePickerWeekNumbers.None;

    // Time picker
    public bool? TimePicker { get; set; }

    // Time picker increment
    public int? TimePickerIncrement { get; set; }

    // Time picker 24 hour
    public bool? TimePicker24Hour { get; set; }

    // Time picker seconds
    public bool? TimePickerSeconds { get; set; }

    // Ranges object
    public List<AbpDatePickerRange> Ranges { get; set; }
    
    // Show custom range label
    public bool ShowCustomRangeLabel { get; set; } = true;
    
    // Always show calendar
    public bool AlwaysShowCalendars { get; set; } = false;

    // Opens date picker on left or right or center of the input
    public AbpDatePickerOpens Opens { get; set; } = AbpDatePickerOpens.Center;

    // Drops down or up or auto
    public AbpDatePickerDrops Drops { get; set; } = AbpDatePickerDrops.Down;

    // Button classes
    [CanBeNull] 
    public string ButtonClasses { get; set; }

    // Apply class to all buttons
    [CanBeNull] 
    public string ApplyButtonClasses { get; set; }

    // Cancel class to all buttons
    [CanBeNull] 
    public string CancelButtonClasses { get; set; }

    // Locale
    [CanBeNull] 
    public object Locale { get; set; }

    // Auto apply
    public bool AutoApply { get; set; } = true;

    // Linked calendars
    public bool? LinkedCalendars { get; set; }

    // Auto update input
    public bool AutoUpdateInput { get; set; } = false;

    // Parent element
    [CanBeNull] 
    public string ParentEl { get; set; }

    // public DatePickerType Type { get; set; } = DatePickerType.Date;

    [CanBeNull] 
    public string DateFormat { get; set; }

    public bool OpenButton { get; set; } = true;

    public bool ClearButton { get; set; } = true;

    public bool IsUtc { get; set; } = false;
    
    public bool IsIso { get; set; } = false;
    
    [CanBeNull] 
    public object Options { get; set; }

    protected AbpDatePickerBaseTagHelper(AbpDatePickerBaseTagHelperService<TTagHelper> service) : base(service)
    {
    }
}