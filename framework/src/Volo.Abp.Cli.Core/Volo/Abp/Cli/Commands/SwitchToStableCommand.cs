using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class SwitchToStableCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "switch-to-stable";
    
    private readonly PackagePreviewSwitcher _packagePreviewSwitcher;

    public SwitchToStableCommand(PackagePreviewSwitcher packagePreviewSwitcher)
    {
        _packagePreviewSwitcher = packagePreviewSwitcher;
    }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        await _packagePreviewSwitcher.SwitchToStable(commandLineArgs);
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("  abp switch-to-stable [options]");
        sb.AppendLine("");
        sb.AppendLine("Options:");
        sb.AppendLine("-sd|--solution-directory");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

        return sb.ToString();
    }

    public string GetShortDescription()
    {
        return "Switches packages to stable ABP version from preview version.";
    }
}
