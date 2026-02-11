using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Bundling;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class BundleCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "bundle";

    public ILogger<BundleCommand> Logger { get; set; }

    public IBundlingService BundlingService { get; set; }


    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var workingDirectoryArg = commandLineArgs.Options.GetOrNull(
            Options.WorkingDirectory.Short,
            Options.WorkingDirectory.Long
        );

        var workingDirectory = workingDirectoryArg ?? Directory.GetCurrentDirectory();

        var forceBuild = commandLineArgs.Options.ContainsKey(Options.ForceBuild.Short) ||
                         commandLineArgs.Options.ContainsKey(Options.ForceBuild.Long);

        var projectType = GetProjectType(commandLineArgs);

        if (!Directory.Exists(workingDirectory))
        {
            throw new CliUsageException(
                "Specified directory does not exist." +
                Environment.NewLine + Environment.NewLine +
                GetUsageInfo()
            );
        }

        await BundlingService.BundleAsync(workingDirectory, forceBuild, projectType);
    }

    public static string GetShortDescription()
    {
        return "Bundles all third party styles and scripts required by modules and updates index.html file.";
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("");
        sb.AppendLine("  abp bundle [options]");
        sb.AppendLine("");
        sb.AppendLine("Options:");
        sb.AppendLine("");
        sb.AppendLine("-wd|--working-directory <directory-path>                (default: empty)");
        sb.AppendLine("-f | --force                                            (default: false)");
        sb.AppendLine("-t | --project-type                                     (default: webassembly)");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://abp.io/docs/latest/cli");

        return sb.ToString();
    }

    private string GetProjectType(CommandLineArgs commandLineArgs)
    {
        var projectType = commandLineArgs.Options.GetOrNull(Options.ProjectType.Short, Options.ProjectType.Long);
        projectType ??= BundlingConsts.WebAssembly;

        return projectType.ToLower() switch {
            "webassembly" => BundlingConsts.WebAssembly,
            "maui-blazor" => BundlingConsts.MauiBlazor,
            _ => throw new CliUsageException(ExceptionMessageHelper.GetInvalidOptionExceptionMessage("Project Type"))
        };
    }

    public static class Options
    {
        public static class WorkingDirectory
        {
            public const string Short = "wd";
            public const string Long = "working-directory";
        }

        public static class ProjectType
        {
            public const string Short = "t";
            public const string Long = "project-type";
        }

        public static class ForceBuild
        {
            public const string Short = "f";
            public const string Long = "force";
        }
    }
}
