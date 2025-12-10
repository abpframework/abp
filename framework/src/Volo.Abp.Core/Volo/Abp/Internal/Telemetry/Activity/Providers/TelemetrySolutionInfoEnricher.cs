using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Volo.Abp.Internal.Telemetry.Constants;

namespace Volo.Abp.Internal.Telemetry.Activity.Providers;

[ExposeServices(typeof(ITelemetryActivityEventEnricher), typeof(IHasParentTelemetryActivityEventEnricher<TelemetrySessionInfoEnricher>))]
internal sealed class TelemetrySolutionInfoEnricher : TelemetryActivityEventEnricher, IHasParentTelemetryActivityEventEnricher<TelemetrySessionInfoEnricher>
{
    private readonly ITelemetryActivityStorage _telemetryActivityStorage;

    public TelemetrySolutionInfoEnricher(ITelemetryActivityStorage telemetryActivityStorage, IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _telemetryActivityStorage = telemetryActivityStorage;
    }

    protected override Task<bool> CanExecuteAsync(ActivityContext context)
    {
        if (context.SolutionPath.IsNullOrEmpty())
        {
            return Task.FromResult(false);
        }

        if (context.SolutionId.HasValue)
        {
            return Task.FromResult(_telemetryActivityStorage.ShouldAddSolutionInformation(context.SolutionId.Value));
        }

        return Task.FromResult(true);
    }

    protected override Task ExecuteAsync(ActivityContext context)
    {
        try
        {
            if (context.SolutionPath.IsNullOrEmpty())
            {
                return Task.CompletedTask;
            }
            
            var correctSolutionPath = FindCorrectSolutionPath(context.SolutionPath);
            if (correctSolutionPath.IsNullOrEmpty())
            {
                return Task.CompletedTask;
            }
            
            context.ExtraProperties[ActivityPropertyNames.SolutionPath] = correctSolutionPath;

            var jsonContent = File.ReadAllText(context.SolutionPath!);
            using var doc = JsonDocument.Parse(jsonContent, new JsonDocumentOptions
            {
                AllowTrailingCommas = true
            });

            var root = doc.RootElement;
            
            var solutionId = TelemetryJsonExtensions.GetGuidOrNull(root, "id");

            if (!solutionId.HasValue)
            {
                return Task.CompletedTask;
            }
            
            context.Current[ActivityPropertyNames.SolutionId] = solutionId;

            if (root.TryGetProperty("versions", out var versions))
            {
                AddVersions(context, versions);
            }
            
            if (root.TryGetProperty("creatingStudioConfiguration", out var creatingStudioConfiguration))
            {
                AddSolutionCreationConfiguration(context, creatingStudioConfiguration);
            }

            if (root.TryGetProperty("modules", out var modulesElement))
            {
                AddModuleInfo(context, modulesElement);
            }

            context.Current[ActivityPropertyNames.HasSolutionInfo] = true;
        }
        catch
        {
            //ignored
        }

        return Task.CompletedTask;
    }

    private static void AddVersions(ActivityContext context, JsonElement config)
    {
        context.Current[ActivityPropertyNames.FirstAbpVersion] = TelemetryJsonExtensions.GetStringOrNull(config, "AbpFramework");
        context.Current[ActivityPropertyNames.FirstDotnetVersion] = TelemetryJsonExtensions.GetStringOrNull(config, "TargetDotnetFramework");
    }

