using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form.DatePicker;

public abstract class
    AbpDatePickerBaseTagHelper<TTagHelper> : AbpTagHelper<TTagHelper, AbpDatePickerBaseTagHelperService<TTagHelper>>, IAbpDatePickerOptions
    where TTagHelper : AbpDatePickerBaseTagHelper<TTagHelper>

{
    private IAbpDatePickerOptions _abpDatePickerOptionsImplementation;

    public string? Label { get; set; }

    public string? LabelTooltip { get; set; }

    public string LabelTooltipIcon { get; set; } = "bi-info-circle";

    public string LabelTooltipPlacement { get; set; } = "right";

    public bool LabelTooltipHtml { get; set; } = false;

    [HtmlAttributeName("info")]
    public string? InfoText { get; set; }

    [HtmlAttributeName("disabled")]
    public bool IsDisabled { get; set; } = false;

    [HtmlAttributeName("readonly")]
    public bool? IsReadonly { get; set; } = false;

    public bool AutoFocus { get; set; }

    public AbpFormControlSize Size { get; set; } = AbpFormControlSize.Default;

    [HtmlAttributeName("required-symbol")]
    public bool DisplayRequiredSymbol { get; set; } = true;

    public string? Name { get; set; }

    public string? Value { get; set; }

    public bool SuppressLabel { get; set; }

    public bool AddMarginBottomClass  { get; set; } = true;

    protected AbpDatePickerBaseTagHelper(AbpDatePickerBaseTagHelperService<TTagHelper> service) : base(service)
    {
        _abpDatePickerOptionsImplementation = new AbpDatePickerOptions();
    }

    public void SetDatePickerOptions(IAbpDatePickerOptions options)
    {
        _abpDatePickerOptionsImplementation = options;
    }

    public string? PickerId {
        get => _abpDatePickerOptionsImplementation.PickerId;
        set => _abpDatePickerOptionsImplementation.PickerId = value;
    }

    public DateTime? MinDate {
        get => _abpDatePickerOptionsImplementation.MinDate;
        set => _abpDatePickerOptionsImplementation.MinDate = value;
    }

    public DateTime? MaxDate {
        get => _abpDatePickerOptionsImplementation.MaxDate;
        set => _abpDatePickerOptionsImplementation.MaxDate = value;
    }

    public object? MaxSpan {
        get => _abpDatePickerOptionsImplementation.MaxSpan;
        set => _abpDatePickerOptionsImplementation.MaxSpan = value;
    }

    public bool? ShowDropdowns {
        get => _abpDatePickerOptionsImplementation.ShowDropdowns;
        set => _abpDatePickerOptionsImplementation.ShowDropdowns = value;
    }

    public int? MinYear {
        get => _abpDatePickerOptionsImplementation.MinYear;
        set => _abpDatePickerOptionsImplementation.MinYear = value;
    }

    public int? MaxYear {
        get => _abpDatePickerOptionsImplementation.MaxYear;
        set => _abpDatePickerOptionsImplementation.MaxYear = value;
    }

    public AbpDatePickerWeekNumbers WeekNumbers {
        get => _abpDatePickerOptionsImplementation.WeekNumbers;
        set => _abpDatePickerOptionsImplementation.WeekNumbers = value;
    }

    public bool? TimePicker {
        get => _abpDatePickerOptionsImplementation.TimePicker;
        set => _abpDatePickerOptionsImplementation.TimePicker = value;
    }

    public int? TimePickerIncrement {
        get => _abpDatePickerOptionsImplementation.TimePickerIncrement;
        set => _abpDatePickerOptionsImplementation.TimePickerIncrement = value;
    }

    public bool? TimePicker24Hour {
        get => _abpDatePickerOptionsImplementation.TimePicker24Hour;
        set => _abpDatePickerOptionsImplementation.TimePicker24Hour = value;
    }

    public bool? TimePickerSeconds {
        get => _abpDatePickerOptionsImplementation.TimePickerSeconds;
        set => _abpDatePickerOptionsImplementation.TimePickerSeconds = value;
    }

    public List<AbpDatePickerRange>? Ranges {
        get => _abpDatePickerOptionsImplementation.Ranges;
        set => _abpDatePickerOptionsImplementation.Ranges = value;
    }

    public bool? ShowCustomRangeLabel {
        get => _abpDatePickerOptionsImplementation.ShowCustomRangeLabel;
        set => _abpDatePickerOptionsImplementation.ShowCustomRangeLabel = value;
    }

    public bool? AlwaysShowCalendars {
        get => _abpDatePickerOptionsImplementation.AlwaysShowCalendars;
        set => _abpDatePickerOptionsImplementation.AlwaysShowCalendars = value;
    }

    public AbpDatePickerOpens Opens {
        get => _abpDatePickerOptionsImplementation.Opens;
        set => _abpDatePickerOptionsImplementation.Opens = value;
    }

    public AbpDatePickerDrops Drops {
        get => _abpDatePickerOptionsImplementation.Drops;
        set => _abpDatePickerOptionsImplementation.Drops = value;
    }

    public string? ButtonClasses {
        get => _abpDatePickerOptionsImplementation.ButtonClasses;
        set => _abpDatePickerOptionsImplementation.ButtonClasses = value;
    }

    public string? TodayButtonClasses {
        get => _abpDatePickerOptionsImplementation.TodayButtonClasses;
        set => _abpDatePickerOptionsImplementation.TodayButtonClasses = value;
    }

    public string? ApplyButtonClasses {
        get => _abpDatePickerOptionsImplementation.ApplyButtonClasses;
        set => _abpDatePickerOptionsImplementation.ApplyButtonClasses = value;
    }

    public string? ClearButtonClasses {
        get => _abpDatePickerOptionsImplementation.ClearButtonClasses;
        set => _abpDatePickerOptionsImplementation.ClearButtonClasses = value;
    }

    public object? Locale {
        get => _abpDatePickerOptionsImplementation.Locale;
        set => _abpDatePickerOptionsImplementation.Locale = value;
    }

    public bool? AutoApply {
        get => _abpDatePickerOptionsImplementation.AutoApply;
        set => _abpDatePickerOptionsImplementation.AutoApply = value;
    }

    public bool? LinkedCalendars {
        get => _abpDatePickerOptionsImplementation.LinkedCalendars;
        set => _abpDatePickerOptionsImplementation.LinkedCalendars = value;
    }

    public bool? AutoUpdateInput {
        get => _abpDatePickerOptionsImplementation.AutoUpdateInput;
        set => _abpDatePickerOptionsImplementation.AutoUpdateInput = value;
    }

    public string? ParentEl {
        get => _abpDatePickerOptionsImplementation.ParentEl;
        set => _abpDatePickerOptionsImplementation.ParentEl = value;
    }

    [Obsolete("Use VisibleDateFormat instead.")]
    public string? DateFormat {
        get => _abpDatePickerOptionsImplementation.DateFormat;
        set => _abpDatePickerOptionsImplementation.DateFormat = value;
    }
    
    public string? VisibleDateFormat {
        get => _abpDatePickerOptionsImplementation.VisibleDateFormat;
        set => _abpDatePickerOptionsImplementation.VisibleDateFormat = value;
    }
    
    public string? InputDateFormat {
        get => _abpDatePickerOptionsImplementation.InputDateFormat;
        set => _abpDatePickerOptionsImplementation.InputDateFormat = value;
    }

    public bool OpenButton {
        get => _abpDatePickerOptionsImplementation.OpenButton;
        set => _abpDatePickerOptionsImplementation.OpenButton = value;
    }

    public bool? ClearButton {
        get => _abpDatePickerOptionsImplementation.ClearButton;
        set => _abpDatePickerOptionsImplementation.ClearButton = value;
    }

    public bool SingleOpenAndClearButton {
        get => _abpDatePickerOptionsImplementation.SingleOpenAndClearButton;
        set => _abpDatePickerOptionsImplementation.SingleOpenAndClearButton = value;
    }

    public bool? IsUtc {
        get => _abpDatePickerOptionsImplementation.IsUtc;
        set => _abpDatePickerOptionsImplementation.IsUtc = value;
    }

    public bool? IsIso {
        get => _abpDatePickerOptionsImplementation.IsIso;
        set => _abpDatePickerOptionsImplementation.IsIso = value;
    }

    public object? Options {
        get => _abpDatePickerOptionsImplementation.Options;
        set => _abpDatePickerOptionsImplementation.Options = value;
    }
}
