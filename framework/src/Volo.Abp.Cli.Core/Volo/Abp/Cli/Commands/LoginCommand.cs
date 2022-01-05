using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Auth;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.Commands;

public class LoginCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "login";
    
    public ILogger<LoginCommand> Logger { get; set; }

    protected AuthService AuthService { get; }
    public ICancellationTokenProvider CancellationTokenProvider { get; }
    public IRemoteServiceExceptionHandler RemoteServiceExceptionHandler { get; }

    private readonly CliHttpClientFactory _cliHttpClientFactory;

    public LoginCommand(AuthService authService,
        ICancellationTokenProvider cancellationTokenProvider,
        IRemoteServiceExceptionHandler remoteServiceExceptionHandler,
        CliHttpClientFactory cliHttpClientFactory)
    {
        AuthService = authService;
        CancellationTokenProvider = cancellationTokenProvider;
        RemoteServiceExceptionHandler = remoteServiceExceptionHandler;
        _cliHttpClientFactory = cliHttpClientFactory;
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

        var organization = commandLineArgs.Options.GetOrNull(Options.Organization.Short, Options.Organization.Long);

        if (await HasMultipleOrganizationAndThisNotSpecified(commandLineArgs, organization))
        {
            return;
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

        try
        {
            await AuthService.LoginAsync(
                commandLineArgs.Target,
                password,
                organization
            );
        }
        catch (Exception ex)
        {
            LogCliError(ex, commandLineArgs);
            return;
        }

        Logger.LogInformation($"Successfully logged in as '{commandLineArgs.Target}'");
    }

    private async Task<bool> HasMultipleOrganizationAndThisNotSpecified(CommandLineArgs commandLineArgs, string organization)
    {
        if (string.IsNullOrWhiteSpace(organization) &&
            await CheckMultipleOrganizationsAsync(commandLineArgs.Target))
        {
            Logger.LogError($"You have multiple organizations, please specify your organization with `--organization` parameter.");
            return true;
        }

        return false;
    }

    private void LogCliError(Exception ex, CommandLineArgs args)
    {
        if (ex.Message.Contains("Invalid username or password"))
        {
            Logger.LogError("Invalid username or password!");
            return;
        }

        if (TryGetErrorMessageFromHtmlPage(ex.Message, out var errorMsg))
        {
            Logger.LogError(errorMsg);
            return;
        }

        Logger.LogError(ex.Message);
    }

    private static bool TryGetErrorMessageFromHtmlPage(string htmlPage, out string errorMessage)
    {
        if (!htmlPage.Contains("error-page-container"))
        {
            errorMessage = null;
            return false;
        }

        var decodedHtml = HttpUtility.HtmlDecode(htmlPage);

        var error = Regex.Match(decodedHtml,
            @"<h2\ class=""text-danger.*"">(.*?)</h2>",
            RegexOptions.IgnoreCase |
            RegexOptions.IgnorePatternWhitespace |
            RegexOptions.Singleline |
            RegexOptions.Multiline);

        if (error.Success && error.Groups.Count > 1)
        {
            errorMessage = error.Groups[1].Value;
            errorMessage = errorMessage
                .Replace("<eof/>", string.Empty)
                .Replace("</small>", string.Empty);

            return true;
        }

        errorMessage = null;
        return false;
    }

    private async Task<bool> CheckMultipleOrganizationsAsync(string username)
    {
        var url = $"{CliUrls.WwwAbpIo}api/license/check-multiple-organizations?username={username}";

        var client = _cliHttpClientFactory.CreateClient();

        using (var response = await client.GetHttpResponseMessageWithRetryAsync(url, CancellationTokenProvider.Token, Logger))
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"ERROR: Remote server returns '{response.StatusCode}'");
            }

            await RemoteServiceExceptionHandler.EnsureSuccessfulHttpResponseAsync(response);

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<bool>(responseContent);
        }
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
