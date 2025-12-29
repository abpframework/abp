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
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class McpCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "mcp";
    
    private readonly AuthService _authService;
    private readonly AbpNuGetIndexUrlService _nuGetIndexUrlService;
    private readonly McpServerService _mcpServerService;
    private readonly McpHttpClientService _mcpHttpClient;
    
    public ILogger<McpCommand> Logger { get; set; }

    public McpCommand(
        AbpNuGetIndexUrlService nuGetIndexUrlService,
        AuthService authService,
        McpServerService mcpServerService,
        McpHttpClientService mcpHttpClient)
    {
        _nuGetIndexUrlService = nuGetIndexUrlService;
        _authService = authService;
        _mcpServerService = mcpServerService;
        _mcpHttpClient = mcpHttpClient;
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

        var option = commandLineArgs.Target;

        if (!string.IsNullOrEmpty(option) && option.Equals("getconfig", StringComparison.OrdinalIgnoreCase))
        {
            await PrintConfigurationAsync();
            return;
        }

        // Check server health before starting (log to stderr)
        await Console.Error.WriteLineAsync("[MCP] Checking ABP.IO MCP Server connection...");
        var isHealthy = await _mcpHttpClient.CheckServerHealthAsync();
        
        if (!isHealthy)
        {
            await Console.Error.WriteLineAsync("[MCP] Warning: Could not connect to ABP.IO MCP Server. The server might be offline.");
            await Console.Error.WriteLineAsync("[MCP] Continuing to start local MCP server...");
        }

        await Console.Error.WriteLineAsync("[MCP] Starting ABP MCP Server...");
        
        var cts = new CancellationTokenSource();
        ConsoleCancelEventHandler cancelHandler = null;
        
        cancelHandler = (sender, e) =>
        {
            e.Cancel = true;
            Console.Error.WriteLine("[MCP] Shutting down ABP MCP Server...");
            
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
            await Console.Error.WriteLineAsync($"[MCP] Error running MCP server: {ex.Message}");
            throw;
        }
        finally
        {
            Console.CancelKeyPress -= cancelHandler;
            cts.Dispose();
        }
    }

    private Task PrintConfigurationAsync()
    {
        var abpCliPath = GetAbpCliExecutablePath();
        
        var config = new McpClientConfiguration
        {
            McpServers = new Dictionary<string, McpServerConfig>
            {
                ["abp"] = new McpServerConfig
                {
                    Command = abpCliPath,
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

    private string GetAbpCliExecutablePath()
    {
        // Try to find the abp CLI executable
        try
        {
            using (var process = Process.GetCurrentProcess())
            {
                var processPath = process.MainModule?.FileName;
                
                if (!string.IsNullOrEmpty(processPath))
                {
                    // If running as a published executable
                    if (Path.GetFileName(processPath).StartsWith("abp", StringComparison.OrdinalIgnoreCase))
                    {
                        return processPath;
                    }
                }
            }
        }
        catch
        {
            // Ignore errors getting process path
        }

        // Check if abp is in PATH
        var pathEnv = Environment.GetEnvironmentVariable("PATH");
        if (!string.IsNullOrEmpty(pathEnv))
        {
            var paths = pathEnv.Split(Path.PathSeparator);
            foreach (var path in paths)
            {
                var abpPath = Path.Combine(path, "abp.exe");
                if (File.Exists(abpPath))
                {
                    return abpPath;
                }
                
                abpPath = Path.Combine(path, "abp");
                if (File.Exists(abpPath))
                {
                    return abpPath;
                }
            }
        }

        // Default to "abp" and let the system resolve it
        return "abp";
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