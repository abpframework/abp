using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class GenerateRazorPage : IConsoleCommand, ITransientDependency
{
    public const string Name = "generate-razor-page";

    public ILogger<GenerateRazorPage> Logger { get; set; }

    public GenerateRazorPage()
    {
        Logger = NullLogger<GenerateRazorPage>.Instance;
    }

    public Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var targetProjectDirectory = Directory.GetCurrentDirectory();
        var projectEngine = CreateProjectEngine(targetProjectDirectory);

        var results = MainCore(projectEngine, targetProjectDirectory);

        foreach (var result in results)
        {
            File.WriteAllText(result.FilePath, result.GeneratedCode);
        }

        Logger.LogInformation($"{results.Count} files successfully generated.");

        return Task.CompletedTask;
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("abp generate-razor-page");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://abp.io/docs/latest/cli");

        return sb.ToString();
    }

    public string GetShortDescription()
    {
        return "Generates code files for Razor page.";
    }

    private RazorProjectEngine CreateProjectEngine(string targetProjectDirectory, Action<RazorProjectEngineBuilder>? configure = null)
    {
        var fileSystem = RazorProjectFileSystem.Create(targetProjectDirectory);
        var projectEngine = RazorProjectEngine.Create(RazorConfiguration.Default, fileSystem, builder =>
        {
            builder
                .SetNamespace("Volo.Abp.AspNetCore.RazorViews")
                .ConfigureClass((document, @class) =>
                {
                    @class.ClassName = Path.GetFileNameWithoutExtension(document.Source.FilePath);
                    @class.Modifiers.Clear();
                    @class.Modifiers.Add("internal");
                });

            SectionDirective.Register(builder);

            builder.Features.Add(new SuppressChecksumOptionsFeature());
            builder.Features.Add(new SuppressMetadataAttributesFeature());

            if (configure != null)
            {
                configure(builder);
            }

            builder.AddDefaultImports(@"
@using System
@using System.Threading.Tasks");
        });

        return projectEngine;
    }

    private List<RazorPageGeneratorResult> MainCore(RazorProjectEngine projectEngine, string targetProjectDirectory)
    {
        var results = new List<RazorPageGeneratorResult>();
        Logger.LogInformation("Generating code files for pages in {0}", targetProjectDirectory);

        var cshtmlFiles = projectEngine.FileSystem.EnumerateItems(targetProjectDirectory)
            .Where(x => File.ReadAllText(x.PhysicalPath).Contains("@inherits AbpCompilationRazorPageBase"))
            .ToList();

        if (!cshtmlFiles.Any())
        {
            Logger.LogInformation("No .cshtml or .razor files were found.");
            return results;
        }

        foreach (var item in cshtmlFiles)
        {
            Logger.LogInformation("  Generating code file for page {0} ...", item.FileName);

            results.Add(GenerateCodeFile(projectEngine, item));

            Logger.LogInformation("    Done!");
        }

        return results;
    }

    private RazorPageGeneratorResult GenerateCodeFile(RazorProjectEngine projectEngine, RazorProjectItem projectItem)
    {
        var projectItemWrapper = new FileSystemRazorProjectItemWrapper(Logger, projectItem);
        var codeDocument = projectEngine.Process(projectItemWrapper);
        var cSharpDocument = codeDocument.GetCSharpDocument();
        if (cSharpDocument.Diagnostics.Any())
        {
            var diagnostics = string.Join(Environment.NewLine, cSharpDocument.Diagnostics);
            Logger.LogInformation($"One or more parse errors encountered. This will not prevent the generator from continuing: {Environment.NewLine}{diagnostics}.");
        }

        var generatedCodeFilePath = Path.ChangeExtension(projectItem.PhysicalPath, ".Designer.cs");
        return new RazorPageGeneratorResult
        {
            FilePath = generatedCodeFilePath,
            GeneratedCode = cSharpDocument.GeneratedCode,
        };
    }

    private class SuppressChecksumOptionsFeature : RazorEngineFeatureBase, IConfigureRazorCodeGenerationOptionsFeature
    {
        public int Order { get; set; }

        public void Configure(RazorCodeGenerationOptionsBuilder options)
        {
           Check.NotNull(options, nameof(options));

            options.SuppressChecksum = true;
        }
    }

    private class SuppressMetadataAttributesFeature : RazorEngineFeatureBase, IConfigureRazorCodeGenerationOptionsFeature
    {
        public int Order { get; set; }

        public void Configure(RazorCodeGenerationOptionsBuilder options)
        {
            Check.NotNull(options, nameof(options));
            options.SuppressMetadataAttributes = true;
        }
    }

    private class FileSystemRazorProjectItemWrapper : RazorProjectItem
    {
        private readonly ILogger<GenerateRazorPage> _logger;
        private readonly RazorProjectItem _source;

        public FileSystemRazorProjectItemWrapper(ILogger<GenerateRazorPage> logger, RazorProjectItem item)
        {
            _logger = logger;
            _source = item;

            // Mask the full name since we don't want a developer's local file paths to be committed.
            PhysicalPath = $"{_source.FileName}";
        }

        public override string BasePath => _source.BasePath;

        public override string FilePath => _source.FilePath;

        public override string PhysicalPath { get; }

        public override bool Exists => _source.Exists;

        public override Stream Read()
        {
            var processedContent = ProcessFileIncludes();
            return new MemoryStream(Encoding.UTF8.GetBytes(processedContent));
        }

        private string ProcessFileIncludes()
        {
            var basePath = Path.GetDirectoryName(_source.PhysicalPath);
            var cshtmlContent = File.ReadAllText(_source.PhysicalPath);

            var startMatch = "<%$ include: ";
            var endMatch = " %>";
            var startIndex = 0;
            while (startIndex < cshtmlContent.Length)
            {
                startIndex = cshtmlContent.IndexOf(startMatch, startIndex, StringComparison.Ordinal);
                if (startIndex == -1)
                {
                    break;
                }
                var endIndex = cshtmlContent.IndexOf(endMatch, startIndex, StringComparison.Ordinal);
                if (endIndex == -1)
                {
                    throw new InvalidOperationException($"Invalid include file format in {_source.PhysicalPath}. Usage example: <%$ include: ErrorPage.js %>");
                }
                var includeFileName = cshtmlContent.Substring(startIndex + startMatch.Length, endIndex - (startIndex + startMatch.Length));
                _logger.LogInformation("    Inlining file {0}", includeFileName);
                var includeFileContent = File.ReadAllText(Path.Combine(basePath, includeFileName));
                cshtmlContent = string.Concat(cshtmlContent.Substring(0, startIndex), includeFileContent, cshtmlContent.Substring(endIndex + endMatch.Length));
                startIndex += includeFileContent.Length;
            }
            return cshtmlContent;
        }
    }

    private class RazorPageGeneratorResult
    {
        public string FilePath { get; set; }

        public string GeneratedCode { get; set; }
    }
}
