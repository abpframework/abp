using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class HelpCommand : IConsoleCommand, ITransientDependency
    {
        public ILogger<HelpCommand> Logger { get; set; }

        public HelpCommand()
        {
            Logger = NullLogger<HelpCommand>.Instance;
        }

        public Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            Logger.LogInformation("");
            Logger.LogInformation("Usage:");
            Logger.LogInformation("");
            Logger.LogInformation("  abp <command> <target> [options]");
            Logger.LogInformation("");

            return Task.CompletedTask;
        }
    }
}