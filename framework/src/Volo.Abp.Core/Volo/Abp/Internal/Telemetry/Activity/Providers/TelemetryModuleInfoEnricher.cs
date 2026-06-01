using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Volo.Abp.Internal.Telemetry.Constants;
using Volo.Abp.Internal.Telemetry.Constants.Enums;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;

namespace Volo.Abp.Internal.Telemetry.Activity.Providers;

[ExposeServices(typeof(ITelemetryActivityEventEnricher), typeof(IHasParentTelemetryActivityEventEnricher<TelemetryApplicationInfoEnricher>))]
internal sealed class TelemetryModuleInfoEnricher : TelemetryActivityEventEnricher, IHasParentTelemetryActivityEventEnricher<TelemetryApplicationInfoEnricher>
{
    private readonly IModuleContainer _moduleContainer;
    private readonly IAssemblyFinder _assemblyFinder;

    public TelemetryModuleInfoEnricher(IModuleContainer moduleContainer, IAssemblyFinder assemblyFinder,
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _moduleContainer = moduleContainer;
        _assemblyFinder = assemblyFinder;
    }

    protected override Task<bool> CanExecuteAsync(ActivityContext context)
    {
        return Task.FromResult(context.SessionType == SessionType.ApplicationRuntime);
    }

    protected override Task ExecuteAsync(ActivityContext context)
    {
        context.Current[ActivityPropertyNames.ModuleCount] = _moduleContainer.Modules.Count;
        context.Current[ActivityPropertyNames.ProjectCount] = _assemblyFinder.Assemblies.Count(x =>
            !x.FullName.IsNullOrEmpty() && 
            !x.FullName.StartsWith(TelemetryConsts.VoloNameSpaceFilter));
        return Task.CompletedTask;
    }
}