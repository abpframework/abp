using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Internal.Telemetry.Activity;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Volo.Abp.Internal.Telemetry.Activity.Providers;
using Volo.Abp.Internal.Telemetry.Constants;
using Volo.Abp.Internal.Telemetry.Constants.Enums;
using Volo.Abp.Reflection;

namespace Volo.Abp.AspNetCore.Mvc;

[ExposeServices(typeof(ITelemetryActivityEventEnricher), typeof(IHasParentTelemetryActivityEventEnricher<TelemetryApplicationInfoEnricher>))]
public sealed class TelemetryApplicationMetricsEnricher : TelemetryActivityEventEnricher, IHasParentTelemetryActivityEventEnricher<TelemetryApplicationInfoEnricher>
{
    private readonly ITypeFinder _typeFinder;
    public TelemetryApplicationMetricsEnricher(ITypeFinder typeFinder, IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _typeFinder = typeFinder;
    }

    protected override Task<bool> CanExecuteAsync(ActivityContext context)
    {
        return Task.FromResult(context.SessionType == SessionType.ApplicationRuntime);
    }

    protected override Task ExecuteAsync(ActivityContext context)
    {
        var appServiceCount = _typeFinder.Types.Count(t =>
            typeof(IApplicationService).IsAssignableFrom(t) &&
            t is { IsAbstract: false, IsInterface: false } &&
            !t.AssemblyQualifiedName!.StartsWith(TelemetryConsts.VoloNameSpaceFilter));

        var controllerCount = _typeFinder.Types.Count(t =>
            typeof(ControllerBase).IsAssignableFrom(t) &&
            !t.IsAbstract &&
            !t.AssemblyQualifiedName!.StartsWith(TelemetryConsts.VoloNameSpaceFilter));


        context.Current[ActivityPropertyNames.AppServiceCount] = appServiceCount;
        context.Current[ActivityPropertyNames.ControllerCount] = controllerCount;
        return Task.CompletedTask;
    }
}