using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Volo.Abp.Bundling;
using Volo.Abp.Cli.Build;
using Volo.Abp.Cli.Bundling.Scripts;
using Volo.Abp.Cli.Bundling.Styles;
using Volo.Abp.Cli.Configuration;
using Volo.Abp.Cli.Version;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Minify.Scripts;
using Volo.Abp.Minify.Styles;
using Volo.Abp.Modularity;

namespace Volo.Abp.Cli.Bundling;

public class BundlingService : IBundlingService, ITransientDependency
{
    public IDotNetProjectBuilder DotNetProjectBuilder { get; set; }
    public IJavascriptMinifier JsMinifier { get; set; }
    public ICssMinifier CssMinifier { get; set; }
    public ILogger<BundlingService> Logger { get; set; }
    public IScriptBundler ScriptBundler { get; set; }
    public IStyleBundler StyleBundler { get; set; }
    public IConfigReader ConfigReader { get; set; }
    public CliVersionService CliVersionService { get; set; }

    public async Task BundleAsync(string directory, bool forceBuild, string projectType = BundlingConsts.WebAssembly)
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && projectType == BundlingConsts.MauiBlazor)
        {
            Logger.LogWarning("ABP bundle command does not support OSX for MAUI Blazor");
            return;
        }

        var projectFiles = Directory.GetFiles(directory, "*.csproj");
        if (!projectFiles.Any())
        {
            throw new BundlingException(
                "No project file found in the directory. The working directory must have a Blazor project file.");
        }

        var projectFilePath = projectFiles[0];

        await CheckProjectIsSupportedTypeAsync(projectFilePath, projectType);

        var config = projectType == BundlingConsts.WebAssembly? ConfigReader.Read(PathHelper.GetWwwRootPath(directory)): ConfigReader.Read(directory);
        var bundleConfig = config.Bundle;

        if (forceBuild)
        {
            var projects = new List<DotNetProjectInfo>()
                {
                    new DotNetProjectInfo(string.Empty, projectFilePath, true)
                };

            DotNetProjectBuilder.BuildProjects(projects, string.Empty);
        }

        var frameworkVersion = GetTargetFrameworkVersion(projectFilePath, projectType);
        var projectName = Path.GetFileNameWithoutExtension(projectFilePath);
        var assemblyFilePath = projectType == BundlingConsts.WebAssembly? PathHelper.GetWebAssemblyFilePath(directory, frameworkVersion, projectName) : PathHelper.GetMauiBlazorAssemblyFilePath(directory, projectName);
        var startupModule = GetStartupModule(assemblyFilePath);

        var bundleDefinitions = new List<BundleTypeDefinition>();
        FindBundleContributorsRecursively(startupModule, 0, bundleDefinitions);
        bundleDefinitions = bundleDefinitions.OrderByDescending(t => t.Level).ToList();

        var styleContext = GetStyleContext(bundleDefinitions, bundleConfig);
        var scriptContext = GetScriptContext(bundleDefinitions, bundleConfig, projectType);
        string styleDefinitions;
        string scriptDefinitions;

        if (bundleConfig.Mode is BundlingMode.Bundle || bundleConfig.Mode is BundlingMode.BundleAndMinify)
        {
            var options = new BundleOptions
            {
                Directory = directory,
                FrameworkVersion = frameworkVersion,
                ProjectFileName = projectName,
                BundleName = bundleConfig.Name.IsNullOrEmpty() ? "global" : bundleConfig.Name,
                Minify = bundleConfig.Mode == BundlingMode.BundleAndMinify,
                ProjectType = projectType
            };

            Logger.LogInformation("Generating style bundle...");
            styleDefinitions = StyleBundler.Bundle(options, styleContext);
            Logger.LogInformation($"Style bundle has been generated successfully.");

            Logger.LogInformation("Generating script bundle...");
            scriptDefinitions = ScriptBundler.Bundle(options, scriptContext);
            Logger.LogInformation($"Script bundle has been generated successfully.");
        }
        else
        {
            Logger.LogInformation("Generating style references...");
            styleDefinitions = GenerateStyleDefinitions(styleContext);
            Logger.LogInformation("Generating script references...");
            scriptDefinitions = GenerateScriptDefinitions(scriptContext);
        }

        if (!bundleConfig.InteractiveAuto)
        {
            var fileName = bundleConfig.IsBlazorWebApp
                ? Directory.GetFiles(Path.GetDirectoryName(projectFilePath)!.Replace(".Client", ""), "App.razor", SearchOption.AllDirectories).FirstOrDefault() ??
                  Directory.GetFiles(Path.GetDirectoryName(projectFilePath)!.Replace(".Blazor", ".Host"), "App.razor", SearchOption.AllDirectories).FirstOrDefault()
                : Path.Combine(PathHelper.GetWwwRootPath(directory), "index.html");

            if (fileName == null)
            {
                throw new BundlingException($"App.razor file could not be found in the {projectFilePath} directory.");
            }

            await UpdateDependenciesInBlazorFileAsync(fileName, styleDefinitions, scriptDefinitions);

            Logger.LogInformation($"Script and style references in the {fileName} file have been updated.");
        }
    }

    private BundleContext GetScriptContext(List<BundleTypeDefinition> bundleDefinitions, BundleConfig bundleConfig, string projectType)
    {
        var scriptContext = new BundleContext
        {
            Parameters = bundleConfig.Parameters,
            InteractiveAuto = bundleConfig.InteractiveAuto
        };

        if (projectType == BundlingConsts.WebAssembly && !bundleConfig.IsBlazorWebApp)
        {
            scriptContext.BundleDefinitions.AddIfNotContains(
                x => x.Source == "_framework/blazor.webassembly.js",
                () => new BundleDefinition { Source = "_framework/blazor.webassembly.js" });
        }

        foreach (var bundleDefinition in bundleDefinitions)
        {
            var contributor = CreateContributorInstance(bundleDefinition.BundleContributorType);
            contributor.AddScripts(scriptContext);
        }

        return scriptContext;
    }

    private BundleContext GetStyleContext(List<BundleTypeDefinition> bundleDefinitions, BundleConfig bundleConfig)
    {
        var styleContext = new BundleContext
        {
            Parameters = bundleConfig.Parameters,
            InteractiveAuto = bundleConfig.InteractiveAuto
        };

        foreach (var bundleDefinition in bundleDefinitions)
        {
            var contributor = CreateContributorInstance(bundleDefinition.BundleContributorType);
            contributor.AddStyles(styleContext);
        }

        return styleContext;
    }

    private async Task UpdateDependenciesInBlazorFileAsync(string fileName, string styleDefinitions, string scriptDefinitions)
    {
        if (!File.Exists(fileName))
        {
            throw new BundlingException($"{fileName} file could not be found.");
        }

        Encoding fileEncoding;
        string content;
        using (var reader = new StreamReader(fileName, true))
        {
            fileEncoding = reader.CurrentEncoding;
            content = await reader.ReadToEndAsync();
        }

        content = UpdatePlaceholders(content, BundlingConsts.StylePlaceholderStart,
            BundlingConsts.StylePlaceholderEnd, styleDefinitions);
        content = UpdatePlaceholders(content, BundlingConsts.ScriptPlaceholderStart,
            BundlingConsts.ScriptPlaceholderEnd, scriptDefinitions);

        using (var writer = new StreamWriter(fileName, false, fileEncoding))
        {
            await writer.WriteAsync(content);
            await writer.FlushAsync();
        }
    }

    private string UpdatePlaceholders(string content, string placeholderStart, string placeholderEnd,
        string definitions)
    {
        var placeholderStartIndex = content.IndexOf(placeholderStart);
        var placeholderEndIndex = content.IndexOf(placeholderEnd);
        var updatedContent = content.Remove(placeholderStartIndex,
            (placeholderEndIndex + placeholderEnd.Length) - placeholderStartIndex);
        return updatedContent.Insert(placeholderStartIndex, definitions);
    }

    private string GenerateStyleDefinitions(BundleContext context)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"{BundlingConsts.StylePlaceholderStart}");

        foreach (var style in context.BundleDefinitions)
        {
            builder.Append($"    <link href=\"{style.Source}\" rel=\"stylesheet\"");

            foreach (var additionalProperty in style.AdditionalProperties)
            {
                builder.Append($"{additionalProperty.Key}={additionalProperty.Value} ");
            }

            builder.AppendLine("/>");
        }

        builder.Append($"    {BundlingConsts.StylePlaceholderEnd}");

        return builder.ToString();
    }

    private string GenerateScriptDefinitions(BundleContext context)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"{BundlingConsts.ScriptPlaceholderStart}");
        foreach (var script in context.BundleDefinitions)
        {
            builder.Append($"    <script src=\"{script.Source}\"");
            foreach (var additionalProperty in script.AdditionalProperties)
            {
                builder.Append($" {additionalProperty.Key}={additionalProperty.Value} ");
            }

            builder.AppendLine("></script>");
        }

        builder.Append($"    {BundlingConsts.ScriptPlaceholderEnd}");

        return builder.ToString();
    }

    private IBundleContributor CreateContributorInstance(Type bundleContributorType)
    {
        return (IBundleContributor)Activator.CreateInstance(bundleContributorType);
    }

    private void FindBundleContributorsRecursively(
        Type module,
        int level,
        List<BundleTypeDefinition> bundleDefinitions)
    {
        var bundleContributors = module.Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface && t.IsAssignableTo<IBundleContributor>())
            .ToList();

        if (bundleContributors.Count > 1)
        {
            throw new BundlingException(
                $"Each project must contain only one class implementing {nameof(IBundleContributor)}");
        }

        if (bundleContributors.Any())
        {
            var bundleContributor = bundleContributors[0];
            var definition = bundleDefinitions.SingleOrDefault(t => t.BundleContributorType == bundleContributor);
            if (definition != null)
            {
                if (definition.Level < level)
                {
                    definition.Level = level;
                }
            }
            else
            {
                bundleDefinitions.Add(new BundleTypeDefinition
                {
                    Level = level,
                    BundleContributorType = bundleContributor
                });
            }
        }

        var dependencyDescriptors = module
            .GetCustomAttributes()
            .OfType<IDependedTypesProvider>();

        foreach (var descriptor in dependencyDescriptors)
        {
            foreach (var dependedModuleType in descriptor.GetDependedTypes())
            {
                FindBundleContributorsRecursively(dependedModuleType, level + 1, bundleDefinitions);
            }
        }
    }

    private Type GetStartupModule(string assemblyPath)
    {
        return Assembly
            .LoadFrom(assemblyPath)
            .GetTypes()
            .SingleOrDefault(AbpModule.IsAbpModule);
    }

    private string GetTargetFrameworkVersion(string projectFilePath, string projectType)
    {
        var document = new XmlDocument();
        document.Load(projectFilePath);

        return projectType switch {
            BundlingConsts.WebAssembly => document.SelectSingleNode("//TargetFramework").InnerText,
            BundlingConsts.MauiBlazor => document.SelectNodes("//TargetFrameworks")[0].InnerText,
            _ => null
        };
    }

    private async Task CheckProjectIsSupportedTypeAsync(string projectFilePath, string projectType)
    {
        var document = new XmlDocument();
        document.Load(projectFilePath);

        var sdk = document.DocumentElement.GetAttribute("Sdk");

        switch (projectType)
        {
            case BundlingConsts.WebAssembly:
                if (sdk != BundlingConsts.SupportedWebAssemblyProjectType)
                {
                    throw new BundlingException(
                        $"Unsupported project type. Project type must be {BundlingConsts.SupportedWebAssemblyProjectType}.");
                }
                break;
            case BundlingConsts.MauiBlazor:
                if (sdk != BundlingConsts.SupportedMauiBlazorProjectType)
                {
                    throw new BundlingException(
                        $"Unsupported project type. Project type must be {BundlingConsts.SupportedMauiBlazorProjectType}.");
                }
                break;
        }

        var targetFramework = document.SelectSingleNode("//TargetFramework")?.InnerText ??
                              document.SelectNodes("//TargetFrameworks")[0].InnerText;
        var currentCliVersion = await CliVersionService.GetCurrentCliVersionAsync();

        if (targetFramework.IsNullOrWhiteSpace() ||
            targetFramework.IndexOf($"net{currentCliVersion.Major}.0", StringComparison.OrdinalIgnoreCase) < 0)
        {
            throw new BundlingException($"Your project({projectFilePath}) target framework is {targetFramework}. " + Environment.NewLine +
                                        $"But ABP CLI version is {currentCliVersion}. " + Environment.NewLine +
                                        $"Please use the ABP CLI that is compatible with your project target framework.");
        }
    }
}
