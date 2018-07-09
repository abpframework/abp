using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Auditing;

namespace Volo.Abp.AspNetCore.Auditing
{
    public class HttpContextClientInfoProvider : IClientInfoProvider
    {
        public string BrowserInfo => GetBrowserInfo();

        public string ClientIpAddress => GetClientIpAddress();

        public string ComputerName => GetComputerName();

        public ILogger<HttpContextClientInfoProvider> Logger { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly HttpContext _httpContext;

        /// <summary>
        /// Creates a new <see cref="HttpContextClientInfoProvider"/>.
        /// </summary>
        public HttpContextClientInfoProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = httpContextAccessor.HttpContext;

            Logger = NullLogger<HttpContextClientInfoProvider>.Instance;
        }

        protected virtual string GetBrowserInfo()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? _httpContext;
            return httpContext?.Request?.Headers?["User-Agent"];
        }

        protected virtual string GetClientIpAddress()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext ?? _httpContext;
                return httpContext?.Connection?.RemoteIpAddress?.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, LogLevel.Warning);
            }

            return null;
        }

        protected virtual string GetComputerName()
        {
            return null; //TODO: Implement!
        }
    }
}
