using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Internal.Telemetry.Activity;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Volo.Abp.Internal.Telemetry.Activity.Providers;
using Volo.Abp.Internal.Telemetry.Constants;
using Volo.Abp.Reflection;

namespace Volo.Abp.Domain.Telemetry;

[ExposeServices(typeof(ITelemetryActivityEventEnricher), typeof(IHasParentTelemetryActivityEventEnricher<TelemetryApplicationInfoEnricher>))]
public class TelemetryDomainInfoEnricher : TelemetryActivityEventEnricher, IHasParentTelemetryActivityEventEnricher<TelemetryApplicationInfoEnricher>
{
    private readonly ITypeFinder _typeFinder;

    public TelemetryDomainInfoEnricher(ITypeFinder typeFinder, IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _typeFinder = typeFinder;
    }

    protected override Task<bool> CanExecuteAsync(ActivityContext context)
    {
        return Task.FromResult(context.ProjectId.HasValue);
    }

    protected override Task ExecuteAsync(ActivityContext context)
    {
        var entityCount = _typeFinder.Types.Count(t =>
            typeof(IEntity).IsAssignableFrom(t) && !t.IsAbstract &&
            !t.AssemblyQualifiedName!.StartsWith(TelemetryConsts.VoloNameSpaceFilter));

        context.Current[ActivityPropertyNames.EntityCount] = entityCount;
        
        return Task.CompletedTask;
    }

}