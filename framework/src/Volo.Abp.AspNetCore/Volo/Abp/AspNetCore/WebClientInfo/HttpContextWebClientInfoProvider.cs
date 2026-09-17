using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyCSharp.HttpUserAgentParser;
using MyCSharp.HttpUserAgentParser.Providers;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.WebClientInfo;

[Dependency(ReplaceServices = true)]
public class HttpContextWebClientInfoProvider : IWebClientInfoProvider, ITransientDependency
{
    protected ILogger<HttpContextWebClientInfoProvider> Logger { get; }
    protected IHttpContextAccessor HttpContextAccessor { get; }
    protected IHttpUserAgentParserProvider HttpUserAgentParser  { get; }

    public HttpContextWebClientInfoProvider(
        ILogger<HttpContextWebClientInfoProvider> logger,
        IHttpContextAccessor httpContextAccessor,
        IHttpUserAgentParserProvider httpUserAgentParser)
    {
        Logger = logger;
        HttpContextAccessor = httpContextAccessor;
        HttpUserAgentParser = httpUserAgentParser;
    }

    public string? BrowserInfo => GetBrowserInfo();

    public string? ClientIpAddress => GetClientIpAddress();

    public string? DeviceInfo => GetDeviceInfo();

    protected virtual string? GetBrowserInfo()
    {
        return HttpContextAccessor.HttpContext?.Request?.Headers?["User-Agent"];
    }

    protected virtual string? GetClientIpAddress()
    {
        try
        {
            return HttpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        }
        catch (Exception ex)
        {
            Logger.LogException(ex, LogLevel.Warning);
            return null;
        }
    }

    protected virtual string? GetDeviceInfo()
    {
        var browserInfo = GetBrowserInfo();
        if (browserInfo.IsNullOrWhiteSpace())
        {
            return null;
        }

        var httpUserAgentInformation = HttpUserAgentParser.Parse(browserInfo);
        switch (httpUserAgentInformation.Type)
        {
            case HttpUserAgentType.Browser:
            case HttpUserAgentType.Robot:
                return (httpUserAgentInformation.Platform.HasValue ?  httpUserAgentInformation.Platform.Value.Name + " " : string.Empty) + httpUserAgentInformation.Name;
            case HttpUserAgentType.Unknown:
            default:
                return httpUserAgentInformation.UserAgent;
        }
    }
}
