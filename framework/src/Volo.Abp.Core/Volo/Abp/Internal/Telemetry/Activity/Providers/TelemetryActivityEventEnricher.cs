using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;

namespace Volo.Abp.Internal.Telemetry.Activity.Providers;

public abstract class TelemetryActivityEventEnricher : ITelemetryActivityEventEnricher, IScopedDependency
{
    public virtual int ExecutionOrder { get; set; } = 0;
    protected bool IgnoreChildren { get; set; }
    protected virtual Type? ReplaceParentType { get; set; }

    private readonly IServiceProvider _serviceProvider;

    protected TelemetryActivityEventEnricher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task EnrichAsync(ActivityContext context)
    {
        if (!await CanExecuteAsync(context))
        {
            return;
        }

        await ExecuteAsync(context);
        await ExecuteChildrenAsync(context);
    }

    protected virtual Task<bool> CanExecuteAsync(ActivityContext context)
    {
        return Task.FromResult(true);
    }

    protected abstract Task ExecuteAsync(ActivityContext context);

    protected virtual async Task ExecuteChildrenAsync(ActivityContext context)
    {
        if (IgnoreChildren)
        {
            return;
        }

        using var scope = _serviceProvider.CreateScope();

        foreach (var child in GetChildren(scope.ServiceProvider))
        {
            await child.EnrichAsync(context);
        }
    }

    private ITelemetryActivityEventEnricher[] GetChildren(IServiceProvider serviceProvider)
    {
        var targetType = ReplaceParentType ?? ProxyHelper.GetUnProxiedType(this);
        var genericInterfaceType = typeof(IHasParentTelemetryActivityEventEnricher<>).MakeGenericType(targetType);
        var enumerableType = typeof(IEnumerable<>).MakeGenericType(genericInterfaceType);

        var childServices = (IEnumerable<object>)serviceProvider.GetRequiredService(enumerableType);

        return childServices
            .Cast<ITelemetryActivityEventEnricher>()
            .ToArray();
    }
}