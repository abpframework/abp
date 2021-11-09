using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Volo.Abp.Bundling;
using Volo.Abp.Cli.Build;
using Volo.Abp.Cli.Bundling.Scripts;
using Volo.Abp.Cli.Bundling.Styles;
using Volo.Abp.Cli.Configuration;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Minify.Scripts;
using Volo.Abp.Minify.Styles;
using Volo.Abp.Modularity;

namespace Volo.Abp.Cli.Bundling
{
    public class BundlingService : IBundlingService, ITransientDependency
    {
        public IDotNetProjectBuilder DotNetProjectBuilder { get; set; }
        public IJavascriptMinifier JsMinifier { get; set; }
        public ICssMinifier CssMinifier { get; set; }
        public ILogger<BundlingService> Logger { get; set; }
        public IScriptBundler ScriptBundler { get; set; }
        public IStyleBundler StyleBundler { get; set; }
        public IConfigReader ConfigReader { get; set; }

        public async Task BundleAsync(string directory, bool forceBuild)
        {
            var projectFiles = Directory.GetFiles(directory, "*.csproj");
            if (!projectFiles.Any())
            {
                throw new BundlingException(
                    "No project file found in the directory. The working directory must have a Blazor project file.");
            }

            var projectFilePath = projectFiles[0];
            
            CheckProjectIsSupportedType(projectFilePath);

            var config = ConfigReader.Read(PathHelper.GetWwwRootPath(directory));
            var bundleConfig = config.Bundle;

            if (forceBuild)
            {
                var projects = new List<DotNetProjectInfo>()
                {
                    new DotNetProjectInfo(string.Empty, projectFilePath, true)
                };

                DotNetProjectBuilder.BuildProjects(projects, string.Empty);
            }

            var frameworkVersion = GetTargetFrameworkVersion(projectFilePath);
            var projectName = Path.GetFileNameWithoutExtension(projectFilePath);
            var assemblyFilePath = PathHelper.GetAssemblyFilePath(directory, frameworkVersion, projectName);
            var startupModule = GetStartupModule(assemblyFilePath);

            var bundleDefinitions = new List<BundleTypeDefinition>();
            FindBundleContributorsRecursively(startupModule, 0, bundleDefinitions);
            bundleDefinitions = bundleDefinitions.OrderByDescending(t => t.Level).ToList();

            var styleContext = GetStyleContext(bundleDefinitions,bundleConfig.Parameters);
            var scriptContext = GetScriptContext(bundleDefinitions,bundleConfig.Parameters);
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
                    Minify = bundleConfig.Mode == BundlingMode.BundleAndMinify
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
            
            await UpdateDependenciesInHtmlFileAsync(directory, styleDefinitions, scriptDefinitions);
            Logger.LogInformation("Script and style references in the index.html file have been updated.");
        }

        private BundleContext GetScriptContext(List<BundleTypeDefinition> bundleDefinitions,
            BundleParameterDictionary parameters)
        {
            var scriptContext = new BundleContext
            {
                Parameters = parameters
            };

            foreach (var bundleDefinition in bundleDefinitions)
            {
                var contributor = CreateContributorInstance(bundleDefinition.BundleContributorType);
                contributor.AddScripts(scriptContext);
            }

            scriptContext.Add("_framework/blazor.webassembly.js");
            return scriptContext;
        }

        private BundleContext GetStyleContext(List<BundleTypeDefinition> bundleDefinitions,
            BundleParameterDictionary parameters)
        {
            var styleContext = new BundleContext
            {
                Parameters = parameters
            };

            foreach (var bundleDefinition in bundleDefinitions)
            {
                var contributor = CreateContributorInstance(bundleDefinition.BundleContributorType);
                contributor.AddStyles(styleContext);
            }

            return styleContext;
        }

        private async Task UpdateDependenciesInHtmlFileAsync(string directory, string styleDefinitions,
            string scriptDefinitions)
        {
            var htmlFilePath = Path.Combine(PathHelper.GetWwwRootPath(directory), "index.html");
            if (!File.Exists(htmlFilePath))
            {
                throw new BundlingException($"index.html file could not be found in the following path:{htmlFilePath}");
            }

            Encoding fileEncoding;
            string content;
            using (var reader = new StreamReader(htmlFilePath, true))
            {
                fileEncoding = reader.CurrentEncoding;
                content = await reader.ReadToEndAsync();
            }

            content = UpdatePlaceholders(content, BundlingConsts.StylePlaceholderStart,
                BundlingConsts.StylePlaceholderEnd, styleDefinitions);
            content = UpdatePlaceholders(content, BundlingConsts.ScriptPlaceholderStart,
                BundlingConsts.ScriptPlaceholderEnd, scriptDefinitions);

            using (var writer = new StreamWriter(htmlFilePath, false, fileEncoding))
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
                    builder.Append($"{additionalProperty.Key}={additionalProperty.Value} ");
                }

                builder.AppendLine("></script>");
            }

            builder.Append($"    {BundlingConsts.ScriptPlaceholderEnd}");

            return builder.ToString();
        }

        private IBundleContributor CreateContributorInstance(Type bundleContributorType)
        {
            return (IBundleContributor) Activator.CreateInstance(bundleContributorType);
        }

        private void FindBundleContributorsRecursively(
            Type module,
            int level,
            List<BundleTypeDefinition> bundleDefinitions)
        {
            var bundleContributors = module.Assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo<IBundleContributor>())
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

        private string GetTargetFrameworkVersion(string projectFilePath)
        {
            var document = new XmlDocument();
            document.Load(projectFilePath);
            CheckProjectIsSupportedType(document);

            return document.SelectSingleNode("//TargetFramework").InnerText;
        }

        private void CheckProjectIsSupportedType(string projectFilePath)
        {
            var document = new XmlDocument();
            document.Load(projectFilePath);
            CheckProjectIsSupportedType(document);
        }

        private void CheckProjectIsSupportedType(XmlDocument document)
        {
            var sdk = document.DocumentElement.GetAttribute("Sdk");
            if (sdk != BundlingConsts.SupportedWebAssemblyProjectType)
            {
                throw new BundlingException(
                    $"Unsupported project type. Project type must be {BundlingConsts.SupportedWebAssemblyProjectType}.");
            }
        }
    }
}