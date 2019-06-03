using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class HelpCommand : IConsoleCommand, ITransientDependency
    {
        public ILogger<HelpCommand> Logger { get; set; }
        protected CliOptions CliOptions { get; }

        public HelpCommand(IOptions<CliOptions> cliOptions)
        {
            Logger = NullLogger<HelpCommand>.Instance;
            CliOptions = cliOptions.Value;
        }

        public Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            Logger.LogInformation("");
            Logger.LogInformation("Usage:");
            Logger.LogInformation("");
            Logger.LogInformation("    abp <command> <target> [options]");
            Logger.LogInformation("");
            Logger.LogInformation("Command List:");

            foreach (var commandKey in CliOptions.Commands.Keys.ToArray())
            {
                Logger.LogInformation("    " + commandKey);
            }

            Logger.LogInformation("");

            return Task.CompletedTask;
        }
    }
}