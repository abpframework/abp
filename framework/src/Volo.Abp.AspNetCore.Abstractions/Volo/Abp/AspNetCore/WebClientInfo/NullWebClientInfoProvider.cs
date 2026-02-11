namespace Volo.Abp.AspNetCore.WebClientInfo;

public class NullWebClientInfoProvider : IWebClientInfoProvider
{
    public virtual string? BrowserInfo { get; }

    public virtual string? ClientIpAddress { get; }

    public virtual string? DeviceInfo { get; }
}
