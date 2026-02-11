using System;
using System.Globalization;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

[Serializable]
public class CurrentCultureDto
{
    public string DisplayName { get; set; } = default!;

    public string EnglishName { get; set; } = default!;

    public string ThreeLetterIsoLanguageName { get; set; } = default!;

    public string TwoLetterIsoLanguageName { get; set; } = default!;

    public bool IsRightToLeft { get; set; }

    public string CultureName { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string NativeName { get; set; } = default!;

    public DateTimeFormatDto DateTimeFormat { get; set; } = default!;

    /// <summary>
    /// Creates a new <see cref="CurrentCultureDto"/> object based on the current CultureInfo.
    /// </summary>
    public static CurrentCultureDto Create()
    {
        return new CurrentCultureDto
        {
            Name = CultureInfo.CurrentUICulture.Name,
            DisplayName = CultureInfo.CurrentUICulture.DisplayName,
            EnglishName = CultureInfo.CurrentUICulture.EnglishName,
            NativeName = CultureInfo.CurrentUICulture.NativeName,
            IsRightToLeft = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft,
            CultureName = CultureInfo.CurrentUICulture.TextInfo.CultureName,
            TwoLetterIsoLanguageName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
            ThreeLetterIsoLanguageName = CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName,
            DateTimeFormat = new DateTimeFormatDto
            {
                CalendarAlgorithmType = CultureInfo.CurrentUICulture.DateTimeFormat.Calendar.AlgorithmType.ToString(),
                DateTimeFormatLong = CultureInfo.CurrentUICulture.DateTimeFormat.LongDatePattern,
                ShortDatePattern = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern,
                FullDateTimePattern = CultureInfo.CurrentUICulture.DateTimeFormat.FullDateTimePattern,
                DateSeparator = CultureInfo.CurrentUICulture.DateTimeFormat.DateSeparator,
                ShortTimePattern = CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern,
                LongTimePattern = CultureInfo.CurrentUICulture.DateTimeFormat.LongTimePattern,
            }
        };
    }
}
