﻿using Microsoft.Extensions.Logging;
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
                throw new CliUsageException("Username name is missing!" + Environment.NewLine + Environment.NewLine + await GetUsageInfo());
            }

            Console.Write("Password: ");
            var password = ConsoleHelper.ReadSecret();
            if (password.IsNullOrWhiteSpace())
            {
                throw new CliUsageException("Password name is missing!" + Environment.NewLine + Environment.NewLine + await GetUsageInfo());
            }

            await AuthService.LoginAsync(commandLineArgs.Target, password);

            Logger.LogInformation($"Successfully logged in as '{commandLineArgs.Target}'");
        }

        public Task<string> GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("  abp login <username>");
            sb.AppendLine("");
            sb.AppendLine("Example:");
            sb.AppendLine("  abp login john");

            return Task.FromResult(sb.ToString());
        }

        public Task<string> GetShortDescriptionAsync()
        {
            return Task.FromResult("");
        }
    }
}