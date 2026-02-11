using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form.DatePicker;

public interface IAbpDatePickerOptions
{
    public string? PickerId { get; set; }

    /// <summary>
    /// Min date allowed
    /// </summary>
    DateTime? MinDate { get; set; }

    /// <summary>
    /// Max date allowed
    /// </summary>
    DateTime? MaxDate { get; set; }

    /// <summary>
    /// The maximum span between the selected start and end dates. Can have any property you can add to a moment object (i.e. days, months)
    /// </summary>
    object? MaxSpan { get; set; }

    /// <summary>
    /// Show year and month select boxes above calendars to jump to a specific month and year.
    /// </summary>
    bool? ShowDropdowns { get; set; }

    /// <summary>
    /// The minimum year shown in the dropdowns when showDropdowns is set to true.
    /// </summary>
    int? MinYear { get; set; }

    /// <summary>
    /// The maximum year shown in the dropdowns when showDropdowns is set to true.
    /// </summary>
    int? MaxYear { get; set; }

    /// <summary>
    /// Show week numbers at the start of each week on the calendars.
    /// </summary>
    AbpDatePickerWeekNumbers WeekNumbers { get; set; }

    /// <summary>
    /// Adds select boxes to choose times in addition to dates.
    /// </summary>
    bool? TimePicker { get; set; }

    /// <summary>
    /// Increment of the minutes selection list for times (i.e. 30 to allow only selection of times ending in 0 or 30).
    /// </summary>
    int? TimePickerIncrement { get; set; }

    /// <summary>
    /// Use 24-hour instead of 12-hour times, removing the AM/PM selection.
    /// </summary>
    bool? TimePicker24Hour { get; set; }

    /// <summary>
    /// Show seconds in the timePicker.
    /// </summary>
    bool? TimePickerSeconds { get; set; }

    /// <summary>
    /// Set predefined date ranges the user can select from. Each key is the label for the range, and its value an array with two dates representing the bounds of the range.
    /// </summary>
    List<AbpDatePickerRange>? Ranges { get; set; }

    /// <summary>
    /// Displays "Custom Range" at the end of the list of predefined ranges, when the ranges option is used. This option will be highlighted whenever the current date range selection does not match one of the predefined ranges. Clicking it will display the calendars to select a new range.
    /// </summary>
    bool? ShowCustomRangeLabel { get; set; }

    /// <summary>
    /// Normally, if you use the ranges option to specify pre-defined date ranges, calendars for choosing a custom date range are not shown until the user clicks "Custom Range". When this option is set to true, the calendars for choosing a custom date range are always shown instead.
    /// </summary>
    bool? AlwaysShowCalendars { get; set; }

    /// <summary>
    /// Whether the picker appears aligned to the left, to the right, or centered under the HTML element it's attached to.
    /// </summary>
    AbpDatePickerOpens Opens { get; set; }

    /// <summary>
    /// Whether the picker appears below (default) or above the HTML element it's attached to.
    /// </summary>
    AbpDatePickerDrops Drops { get; set; }

    /// <summary>
    /// CSS class names that will be added to both the today, clear, and apply buttons.
    /// </summary>
    string? ButtonClasses { get; set; }

    /// <summary>
    /// CSS class names that will be added only to the today button.
    /// </summary>
    string? TodayButtonClasses { get; set; }
    
    /// <summary>
    /// CSS class names that will be added only to the apply button.
    /// </summary>
    string? ApplyButtonClasses { get; set; }

    /// <summary>
    /// CSS class names that will be added only to the clear button.
    /// </summary>
    string? ClearButtonClasses { get; set; }

    /// <summary>
    /// Allows you to provide localized strings for buttons and labels, customize the date format, and change the first day of week for the calendars.
    /// </summary>
    [CanBeNull]
    object? Locale { get; set; }

    /// <summary>
    /// Hide the apply button, and automatically apply a new date range as soon as two dates are clicked.
    /// </summary>
    bool? AutoApply { get; set; }

    /// <summary>
    /// When enabled, the two calendars displayed will always be for two sequential months (i.e. January and February), and both will be advanced when clicking the left or right arrows above the calendars. When disabled, the two calendars can be individually advanced and display any month/year.
    /// </summary>
    bool? LinkedCalendars { get; set; }

    /// <summary>
    /// Indicates whether the date range picker should automatically update the value of the input element it's attached to at initialization and when the selected dates change.
    /// </summary>
    bool? AutoUpdateInput { get; set; }

    /// <summary>
    /// jQuery selector of the parent element that the date range picker will be added to, if not provided this will be 'body'
    /// </summary>
    string? ParentEl { get; set; }
    
    [Obsolete("Use VisibleDateFormat instead.")]
    string? DateFormat { get; set; }
    
    /// <summary>
    /// The date format string that will appear in the input element. For user input.
    /// </summary>
    string? VisibleDateFormat { get; set; }
    
    /// <summary>
    /// The date format string that will appear in the hidden input element. For backend compatibility.
    /// </summary>
    string? InputDateFormat { get; set; }

    bool OpenButton { get; set; }

    bool? ClearButton { get; set; }

    bool SingleOpenAndClearButton { get; set; }

    bool? IsUtc { get; set; }

    bool? IsIso { get; set; }

    /// <summary>
    /// Other non-mapped attributes will be automatically added to the input element as is.
    /// </summary>
    object? Options { get; set; }
}