using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class ListModulesCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "list-modules";
    
    public ModuleInfoProvider ModuleInfoProvider { get; }
    public ILogger<ListModulesCommand> Logger { get; set; }


    public ListModulesCommand(ModuleInfoProvider moduleInfoProvider)
    {
        ModuleInfoProvider = moduleInfoProvider;
        Logger = NullLogger<ListModulesCommand>.Instance;
    }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var modules = await ModuleInfoProvider.GetModuleListAsync();
        var freeModules = modules.Where(m => !m.IsPro).ToList();
        var proModules = modules.Where(m => m.IsPro).ToList();

        var output = new StringBuilder(Environment.NewLine);
        output.AppendLine("Open Source Application Modules");
        output.AppendLine();

        foreach (var module in freeModules)
        {
            output.AppendLine($"> {module.DisplayName.PadRight(50)} ({module.Name})");
        }

        if (commandLineArgs.Options.ContainsKey("include-pro-modules"))
        {
            output.AppendLine();
            output.AppendLine("Commercial (Pro) Application Modules");
            output.AppendLine();
            foreach (var module in proModules)
            {
                output.AppendLine($"> {module.DisplayName.PadRight(50)} ({module.Name})");
            }
        }

        Logger.LogInformation(output.ToString());
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("'list-modules' command is used for listing open source application modules.");
        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("  abp list-modules");
        sb.AppendLine("  abp list-modules --include-pro-modules");
        sb.AppendLine("");
        sb.AppendLine("Options:");
        sb.AppendLine("  --include-pro-modules                                           Includes commercial (pro) modules in the output.");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

        return sb.ToString();
    }

    public string GetShortDescription()
    {
        return "List open source application modules";
    }

}
