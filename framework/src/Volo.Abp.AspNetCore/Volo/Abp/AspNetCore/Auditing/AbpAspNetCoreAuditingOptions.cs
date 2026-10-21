using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Auditing;

public class AbpAspNetCoreAuditingOptions
{
    /// <summary>
    /// This is used to disable the <see cref="AbpAuditingMiddleware"/>,
    /// app.UseAuditing(), for the specified URLs.
    /// <see cref="AbpAuditingMiddleware"/> will be disabled for URLs
    /// starting with an ignored URL.  
    /// </summary>
    public List<string> IgnoredUrls { get; } = new();

    /// <summary>
    /// SignalR hub types (including the derived types) that no audit log scope is created for,
    /// neither for their requests by the <see cref="AbpAuditingMiddleware"/> nor for their method calls by the ABP SignalR integration.
    /// The audited services called by these hubs still create and save their own audit logs.
    /// </summary>
    public List<Type> IgnoredHubTypes { get; } = new();
}
