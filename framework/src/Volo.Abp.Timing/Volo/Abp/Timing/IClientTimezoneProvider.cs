namespace Volo.Abp.Timing;

public interface ICurrentTimezoneProvider
{
    string? TimeZone { get; set; }
}
