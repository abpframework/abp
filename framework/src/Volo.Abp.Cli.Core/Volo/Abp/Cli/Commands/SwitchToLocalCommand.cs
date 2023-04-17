using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class SwitchToLocal : IConsoleCommand, ITransientDependency
{
    public const string Name = "switch-to-local";
    
    public ILogger<SwitchToLocal> Logger { get; set; }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var workingDirectory = GetWorkingDirectory(commandLineArgs) ?? Directory.GetCurrentDirectory();

        if (!Directory.Exists(workingDirectory))
        {
            throw new CliUsageException(
                "Specified directory does not exist." +
                Environment.NewLine + Environment.NewLine +
                GetUsageInfo()
            );
        }

        var paths = GetPaths(commandLineArgs);
        
    }

    private List<string> GetPaths(CommandLineArgs commandLineArgs)
    {
        var paths = commandLineArgs.Options.GetOrNull(
            Options.LocalPaths.Short,
            Options.LocalPaths.Long
        );

        if (paths == null)
        {
            throw new CliUsageException(
                "Local paths are not specified!" +
                Environment.NewLine + Environment.NewLine +
                GetUsageInfo()
            );
        }

        return paths.Split("|").Select(x=> x.Trim()).ToList();
    }

    private string GetWorkingDirectory(CommandLineArgs commandLineArgs)
    {
        var path = commandLineArgs.Options.GetOrNull(
            Options.SolutionPath.Short,
            Options.SolutionPath.Long
        );

        if (path == null)
        {
            return null;
        }

        if (path.EndsWith(".sln") || path.EndsWith(".csproj"))
        {
            return Path.GetDirectoryName(path);
        }

        return path;
    }

    public string GetShortDescription()
    {
        return "Bundles all third party styles and scripts required by modules and updates index.html file.";
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("");
        sb.AppendLine("  abp switch-to-local [options]");
        sb.AppendLine("");
        sb.AppendLine("Options:");
        sb.AppendLine("");
        sb.AppendLine("-s|--solution <directory-path>                (default: current directory)");
        sb.AppendLine("-p | --paths <local-paths>                    (Required)");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

        return sb.ToString();
    }

    public static class Options
    {
        public static class SolutionPath
        {
            public const string Short = "s";
            public const string Long = "solution";
        }

        public static class LocalPaths
        {
            public const string Short = "p";
            public const string Long = "paths";
        }
    }
}
