namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class DateTimeFormatDto
{
    public string CalendarAlgorithmType { get; set; } = default!;

    public string DateTimeFormatLong { get; set; } = default!;

    public string ShortDatePattern { get; set; } = default!;

    public string FullDateTimePattern { get; set; } = default!;

    public string DateSeparator { get; set; } = default!;

    public string ShortTimePattern { get; set; } = default!;

    public string LongTimePattern { get; set; } = default!;
}
