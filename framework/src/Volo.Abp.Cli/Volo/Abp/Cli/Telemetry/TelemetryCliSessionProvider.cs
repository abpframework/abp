using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal.Telemetry.Activity;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Volo.Abp.Internal.Telemetry.Activity.Providers;
using Volo.Abp.Internal.Telemetry.Constants;
using Volo.Abp.Internal.Telemetry.Constants.Enums;

namespace Volo.Abp.Cli.Telemetry;

[ExposeServices(typeof(ITelemetryActivityEventEnricher))]
public class TelemetryCliSessionProvider : TelemetryActivityEventEnricher
{
    public TelemetryCliSessionProvider(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override int ExecutionOrder { get; set; } = 10;
    protected override Type? ReplaceParentType { get; set; } = typeof(TelemetrySessionInfoEnricher);

    protected override Task ExecuteAsync(ActivityContext context)
    {
        context.Current[ActivityPropertyNames.SessionType] = SessionType.AbpCli;
        context.Current[ActivityPropertyNames.SessionId] = Guid.NewGuid();
        context.Current[ActivityPropertyNames.IsFirstSession] = !File.Exists(TelemetryPaths.ActivityStorage);
        
        return Task.CompletedTask;
    }
}