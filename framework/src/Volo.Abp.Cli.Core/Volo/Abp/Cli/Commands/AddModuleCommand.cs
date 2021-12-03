using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule;
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class AddModuleCommand : IConsoleCommand, ITransientDependency
{
    private AddModuleInfoOutput _lastAddedModuleInfo;
    public ILogger<AddModuleCommand> Logger { get; set; }

    protected SolutionModuleAdder SolutionModuleAdder { get; }
    public SolutionPackageVersionFinder SolutionPackageVersionFinder { get; }

    public AddModuleInfoOutput LastAddedModuleInfo {
        get {
            if (_lastAddedModuleInfo == null)
            {
                throw new Exception("You need to add a module first to get the last added module info!");
            }

            return _lastAddedModuleInfo;
        }
    }

    public AddModuleCommand(SolutionModuleAdder solutionModuleAdder, SolutionPackageVersionFinder solutionPackageVersionFinder)
    {
        SolutionModuleAdder = solutionModuleAdder;
        SolutionPackageVersionFinder = solutionPackageVersionFinder;
        Logger = NullLogger<AddModuleCommand>.Instance;
    }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        if (commandLineArgs.Target == null)
        {
            throw new CliUsageException(
                "Module name is missing!" +
                Environment.NewLine + Environment.NewLine +
                GetUsageInfo()
            );
        }

        var newTemplate = commandLineArgs.Options.ContainsKey(Options.NewTemplate.Long);
        var template = commandLineArgs.Options.GetOrNull(Options.Template.Short, Options.Template.Long);
        var newProTemplate = !string.IsNullOrEmpty(template) && template == ModuleProTemplate.TemplateName;
        var withSourceCode = newTemplate || newProTemplate || commandLineArgs.Options.ContainsKey(Options.SourceCode.Long);
        var addSourceCodeToSolutionFile = withSourceCode && commandLineArgs.Options.ContainsKey("add-to-solution-file");
        var skipDbMigrations = newTemplate || newProTemplate || commandLineArgs.Options.ContainsKey(Options.DbMigrations.Skip);
        var solutionFile = GetSolutionFile(commandLineArgs);

        var version = commandLineArgs.Options.GetOrNull(Options.Version.Short, Options.Version.Long);
        if (version == null)
        {
            version = SolutionPackageVersionFinder.Find(solutionFile);
        }

        var moduleInfo = await SolutionModuleAdder.AddAsync(
             solutionFile,
             commandLineArgs.Target,
             version,
             skipDbMigrations,
             withSourceCode,
             addSourceCodeToSolutionFile,
             newTemplate,
             newProTemplate
         );

        _lastAddedModuleInfo = new AddModuleInfoOutput
        {
            DisplayName = moduleInfo.DisplayName,
            Name = moduleInfo.Name,
            DocumentationLinks = moduleInfo.DocumentationLinks,
            InstallationCompleteMessage = moduleInfo.InstallationCompleteMessage
        };
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("'add-module' command is used to add a multi-package ABP module to a solution.");
        sb.AppendLine("It should be used in a folder containing a .sln file.");
        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("  abp add-module <module-name> [options]");
        sb.AppendLine("");
        sb.AppendLine("Options:");
        sb.AppendLine("  --new                                           Creates a fresh new module (speсialized for your solution) and adds it your solution.");
        sb.AppendLine("  --with-source-code                              Downloads the source code of the module to your solution folder. (Always True if `--new` is used.)");
        sb.AppendLine("  --add-to-solution-file                          Adds the downloaded/created module to your solution file. (only available when --with-source-code used)");
        sb.AppendLine("  -s|--solution <solution-file>                   Specify the solution file explicitly.");
        sb.AppendLine("  --skip-db-migrations <boolean>                  Specify if a new migration will be added or not.  (Always True if `--new` is used.)");
        sb.AppendLine("  -sp|--startup-project <startup-project-path>    Relative path to the project folder of the startup project. Default value is the current folder.");
        sb.AppendLine("  -v|--version <version>                          Specify the version of the module. Default is your project's ABP version.");
        sb.AppendLine("");
        sb.AppendLine("Examples:");
        sb.AppendLine("");
        sb.AppendLine("  abp add-module Volo.Blogging                      Adds the module to the current solution.");
        sb.AppendLine("  abp add-module Volo.Blogging -s Acme.BookStore    Adds the module to the given solution.");
        sb.AppendLine("  abp add-module Volo.Blogging -s Acme.BookStore --skip-db-migrations false    Adds the module to the given solution but doesn't create a database migration.");
        sb.AppendLine(@"  abp add-module Volo.Blogging -s Acme.BookStore -sp ..\Acme.BookStore.Web\Acme.BookStore.Web.csproj   Adds the module to the given solution and specify migration startup project.");
        sb.AppendLine(@"  abp add-module ProductManagement --new -sp ..\Acme.BookStore.Web\Acme.BookStore.Web.csproj   Crates a new module named `ProductManagement` and adds it to your solution.");
        sb.AppendLine(@"  abp add-module ProductManagement --new --add-to-solution-file -sp ..\Acme.BookStore.Web\Acme.BookStore.Web.csproj   Crates a new module named `ProductManagement`, adds it to your solution & solution file.");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

        return sb.ToString();
    }

    public string GetShortDescription()
    {
        return "Add a multi-package module to a solution by finding all packages of the module, " +
               "finding related projects in the solution and adding each package to the corresponding project in the solution.";
    }

    protected virtual string GetSolutionFile(CommandLineArgs commandLineArgs)
    {
        var providedSolutionFile = PathHelper.NormalizePath(
            commandLineArgs.Options.GetOrNull(
                Options.Solution.Short,
                Options.Solution.Long
            )
        );

        if (!providedSolutionFile.IsNullOrWhiteSpace())
        {
            return providedSolutionFile;
        }

        var foundSolutionFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sln");
        if (foundSolutionFiles.Length == 1)
        {
            return foundSolutionFiles[0];
        }

        if (foundSolutionFiles.Length == 0)
        {
            throw new CliUsageException("'abp add-module' command should be used inside a folder containing a .sln file!");
        }

        //foundSolutionFiles.Length > 1

        var sb = new StringBuilder("There are multiple solution (.sln) files in the current directory. Please specify one of the files below:");

        foreach (var foundSolutionFile in foundSolutionFiles)
        {
            sb.AppendLine("* " + foundSolutionFile);
        }

        sb.AppendLine("Example:");
        sb.AppendLine($"abp add-module {commandLineArgs.Target} -p {foundSolutionFiles[0]}");

        throw new CliUsageException(sb.ToString());
    }

    public static class Options
    {
        public static class Solution
        {
            public const string Short = "s";
            public const string Long = "solution";
        }
        public static class Version
        {
            public const string Short = "v";
            public const string Long = "version";
        }

        public static class DbMigrations
        {
            public const string Skip = "skip-db-migrations";
        }

        public static class SourceCode
        {
            public const string Long = "with-source-code";
        }

        public class NewTemplate
        {
            public const string Long = "new";
        }

        public class Template
        {
            public const string Short = "t";
            public const string Long = "template";
        }
    }
}
