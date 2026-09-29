using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class SwitchToNightlyCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "switch-to-nightly";
    
    private readonly PackagePreviewSwitcher _packagePreviewSwitcher;

    public SwitchToNightlyCommand(PackagePreviewSwitcher packagePreviewSwitcher)
    {
        _packagePreviewSwitcher = packagePreviewSwitcher;
    }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        await _packagePreviewSwitcher.SwitchToNightlyPreview(commandLineArgs);
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("  abp switch-to-nightly [options]");
        sb.AppendLine("");
        sb.AppendLine("Options:");
        sb.AppendLine("-d|--directory");
        sb.AppendLine("-i|--include                 (optional) comma-separated list of Directory.Packages.props-style files to also update for Central Package Management");
        sb.AppendLine("-ep|--exclude-packages       (optional) comma-separated list of package ids to never touch in --include files");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://abp.io/docs/latest/cli");

        return sb.ToString();
    }

    public static string GetShortDescription()
    {
        return "Switches packages to nightly preview ABP version.";
    }
}
