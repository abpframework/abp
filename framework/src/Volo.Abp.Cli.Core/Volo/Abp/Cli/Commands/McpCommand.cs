using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Auth;
using Volo.Abp.Cli.Commands.Models;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.Licensing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal.Telemetry;
using Volo.Abp.Internal.Telemetry.Constants;

namespace Volo.Abp.Cli.Commands;

public class McpCommand : IConsoleCommand, ITransientDependency
{
    private const string LogSource = nameof(McpCommand);
    public const string Name = "mcp";
    
    private readonly AuthService _authService;
    private readonly IApiKeyService _apiKeyService;
    private readonly McpServerService _mcpServerService;
    private readonly McpHttpClientService _mcpHttpClient;
    private readonly IMcpLogger _mcpLogger;
    private readonly ITelemetryService _telemetryService;
    
    public ILogger<McpCommand> Logger { get; set; }

    public McpCommand(
        IApiKeyService apiKeyService,
        AuthService authService,
        McpServerService mcpServerService,
        McpHttpClientService mcpHttpClient,
        IMcpLogger mcpLogger,
        ITelemetryService telemetryService)
    {
        _apiKeyService = apiKeyService;
        _authService = authService;
        _mcpServerService = mcpServerService;
        _mcpHttpClient = mcpHttpClient;
        _mcpLogger = mcpLogger;
        _telemetryService = telemetryService;
        Logger = NullLogger<McpCommand>.Instance;
    }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        await ValidateLicenseAsync();

        var option = commandLineArgs.Target;

        if (!string.IsNullOrEmpty(option) && option.Equals("get-config", StringComparison.OrdinalIgnoreCase))
        {
            await PrintConfigurationAsync();
            return;
        }

        await using var _ = _telemetryService.TrackActivityAsync(ActivityNameConsts.AbpCliCommandsMcp);

        // Check server health before starting - fail if not reachable
        _mcpLogger.Info(LogSource, "Checking ABP.IO MCP Server connection...");
        var isHealthy = await _mcpHttpClient.CheckServerHealthAsync();
        
        if (!isHealthy)
        {
            throw new CliUsageException(
                "Could not connect to ABP.IO MCP Server. " +
                "The MCP server requires a connection to fetch tool definitions. " +
                "Please check your internet connection and try again.");
        }

        _mcpLogger.Info(LogSource, "Starting ABP MCP Server...");
        
        var cts = new CancellationTokenSource();
        
        ConsoleCancelEventHandler cancelHandler = (sender, e) =>
        {
            e.Cancel = true;
            _mcpLogger.Info(LogSource, "Shutting down ABP MCP Server...");
            
            try
            {
                cts.Cancel();
            }
            catch (ObjectDisposedException)
            {
                // CTS already disposed
            }
        };
        
        Console.CancelKeyPress += cancelHandler;

        try
        {
            await _mcpServerService.RunAsync(cts.Token);
        }
        catch (OperationCanceledException)
        {
            // Expected when Ctrl+C is pressed
        }
        catch (Exception ex)
        {
            _mcpLogger.Error(LogSource, "Error running MCP server", ex);
            throw;
        }
        finally
        {
            Console.CancelKeyPress -= cancelHandler;
            cts.Dispose();
        }
    }

    private async Task ValidateLicenseAsync()
    {
        var loginInfo = await _authService.GetLoginInfoAsync();

        if (string.IsNullOrEmpty(loginInfo?.Organization))
        {
            throw new CliUsageException("Please log in with your account!");
        }
        
        var licenseResult = await _apiKeyService.GetApiKeyOrNullAsync();

        if (licenseResult == null || !licenseResult.HasActiveLicense)
        {
            var errorMessage = licenseResult?.ErrorMessage ?? "No active license found.";
            throw new CliUsageException(errorMessage);
        }

        if (licenseResult.LicenseEndTime.HasValue && licenseResult.LicenseEndTime.Value < DateTime.UtcNow)
        {
            throw new CliUsageException("Your license has expired. Please renew your license to use the MCP server.");
        }
    }

    private Task PrintConfigurationAsync()
    {
        var config = new McpClientConfiguration
        {
            McpServers = new Dictionary<string, McpServerConfig>
            {
                ["abp"] = new McpServerConfig
                {
                    Command = "abp",
                    Args = new List<string> { "mcp" },
                    Env = new Dictionary<string, string>()
                }
            }
        };

        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        Console.WriteLine(json);
        
        return Task.CompletedTask;
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
        sb.AppendLine("get-config                             (print MCP client configuration as JSON)");
        sb.AppendLine("");
        sb.AppendLine("Examples:");
        sb.AppendLine("");
        sb.AppendLine("  abp mcp");
        sb.AppendLine("  abp mcp get-config");
        sb.AppendLine("");

        return sb.ToString();
    }

    public static string GetShortDescription()
    {
        return "Runs the local MCP server and outputs client configuration for AI tool integration.";
    }
}