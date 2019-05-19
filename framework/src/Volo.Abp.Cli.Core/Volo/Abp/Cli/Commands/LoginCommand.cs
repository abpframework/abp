using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Auth;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class LoginCommand : IConsoleCommand, ITransientDependency
    {
        public ILogger<LoginCommand> Logger { get; set; }

        protected AuthService AuthService { get; }

        public LoginCommand(AuthService authService)
        {
            AuthService = authService;
            Logger = NullLogger<LoginCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Target.IsNullOrEmpty())
            {
                Logger.LogInformation("");
                Logger.LogWarning("Username is missing.");
                LogHelp();
                return;
            }

            Console.Write("Password: ");
            var password = ConsoleHelper.ReadSecret();
            if (password.IsNullOrWhiteSpace())
            {
                Logger.LogInformation("");
                Logger.LogWarning("Password is missing.");
                LogHelp();
                return;
            }

            await AuthService.LoginAsync(commandLineArgs.Target, password);

            Logger.LogInformation($"Successfully logged in as '{commandLineArgs.Target}'");
        }

        private void LogHelp()
        {
            Logger.LogWarning("");
            Logger.LogWarning("Usage:");
            Logger.LogWarning("  abp login <username>");
            Logger.LogWarning("");
            Logger.LogWarning("Example:");
            Logger.LogWarning("  abp login john");
        }
    }
}