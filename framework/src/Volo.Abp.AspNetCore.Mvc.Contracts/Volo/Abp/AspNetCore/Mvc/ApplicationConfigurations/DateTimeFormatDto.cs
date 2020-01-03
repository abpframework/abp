namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations
{
    public class DateTimeFormatDto
    {
        public string CalendarAlgorithmType { get; set; }

        public string DateTimeFormatLong { get; set; }

        public string ShortDatePattern { get; set; }

        public string FullDateTimePattern { get; set; }

        public string DateSeparator { get; set; }

        public string ShortTimePattern { get; set; }

        public string LongTimePattern { get; set; }
    }
}