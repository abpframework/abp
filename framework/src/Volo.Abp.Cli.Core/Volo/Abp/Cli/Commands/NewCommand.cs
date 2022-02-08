using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class NewCommand : ProjectCreationCommandBase, IConsoleCommand, ITransientDependency
{
    public const string Name = "new";

    public ILogger<NewCommand> Logger { get; set; }

    protected TemplateProjectBuilder TemplateProjectBuilder { get; }
    public ITemplateInfoProvider TemplateInfoProvider { get; }

    public NewCommand(TemplateProjectBuilder templateProjectBuilder
        , ITemplateInfoProvider templateInfoProvider,
        ConnectionStringProvider connectionStringProvider,
        SolutionPackageVersionFinder solutionPackageVersionFinder,
        ICmdHelper cmdHelper)
    : base(connectionStringProvider, solutionPackageVersionFinder, cmdHelper)
    {
        TemplateProjectBuilder = templateProjectBuilder;
        TemplateInfoProvider = templateInfoProvider;

        Logger = NullLogger<NewCommand>.Instance;
    }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var projectName = NamespaceHelper.NormalizeNamespace(commandLineArgs.Target);
        if (string.IsNullOrWhiteSpace(projectName))
        {
            throw new CliUsageException("Project name is missing!" + Environment.NewLine + Environment.NewLine + GetUsageInfo());
        }

        ProjectNameValidator.Validate(projectName);

        Logger.LogInformation("Creating your project...");
        Logger.LogInformation("Project name: " + projectName);

        var template = commandLineArgs.Options.GetOrNull(Options.Template.Short, Options.Template.Long);
        if (template != null)
        {
            Logger.LogInformation("Template: " + template);
        }
        else
        {
            template = (await TemplateInfoProvider.GetDefaultAsync()).Name;
        }

        var isTiered = commandLineArgs.Options.ContainsKey(Options.Tiered.Long);
        if (isTiered)
        {
            Logger.LogInformation("Tiered: yes");
        }

        var projectArgs = GetProjectBuildArgs(commandLineArgs, template, projectName);

        var result = await TemplateProjectBuilder.BuildAsync(
            projectArgs
        );

        ExtractProjectZip(result, projectArgs.OutputFolder);

        Logger.LogInformation($"'{projectName}' has been successfully created to '{projectArgs.OutputFolder}'");

        RunGraphBuildForMicroserviceServiceTemplate(projectArgs);
        RunInstallLibsForAppTemplate(projectArgs);
        OpenRelatedWebPage(projectArgs, template, isTiered, commandLineArgs);
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("");
        sb.AppendLine("  abp new <project-name> [options]");
        sb.AppendLine("");
        sb.AppendLine("Options:");
        sb.AppendLine("");
        sb.AppendLine("-t|--template <template-name>               (default: app)");
        sb.AppendLine("-u|--ui <ui-framework>                      (if supported by the template)");
        sb.AppendLine("-m|--mobile <mobile-framework>              (if supported by the template)");
        sb.AppendLine("-d|--database-provider <database-provider>  (if supported by the template)");
        sb.AppendLine("-o|--output-folder <output-folder>          (default: current folder)");
        sb.AppendLine("-v|--version <version>                      (default: latest version)");
        sb.AppendLine("--preview                                   (Use latest pre-release version if there is at least one pre-release after latest stable version)");
        sb.AppendLine("-ts|--template-source <template-source>     (your local or network abp template source)");
        sb.AppendLine("-csf|--create-solution-folder               (default: true)");
        sb.AppendLine("-cs|--connection-string <connection-string> (your database connection string)");
        sb.AppendLine("--dbms <database-management-system>         (your database management system)");
        sb.AppendLine("--tiered                                    (if supported by the template)");
        sb.AppendLine("--no-ui                                     (if supported by the template)");
        sb.AppendLine("--no-random-port                            (Use template's default ports)");
        sb.AppendLine("--separate-identity-server                  (if supported by the template)");
        sb.AppendLine("--local-framework-ref --abp-path <your-local-abp-repo-path>  (keeps local references to projects instead of replacing with NuGet package references)");
        sb.AppendLine("");
        sb.AppendLine("Examples:");
        sb.AppendLine("");
        sb.AppendLine("  abp new Acme.BookStore");
        sb.AppendLine("  abp new Acme.BookStore --tiered");
        sb.AppendLine("  abp new Acme.BookStore -u angular");
        sb.AppendLine("  abp new Acme.BookStore -u angular -d mongodb");
        sb.AppendLine("  abp new Acme.BookStore -m none");
        sb.AppendLine("  abp new Acme.BookStore -m react-native");
        sb.AppendLine("  abp new Acme.BookStore -d mongodb");
        sb.AppendLine("  abp new Acme.BookStore -d mongodb -o d:\\my-project");
        sb.AppendLine("  abp new Acme.BookStore -t module");
        sb.AppendLine("  abp new Acme.BookStore -t module --no-ui");
        sb.AppendLine("  abp new Acme.BookStore -t console");
        sb.AppendLine("  abp new Acme.BookStore -ts \"D:\\localTemplate\\abp\"");
        sb.AppendLine("  abp new Acme.BookStore -csf false");
        sb.AppendLine("  abp new Acme.BookStore --local-framework-ref --abp-path \"D:\\github\\abp\"");
        sb.AppendLine("  abp new Acme.BookStore --dbms mysql");
        sb.AppendLine("  abp new Acme.BookStore --connection-string \"Server=myServerName\\myInstanceName;Database=myDatabase;User Id=myUsername;Password=myPassword\"");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

        return sb.ToString();
    }

    public string GetShortDescription()
    {
        return "Generate a new solution based on the ABP startup templates.";
    }

}
