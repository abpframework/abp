using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.AspNetCore.WebClientInfo;
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

            var clientInfoProvider = context.ServiceProvider.GetRequiredService<IWebClientInfoProvider>();
            if (context.AuditInfo.ClientIpAddress == null)
            {
                context.AuditInfo.ClientIpAddress = clientInfoProvider.ClientIpAddress;
            }

            if (context.AuditInfo.BrowserInfo == null)
            {
                context.AuditInfo.BrowserInfo = clientInfoProvider.BrowserInfo;
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
