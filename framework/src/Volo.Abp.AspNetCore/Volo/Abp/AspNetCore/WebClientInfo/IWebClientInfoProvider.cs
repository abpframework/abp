namespace Volo.Abp.AspNetCore.WebClientInfo
{
    public interface IWebClientInfoProvider
    {
        string BrowserInfo { get; }

        string ClientIpAddress { get; }
    }
}
