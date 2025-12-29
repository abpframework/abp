using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Auth;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class McpCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "mcp";
    
    private readonly AuthService _authService;
    private readonly AbpNuGetIndexUrlService _nuGetIndexUrlService;
    
    public ILogger<McpCommand> Logger { get; set; }

    public McpCommand(
        AbpNuGetIndexUrlService nuGetIndexUrlService,
        AuthService authService, ILogger<McpCommand> logger)
    {
        _nuGetIndexUrlService = nuGetIndexUrlService;
        _authService = authService;
        Logger = NullLogger<McpCommand>.Instance;
    }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var loginInfo = await _authService.GetLoginInfoAsync();

        if (string.IsNullOrEmpty(loginInfo?.Organization))
        {
            throw new CliUsageException("Please log in with your account!");
        }
        
        var nugetIndexUrl = await _nuGetIndexUrlService.GetAsync();
        
        if (nugetIndexUrl == null)
        {
            throw new CliUsageException("Could not find Nuget Index Url!");
        }
        
        Logger.LogInformation("Starting MCP server...");
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("");
        sb.AppendLine("  abp mcp [options]");
        sb.AppendLine("");
        sb.AppendLine("Options:");
        sb.AppendLine("");
        sb.AppendLine("<no argument>                          (start the local MCP server)");
        sb.AppendLine("getconfig                              (print MCP client configuration as JSON)");
        sb.AppendLine("");
        sb.AppendLine("Examples:");
        sb.AppendLine("");
        sb.AppendLine("  abp mcp");
        sb.AppendLine("  abp mcp getconfig");
        sb.AppendLine("");

        return sb.ToString();
    }

    public static string GetShortDescription()
    {
        return "Runs the local MCP server and outputs client configuration for AI tool integration.";
    }
}