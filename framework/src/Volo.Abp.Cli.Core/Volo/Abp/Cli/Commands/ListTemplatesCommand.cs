using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class ListTemplatesCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "list-templates";
    
    public ListTemplatesCommand()
    {
        
    }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("  abp list-templates");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

        return sb.ToString();
    }

    public string GetShortDescription()
    {
        return "Lists available templates to be created.";
    }
}
