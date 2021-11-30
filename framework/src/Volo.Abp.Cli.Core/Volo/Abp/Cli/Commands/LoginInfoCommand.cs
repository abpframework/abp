using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Auth;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class LoginInfoCommand : IConsoleCommand, ITransientDependency
{
    public ILogger<LoginInfoCommand> Logger { get; set; }

    protected AuthService AuthService { get; }

    public LoginInfoCommand(AuthService authService)
    {
        AuthService = authService;
        Logger = NullLogger<LoginInfoCommand>.Instance;
    }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var loginInfo = await AuthService.GetLoginInfoAsync();

        if (loginInfo == null)
        {
            Logger.LogError("Unable to get login info.");
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine("");
        sb.AppendLine($"Login info:");
        sb.AppendLine($"Name: {loginInfo.Name}");
        sb.AppendLine($"Surname: {loginInfo.Surname}");
        sb.AppendLine($"Username: {loginInfo.Username}");
        sb.AppendLine($"Email Address: {loginInfo.EmailAddress}");
        sb.AppendLine($"Organization: {loginInfo.Organization}");
        Logger.LogInformation(sb.ToString());
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("  abp login-info");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

        return sb.ToString();
    }

    public string GetShortDescription()
    {
        return "Show your login info.";
    }
}
