namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class TimingDto
{
    public TimeZone TimeZone { get; set; }

    public TimingDto()
    {
        TimeZone = new TimeZone();
    }
}

public class TimeZone
{
    public IanaTimeZone Iana { get; set; }

    public WindowsTimeZone Windows { get; set; }

    public TimeZone()
    {
        Iana = new IanaTimeZone();
        Windows = new WindowsTimeZone();
    }
}

public class WindowsTimeZone
{
    public string? TimeZoneId { get; set; }
}

public class IanaTimeZone
{
    public string? TimeZoneName { get; set; }
}
