using Blazorise;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Volo.Abp.BlazoriseUI;

public static class BlazoriseFluentSizingParse
{
    private static readonly Regex SizingPattern = new Regex(
        @"^(\d+(?:\.\d+)?)(px|rem|em|ch|vw|vh|%)$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    /// <summary>
    /// Parses a CSS size string into an IFluentSizingStyle.
    /// Supported formats (based on Blazorise FluentSizing source):
    ///   Fixed units    : 10px, 10rem, 10em, 10ch
    ///   Viewport units : 10vw, 10vh
    ///   Percentage     : 25%, 33%, 50%, 66%, 75%, 100% -> maps to SizingSize enum (CSS class)
    ///                    other % values -> inline style
    ///   Keyword        : auto -> SizingSize.Auto
    ///   CSS variable   : var(--my-var), --my-var, or my-var (all handled by WithVariable)
    /// </summary>
    public static IFluentSizingStyle Parse(string value, SizingType sizingType = SizingType.None)
    {
        var fluentSizing = new FluentSizing(sizingType);

        if (string.IsNullOrWhiteSpace(value))
        {
            return fluentSizing;
        }

        value = value.Trim();

        // "auto" -> SizingSize.Auto
        if (value.Equals("auto", StringComparison.OrdinalIgnoreCase))
        {
            return (IFluentSizingStyle)fluentSizing.WithSize(SizingSize.Auto);
        }

        // CSS variable:
        //   "var(--my-var)" -> used as-is
        //   "--my-var"      -> wrapped as var(--my-var)
        //   "my-var"        -> prepended "--" and wrapped as var(--my-var)
        // All three cases are handled correctly by Blazorise's GetCssVariableValue.
        if (value.StartsWith("var(", StringComparison.Ordinal) || value.StartsWith("--", StringComparison.Ordinal))
        {
            return fluentSizing.WithVariable(value);
        }

        var match = SizingPattern.Match(value);

        if (!match.Success)
        {
            return fluentSizing;
        }

        var number = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        var unit = match.Groups[2].Value.ToLowerInvariant();

        if (unit == "%")
        {
            // Standard percentages map to SizingSize enum (generates CSS class via class provider)
            var sizingSize = number switch
            {
                25 => SizingSize.Is25,
                33 => SizingSize.Is33,
                50 => SizingSize.Is50,
                66 => SizingSize.Is66,
                75 => SizingSize.Is75,
                100 => SizingSize.Is100,
                _ => SizingSize.Default
            };

            if (sizingSize != SizingSize.Default)
            {
                return (IFluentSizingStyle)fluentSizing.WithSize(sizingSize);
            }

            // Non-standard percentage falls back to inline style
            return fluentSizing.WithSize("%", number);
        }

        // px, rem, em, ch, vw, vh -> inline style via WithSize(unit, size)
        return fluentSizing.WithSize(unit, number);
    }
}
