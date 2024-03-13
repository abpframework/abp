using System;
using DeviceDetectorNET;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.WebClientInfo;

[Dependency(ReplaceServices = true)]
public class HttpContextWebClientInfoProvider : IWebClientInfoProvider, ITransientDependency
{
    protected ILogger<HttpContextWebClientInfoProvider> Logger { get; }
    protected IHttpContextAccessor HttpContextAccessor { get; }

    public HttpContextWebClientInfoProvider(
        ILogger<HttpContextWebClientInfoProvider> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        Logger = logger;
        HttpContextAccessor = httpContextAccessor;
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
        string? deviceInfo = null;
        var deviceDetector = new DeviceDetector(GetBrowserInfo());
        deviceDetector.Parse();
        if (!deviceDetector.IsParsed())
        {
            return deviceInfo;
        }

        var osInfo = deviceDetector.GetOs();
        if (osInfo.Success)
        {
            deviceInfo = osInfo.Match.Name;
        }

        var clientInfo = deviceDetector.GetClient();
        if (clientInfo.Success)
        {
            deviceInfo = deviceInfo.IsNullOrWhiteSpace() ? clientInfo.Match.Name : deviceInfo + " " + clientInfo.Match.Name;
        }

        return deviceInfo;
    }

}
