using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form.DatePicker;

public interface IAbpDatePickerOptions
{
    public string PickerId { get; set; }

    /// <summary>
    /// Min date allowed
    /// </summary>
    DateTime? MinDate { get; set; }

    /// <summary>
    /// Max date allowed
    /// </summary>
    DateTime? MaxDate { get; set; }

    /// <summary>
    /// Max span between start and end date
    /// </summary>
    object MaxSpan { get; set; }

    /// <summary>
    /// Show dropdowns
    /// </summary>
    bool? ShowDropdowns { get; set; }

    /// <summary>
    /// Min year allowed
    /// </summary>
    int? MinYear { get; set; }

    /// <summary>
    /// Max year allowed
    /// </summary>
    int? MaxYear { get; set; }

    /// <summary>
    /// Show week numbers
    /// </summary>
    AbpDatePickerWeekNumbers WeekNumbers { get; set; }

    /// <summary>
    /// Allows users to pick a time
    /// </summary>
    bool? TimePicker { get; set; }

    /// <summary>
    /// The time interval allowed
    /// </summary>
    int? TimePickerIncrement { get; set; }

    /// <summary>
    /// Sets the time picker hour format as 12 or 24.
    /// </summary>
    bool? TimePicker24Hour { get; set; }

    /// <summary>
    /// Allows seconds selection
    /// </summary>
    bool? TimePickerSeconds { get; set; }

    /// <summary>
    /// Predefined date range list
    /// </summary>
    List<AbpDatePickerRange> Ranges { get; set; }

    /// <summary>
    /// Show custom range label
    /// </summary>
    bool? ShowCustomRangeLabel { get; set; }

    /// <summary>
    /// Always show calendars
    /// </summary>
    bool? AlwaysShowCalendars { get; set; }

    /// <summary>
    /// The horizontal location of the datepicker
    /// </summary>
    AbpDatePickerOpens Opens { get; set; }

    /// <summary>
    /// The vertical location of the datepicker
    /// </summary>
    AbpDatePickerDrops Drops { get; set; }

    /// <summary>
    /// Button CSS classes
    /// </summary>
    [CanBeNull]
    string ButtonClasses { get; set; }

    /// <summary>
    /// Apply class to all buttons
    /// </summary>
    [CanBeNull]
    string ApplyButtonClasses { get; set; }

    /// <summary>
    /// Cancel CSS classes to all buttons
    /// </summary>
    [CanBeNull]
    string CancelButtonClasses { get; set; }

    /// <summary>
    /// Sets a custom locale
    /// </summary>
    [CanBeNull]
    object Locale { get; set; }

    /// <summary>
    /// Applies the value when it a date is selected
    /// </summary>
    bool? AutoApply { get; set; }

    /// <summary>
    /// When enabled, the two calendars displayed will always be for two sequential months (i.e. January and February), and both will be advanced when clicking the left or right arrows above the calendars. When disabled, the two calendars can be individually advanced and display any month/year. 
    /// </summary>
    bool? LinkedCalendars { get; set; }

    /// <summary>
    /// Updates the input automatically
    /// </summary>
    bool? AutoUpdateInput { get; set; }

    // Parent element
    [CanBeNull]
    string ParentEl { get; set; }

    [CanBeNull]
    string DateFormat { get; set; }

    bool OpenButton { get; set; }

    bool ClearButton { get; set; }

    bool SingleOpenAndClearButton { get; set; }

    bool? IsUtc { get; set; }

    bool? IsIso { get; set; }

    [CanBeNull]
    object Options { get; set; }
}