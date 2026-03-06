using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.OperationRateLimiting;

public class OperationRateLimitingContext
{
    /// <summary>
    /// Optional parameter passed by the caller.
    /// Used by rules configured with PartitionByParameter().
    /// Can be email, phone number, resource id, or any string.
    /// </summary>
    public string? Parameter { get; set; }

    /// <summary>
    /// Additional properties that can be read by custom <see cref="IOperationRateLimitingRule"/> implementations
    /// and are forwarded to the exception's Data dictionary when the rate limit is exceeded.
    /// </summary>
    public Dictionary<string, object?> ExtraProperties { get; set; } = new();

    /// <summary>
    /// The service provider for resolving services.
    /// Set automatically by the checker.
    /// </summary>
    public IServiceProvider ServiceProvider { get; set; } = default!;

    public T GetRequiredService<T>() where T : notnull
        => ServiceProvider.GetRequiredService<T>();

    public T? GetService<T>() => ServiceProvider.GetService<T>();
}
