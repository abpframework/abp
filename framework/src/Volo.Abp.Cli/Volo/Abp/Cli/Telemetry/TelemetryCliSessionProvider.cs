using System;
using System.Collections.Generic;
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

        if (context.ExtraProperties.ContainsKey(ActivityPropertyNames.SolutionPath))
        {
            return Task.CompletedTask;
        }
        
        if(context.Current.TryGetValue(ActivityPropertyNames.SolutionPath, out var existingSolutionPath) && existingSolutionPath is string)
        {
            context.ExtraProperties[ActivityPropertyNames.SolutionPath] = existingSolutionPath;
            return Task.CompletedTask;
        }

        if (context.Current.TryGetValue(ActivityPropertyNames.AdditionalProperties, out var additionalProperties) &&
            additionalProperties is Dictionary<string, object> additionalPropertiesDict &&
            additionalPropertiesDict.TryGetValue(ActivityPropertyNames.SolutionPath, out var solutionPath))
        {
            context.ExtraProperties[ActivityPropertyNames.SolutionPath] = solutionPath;
        }
        
        return Task.CompletedTask;
    }

}