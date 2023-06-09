using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Tracing;

namespace Volo.Abp.AspNetCore.Tracing;

[Dependency(ReplaceServices = true)]
public class AspNetCoreCorrelationIdProvider : DefaultCorrelationIdProvider, ITransientDependency
{
    protected IHttpContextAccessor HttpContextAccessor { get; }
    protected AbpCorrelationIdOptions Options { get; }

    public AspNetCoreCorrelationIdProvider(
        IHttpContextAccessor httpContextAccessor,
        IOptions<AbpCorrelationIdOptions> options)
    {
        HttpContextAccessor = httpContextAccessor;
        Options = options.Value;
    }

    protected override string GetDefaultCorrelationId()
    {
        if (HttpContextAccessor.HttpContext?.Request?.Headers == null)
        {
            return base.GetDefaultCorrelationId();
        }

        string correlationId = HttpContextAccessor.HttpContext.Request.Headers[Options.HttpHeaderName];

        if (correlationId.IsNullOrEmpty())
        {
            lock (HttpContextAccessor.HttpContext.Request.Headers)
            {
                if (correlationId.IsNullOrEmpty())
                {
                    correlationId = base.GetDefaultCorrelationId();;
                    HttpContextAccessor.HttpContext.Request.Headers[Options.HttpHeaderName] = correlationId;
                }
            }
        }

        return correlationId;
    }
}
