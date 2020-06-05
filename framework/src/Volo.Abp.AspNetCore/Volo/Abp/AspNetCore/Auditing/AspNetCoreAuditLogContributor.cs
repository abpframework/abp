using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Auditing
{
    public class AspNetCoreAuditLogContributor : AuditLogContributor, ITransientDependency
    {
        public ILogger<AspNetCoreAuditLogContributor> Logger { get; set; }

        public AspNetCoreAuditLogContributor()
        {
            Logger = NullLogger<AspNetCoreAuditLogContributor>.Instance;
        }

        public override void PreContribute(AuditLogContributionContext context)
        {
            var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            if (httpContext == null)
            {
                return;
            }

            if (context.AuditInfo.HttpMethod == null)
            {
                context.AuditInfo.HttpMethod = httpContext.Request.Method;
            }

            if (context.AuditInfo.Url == null)
            {
                context.AuditInfo.Url = BuildUrl(httpContext);
            }

            if (context.AuditInfo.ClientIpAddress == null)
            {
                context.AuditInfo.ClientIpAddress = GetClientIpAddress(httpContext);
            }

            if (context.AuditInfo.BrowserInfo == null)
            {
                context.AuditInfo.BrowserInfo = GetBrowserInfo(httpContext);
            }

            //TODO: context.AuditInfo.ClientName
        }

        public override void PostContribute(AuditLogContributionContext context)
        {
            var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            if (httpContext == null)
            {
                return;
            }

            if (context.AuditInfo.HttpStatusCode == null)
            {
                context.AuditInfo.HttpStatusCode = httpContext.Response.StatusCode;
            }
        }

        protected virtual string GetBrowserInfo(HttpContext httpContext)
        {
            return httpContext.Request?.Headers?["User-Agent"];
        }

        protected virtual string GetClientIpAddress(HttpContext httpContext)
        {
            try
            {
                return httpContext.Connection?.RemoteIpAddress?.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, LogLevel.Warning);
                return null;
            }
        }

        protected virtual string BuildUrl(HttpContext httpContext)
        {
            //TODO: Add options to include/exclude query, schema and host

            var uriBuilder = new UriBuilder();

            uriBuilder.Scheme = httpContext.Request.Scheme;
            uriBuilder.Host = httpContext.Request.Host.Host;
            uriBuilder.Path = httpContext.Request.Path.ToString();
            uriBuilder.Query = httpContext.Request.QueryString.ToString();

            return uriBuilder.Uri.AbsolutePath;
        }
    }
}
