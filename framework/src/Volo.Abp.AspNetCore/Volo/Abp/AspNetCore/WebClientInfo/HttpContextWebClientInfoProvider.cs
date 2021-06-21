using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.WebClientInfo
{
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

        public string BrowserInfo => GetBrowserInfo();

        public string ClientIpAddress => GetClientIpAddress();

        protected virtual string GetBrowserInfo()
        {
            return HttpContextAccessor.HttpContext?.Request?.Headers?["User-Agent"];
        }

        protected virtual string GetClientIpAddress()
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
    }
}
