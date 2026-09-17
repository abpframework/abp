using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Volo.Abp.Internal.Telemetry.Constants;
using Volo.Abp.Internal.Telemetry.Constants.Enums;

namespace Volo.Abp.Internal.Telemetry.Activity.Providers;
[ExposeServices(typeof(ITelemetryActivityEventEnricher))]
public class TelemetrySessionInfoEnricher : TelemetryActivityEventEnricher 
{
    public override int ExecutionOrder { get; set; } = 10;
    
    public TelemetrySessionInfoEnricher(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override Task ExecuteAsync(ActivityContext context)
    {
        context.Current[ActivityPropertyNames.SessionType] = SessionType.ApplicationRuntime;
        context.Current[ActivityPropertyNames.SessionId] = Guid.NewGuid().ToString();
        context.Current[ActivityPropertyNames.IsFirstSession] = !File.Exists(TelemetryPaths.ActivityStorage);

        return Task.CompletedTask;
    }
}