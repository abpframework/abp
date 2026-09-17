using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Volo.Abp.Internal.Telemetry.Constants;
using Volo.Abp.Internal.Telemetry.Constants.Enums;
using Volo.Abp.Internal.Telemetry.Helpers;

namespace Volo.Abp.Internal.Telemetry.Activity.Providers;

[ExposeServices(typeof(ITelemetryActivityEventEnricher), typeof(IHasParentTelemetryActivityEventEnricher<TelemetrySessionInfoEnricher>))]
public sealed class TelemetryApplicationInfoEnricher : TelemetryActivityEventEnricher, IHasParentTelemetryActivityEventEnricher<TelemetrySessionInfoEnricher>
{
    private readonly ITelemetryActivityStorage _telemetryActivityStorage;

    public TelemetryApplicationInfoEnricher(ITelemetryActivityStorage telemetryActivityStorage, IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _telemetryActivityStorage = telemetryActivityStorage;
    }

    protected override Task<bool> CanExecuteAsync(ActivityContext context)
    {
        return Task.FromResult(context.SessionType == SessionType.ApplicationRuntime);
    }

    protected override Task ExecuteAsync(ActivityContext context)
    {
        try
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly is null)
            {
                context.Terminate();
                return Task.CompletedTask;
            }

            var projectMetaData = AbpProjectMetadataReader.ReadProjectMetadata(entryAssembly);
            if (projectMetaData?.ProjectId == null || projectMetaData.AbpSlnPath.IsNullOrEmpty())
            {
                context.Terminate();
                return Task.CompletedTask;
            }
            
            var solutionId = ReadSolutionIdFromSolutionPath(projectMetaData.AbpSlnPath);
            if (!solutionId.HasValue)
            {
                IgnoreChildren = true;
                context.Terminate();
                return Task.CompletedTask;
            }
            context.Current[ActivityPropertyNames.SolutionId] = solutionId;

            if (!_telemetryActivityStorage.ShouldAddProjectInfo(projectMetaData.ProjectId.Value))
            {
                IgnoreChildren = true;
                return Task.CompletedTask;
            }
            
            context.ExtraProperties[ActivityPropertyNames.SolutionPath] = projectMetaData.AbpSlnPath;
            context.Current[ActivityPropertyNames.ProjectType] = projectMetaData.Role ?? string.Empty;
            context.Current[ActivityPropertyNames.ProjectId] = projectMetaData.ProjectId.Value;
            context.Current[ActivityPropertyNames.HasProjectInfo] = true;
        }
        catch
        {
            //ignored
        }

        return Task.CompletedTask;
    }


    private static Guid? ReadSolutionIdFromSolutionPath(string solutionPath)
    {
        try
        {
            if (solutionPath.IsNullOrEmpty())
            {
                return null;
            }

            using var fs = new FileStream(solutionPath!, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var doc = JsonDocument.Parse(fs, new JsonDocumentOptions
            {
                AllowTrailingCommas = true
            });
            if (doc.RootElement.TryGetProperty("id", out var property) && property.TryGetGuid(out var solutionId))
            {
                return solutionId;
            }
        }
        catch
        {
            // ignored
        }

        return null;
    }

}