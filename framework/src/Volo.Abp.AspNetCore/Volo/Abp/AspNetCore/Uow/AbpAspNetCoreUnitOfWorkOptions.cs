using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Uow;

public class AbpAspNetCoreUnitOfWorkOptions
{
    /// <summary>
    /// This is used to disable the <see cref="AbpUnitOfWorkMiddleware"/>,
    /// app.UseUnitOfWork(), for the specified URLs.
    /// <see cref="AbpUnitOfWorkMiddleware"/> will be disabled for URLs
    /// starting with an ignored URL.  
    /// </summary>
    public List<string> IgnoredUrls { get; } = new List<string>();

    /// <summary>
    /// Completes the request unit of work just before the response starts (on
    /// <c>HttpResponse.OnStarting</c>) instead of at the end of the pipeline, so data written during
    /// the request is committed before the response is flushed. Disabled by default; enable it here
    /// globally or opt-in per endpoint via <see cref="CompleteUnitOfWorkOnResponseStartingUrls"/>.
    /// <para>
    /// Trade-offs when it applies: an exception after the response starts can no longer roll back the
    /// committed data (commit and network response are not atomic); database access after the response
    /// starts is outside the request unit of work (unsuitable for streaming responses); unit of work
    /// events and completed handlers run before the first response byte (adding to its latency); a
    /// nested (requiresNew) unit of work that is current when the response starts, and an active child
    /// unit of work scope (begun without requiresNew), are left to their owners and the request unit of
    /// work then completes at the end of the pipeline as usual.
    /// </para>
    /// </summary>
    public bool CompleteUnitOfWorkOnResponseStarting { get; set; } = false;

    /// <summary>
    /// Request path prefixes that opt-in to <see cref="CompleteUnitOfWorkOnResponseStarting"/> even when
    /// it is globally disabled. A request whose path starts with one of these values (for example
    /// "/connect") is included, matched like <see cref="IgnoredUrls"/>. Use
    /// <see cref="CompleteUnitOfWorkOnResponseStarting"/> to enable it for every request handled by the middleware.
    /// </summary>
    public List<string> CompleteUnitOfWorkOnResponseStartingUrls { get; } = new List<string>();
}
