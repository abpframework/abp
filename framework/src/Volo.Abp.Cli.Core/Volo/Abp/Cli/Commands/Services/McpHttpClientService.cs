using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Cli.Http;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Services;

public class McpHttpClientService : ITransientDependency
{
    private readonly CliHttpClientFactory _httpClientFactory;
    private readonly ILogger<McpHttpClientService> _logger;
    
    private const string DefaultMcpServerUrl = "https://mcp.abp.io";
    private const string LocalMcpServerUrl = "http://localhost:5100";

    public McpHttpClientService(
        CliHttpClientFactory httpClientFactory,
        ILogger<McpHttpClientService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<string> CallToolAsync(string toolName, JsonElement arguments, bool useLocalServer = false)
    {
        var baseUrl = LocalMcpServerUrl;//useLocalServer ? LocalMcpServerUrl : DefaultMcpServerUrl;
        var url = $"{baseUrl}/tools/call";

        try
        {
            using var httpClient = _httpClientFactory.CreateClient(needsAuthentication: true);

            var requestBody = new
            {
                name = toolName,
                arguments = arguments
            };

            var jsonContent = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // Log to stderr to avoid corrupting stdout
                await Console.Error.WriteLineAsync($"[MCP] API call failed: {response.StatusCode} - {errorContent}");
                
                return JsonSerializer.Serialize(new
                {
                    content = new[]
                    {
                        new
                        {
                            type = "text",
                            text = $"Error: {response.StatusCode} - {errorContent}"
                        }
                    }
                });
            }

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            // Log to stderr to avoid corrupting stdout
            await Console.Error.WriteLineAsync($"[MCP] Error calling MCP tool '{toolName}': {ex.Message}");
            
            return JsonSerializer.Serialize(new
            {
                content = new[]
                {
                    new
                    {
                        type = "text",
                        text = $"Error: {ex.Message}"
                    }
                }
            });
        }
    }

    public async Task<bool> CheckServerHealthAsync(bool useLocalServer = false)
    {
        var baseUrl = useLocalServer ? LocalMcpServerUrl : DefaultMcpServerUrl;

        try
        {
            using var httpClient = _httpClientFactory.CreateClient(needsAuthentication: false);
            var response = await httpClient.GetAsync(baseUrl);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            // Silently fail health check - it's optional
            return false;
        }
    }
}

