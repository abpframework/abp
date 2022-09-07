using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;

namespace Volo.Abp.AspNetCore.Auditing;

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

        if (httpContext.WebSockets.IsWebSocketRequest)
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
        if (context.AuditInfo.HttpStatusCode != null)
        {
            return;
        }

        var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        if (httpContext == null)
        {
            return;
        }

        if (context.AuditInfo.Exceptions.Any())
        {
            var httpExceptionStatusCodeFinder = context.ServiceProvider.GetRequiredService<IHttpExceptionStatusCodeFinder>();
            foreach (var auditInfoException in context.AuditInfo.Exceptions)
            {
                var statusCode = httpExceptionStatusCodeFinder.GetStatusCode(httpContext, auditInfoException);
                context.AuditInfo.HttpStatusCode = (int) statusCode;
            }

            if (context.AuditInfo.HttpStatusCode != null)
            {
                return;
            }
        }

        context.AuditInfo.HttpStatusCode = httpContext.Response.StatusCode;
    }

    protected virtual string BuildUrl(HttpContext httpContext)
    {
        //TODO: Add options to include/exclude query, schema and host

        var uriBuilder = new UriBuilder
        {
            Scheme = httpContext.Request.Scheme,
            Host = httpContext.Request.Host.Host,
            Path = httpContext.Request.Path.ToString(),
            Query = httpContext.Request.QueryString.ToString()
        };

        return uriBuilder.Uri.AbsolutePath;
    }
}
