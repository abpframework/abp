using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form.DatePicker;

public interface IAbpDatePickerOptions
{
    public string PickerId { get; set; }
    // Min and Max date
    DateTime? MinDate { get; set; }

    DateTime? MaxDate { get; set; }

    // Max span between start and end date
    object? MaxSpan { get; set; }

    // Show dropdowns
    bool? ShowDropdowns { get; set; }

    // Min and Max year
    int? MinYear { get; set; }

    int? MaxYear { get; set; }

    // Show week numbers
    AbpDatePickerWeekNumbers WeekNumbers { get; set; }

    // Time picker
    bool? TimePicker { get; set; }

    // Time picker increment
    int? TimePickerIncrement { get; set; }

    // Time picker 24 hour
    bool? TimePicker24Hour { get; set; }

    // Time picker seconds
    bool? TimePickerSeconds { get; set; }

    // Ranges object
    List<AbpDatePickerRange> Ranges { get; set; }
    
    // Show custom range label
    bool? ShowCustomRangeLabel { get; set; }
    
    // Always show calendar
    bool? AlwaysShowCalendars { get; set; }

    // Opens date picker on left or right or center of the input
    AbpDatePickerOpens Opens { get; set; }

    // Drops down or up or auto
    AbpDatePickerDrops Drops { get; set; }

    // Button classes
    [CanBeNull] 
    string ButtonClasses { get; set; }

    // Apply class to all buttons
    [CanBeNull] 
    string ApplyButtonClasses { get; set; }

    // Cancel class to all buttons
    [CanBeNull] 
    string CancelButtonClasses { get; set; }

    // Locale
    [CanBeNull] 
    object Locale { get; set; }

    // Auto apply
    bool? AutoApply { get; set; }

    // Linked calendars
    bool? LinkedCalendars { get; set; }

    // Auto update input
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