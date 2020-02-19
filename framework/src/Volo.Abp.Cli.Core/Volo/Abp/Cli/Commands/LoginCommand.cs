using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Text;
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
                throw new CliUsageException(
                    "Username name is missing!" +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo()
                );
            }

            var password = commandLineArgs.Options.GetOrNull(Options.Password.Short, Options.Password.Long);
            if (password == null)
            {
                Console.Write("Password: ");
                password = ConsoleHelper.ReadSecret();
                if (password.IsNullOrWhiteSpace())
                {
                    throw new CliUsageException(
                        "Password is missing!" +
                        Environment.NewLine + Environment.NewLine +
                        GetUsageInfo()
                    );
                }
            }

            await AuthService.LoginAsync(
                commandLineArgs.Target,
                password,
                commandLineArgs.Options.GetOrNull(Options.Organization.Short, Options.Organization.Long)
            );

            Logger.LogInformation($"Successfully logged in as '{commandLineArgs.Target}'");
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("  abp login <username>");
            sb.AppendLine("  abp login <username> -p <password>");
            sb.AppendLine("");
            sb.AppendLine("Example:");
            sb.AppendLine("");
            sb.AppendLine("  abp login john");
            sb.AppendLine("  abp login john -p 1234");
            sb.AppendLine("");
            sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Sign in to " + CliUrls.AccountAbpIo + ".";
        }

        public static class Options
        {
            public static class Organization
            {
                public const string Short = "o";
                public const string Long = "organization";
            }

            public static class Password
            {
                public const string Short = "p";
                public const string Long = "password";
            }
        }
    }
}