    private static void AddSolutionCreationConfiguration(ActivityContext context, JsonElement config)
    {
        context.Current[ActivityPropertyNames.Template] = TelemetryJsonExtensions.GetStringOrNull(config, "template");
        context.Current[ActivityPropertyNames.CreatedAbpStudioVersion] = TelemetryJsonExtensions.GetStringOrNull(config, "createdAbpStudioVersion");
        context.Current[ActivityPropertyNames.MultiTenancy] = TelemetryJsonExtensions.GetBooleanOrNull(config, "multiTenancy");
        context.Current[ActivityPropertyNames.UiFramework] = TelemetryJsonExtensions.GetStringOrNull(config, "uiFramework");
        context.Current[ActivityPropertyNames.DatabaseProvider] = TelemetryJsonExtensions.GetStringOrNull(config, "databaseProvider");
        context.Current[ActivityPropertyNames.Theme] = TelemetryJsonExtensions.GetStringOrNull(config, "theme");
        context.Current[ActivityPropertyNames.ThemeStyle] = TelemetryJsonExtensions.GetStringOrNull(config, "themeStyle");
        context.Current[ActivityPropertyNames.HasPublicWebsite] = TelemetryJsonExtensions.GetBooleanOrNull(config, "publicWebsite");
        context.Current[ActivityPropertyNames.IsTiered] = TelemetryJsonExtensions.GetBooleanOrNull(config, "tiered");
        context.Current[ActivityPropertyNames.SocialLogins] = TelemetryJsonExtensions.GetBooleanOrNull(config, "socialLogin");
        context.Current[ActivityPropertyNames.DatabaseManagementSystem] = TelemetryJsonExtensions.GetStringOrNull(config, "databaseManagementSystem");
        context.Current[ActivityPropertyNames.IsSeparateTenantSchema] = TelemetryJsonExtensions.GetBooleanOrNull(config, "separateTenantSchema");
        context.Current[ActivityPropertyNames.MobileFramework] = TelemetryJsonExtensions.GetStringOrNull(config, "mobileFramework");
        context.Current[ActivityPropertyNames.IncludeTests] = TelemetryJsonExtensions.GetBooleanOrNull(config, "includeTests");
        context.Current[ActivityPropertyNames.DynamicLocalization] = TelemetryJsonExtensions.GetBooleanOrNull(config, "dynamicLocalization");
        context.Current[ActivityPropertyNames.KubernetesConfiguration] = TelemetryJsonExtensions.GetBooleanOrNull(config, "kubernetesConfiguration");
        context.Current[ActivityPropertyNames.GrafanaDashboard] = TelemetryJsonExtensions.GetBooleanOrNull(config, "grafanaDashboard");
        context.Current[ActivityPropertyNames.SampleCrudPage] = TelemetryJsonExtensions.GetBooleanOrNull(config, "sampleCrudPage");
        context.Current[ActivityPropertyNames.CreationTool] = TelemetryJsonExtensions.GetStringOrNull(config, "creationTool");
        context.Current[ActivityPropertyNames.Aspire] = TelemetryJsonExtensions.GetBooleanOrNull(config, "aspire");
    }

    private static void AddModuleInfo(ActivityContext context, JsonElement modulesElement)
    {
        var modules = new List<Dictionary<string, object?>>();

        foreach (var module in modulesElement.EnumerateObject())
        {
            var modulePath = GetModuleFilePath(context.SolutionPath!, module);
            if (modulePath.IsNullOrEmpty())
            {
                continue;
            }

            var moduleJsonFileContent = File.ReadAllText(modulePath);
            using var moduleDoc = JsonDocument.Parse(moduleJsonFileContent, new JsonDocumentOptions
            {
                AllowTrailingCommas = true
            });

            if (!moduleDoc.RootElement.TryGetProperty("imports", out var imports))
            {
                continue;
            }

            foreach (var import in imports.EnumerateObject())
            {
                modules.Add(new Dictionary<string, object?>
                {
                    { ActivityPropertyNames.ModuleName, import.Name },
                    { ActivityPropertyNames.ModuleVersion, TelemetryJsonExtensions.GetStringOrNull(import.Value, "version") },
                    { ActivityPropertyNames.ModuleInstallationTime, TelemetryJsonExtensions.GetDateTimeOffsetOrNull(import.Value, "creationTime") }
                });
            }
        }

        context.Current[ActivityPropertyNames.InstalledModules] = modules;
    }

    private static string? GetModuleFilePath(string solutionPath, JsonProperty module)
    {
        var path = TelemetryJsonExtensions.GetStringOrNull(module.Value, "path");
        if (path.IsNullOrEmpty())
        {
            return null;
        }

        var fullPath = Path.Combine(Path.GetDirectoryName(solutionPath)!, path);
        return File.Exists(fullPath) ? fullPath : null;
    }
    
    private static string? FindCorrectSolutionPath(string solutionPath)
    {
        if (solutionPath.EndsWith(".abpsln"))
        {
            return solutionPath;
        }

        if (solutionPath.EndsWith(".sln"))
        {
            solutionPath = solutionPath[..^4] + ".abpsln";
            if (File.Exists(solutionPath))
            {
                return solutionPath;
            }
        }
        
        var dir = Path.GetDirectoryName(solutionPath);
        if (dir.IsNullOrEmpty())
        {
            return null;
        }
        
        var abpSolutionFiles = Directory.GetFiles(dir, "*.abpsln", SearchOption.TopDirectoryOnly);

        return abpSolutionFiles.Length switch
        {
            1 => abpSolutionFiles[0],
            _ => null
        };
    }
}