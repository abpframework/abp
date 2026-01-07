using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Cli.Commands.Models;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.Memory;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Services;

public class McpHttpClientService : ITransientDependency
{
    private readonly CliHttpClientFactory _httpClientFactory;
    private readonly ILogger<McpHttpClientService> _logger;
    private readonly MemoryService _memoryService;
    private string _cachedServerUrl;

    public McpHttpClientService(
        CliHttpClientFactory httpClientFactory,
        ILogger<McpHttpClientService> logger,
        MemoryService memoryService)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _memoryService = memoryService;
    }

    private async Task<string> GetMcpServerUrlAsync()
    {
        // Return cached URL if already resolved
        if (_cachedServerUrl != null)
        {
            return _cachedServerUrl;
        }

        // 1. Check environment variable (highest priority)
        var envUrl = Environment.GetEnvironmentVariable(CliConsts.McpServerUrlEnvironmentVariable);
        if (!string.IsNullOrWhiteSpace(envUrl))
        {
            _cachedServerUrl = envUrl.TrimEnd('/');
            return _cachedServerUrl;
        }

        // 2. Check persisted setting
        var persistedUrl = await _memoryService.GetAsync(CliConsts.MemoryKeys.McpServerUrl);
        if (!string.IsNullOrWhiteSpace(persistedUrl))
        {
            _cachedServerUrl = persistedUrl.TrimEnd('/');
            return _cachedServerUrl;
        }

        // 3. Return default
        _cachedServerUrl = CliConsts.DefaultMcpServerUrl;
        return _cachedServerUrl;
    }

    public async Task<string> CallToolAsync(string toolName, JsonElement arguments)
    {
        var baseUrl = "http://localhost:5100";//await GetMcpServerUrlAsync();
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
                // Log to stderr to avoid corrupting stdout - sanitize error message
                await Console.Error.WriteLineAsync($"[MCP] API call failed with status: {response.StatusCode}");
                
                return JsonSerializer.Serialize(new
                {
                    content = new[]
                    {
                        new
                        {
                            type = "text",
                            text = $"Error: API call failed with status {response.StatusCode}"
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

    public async Task<bool> CheckServerHealthAsync()
    {
        var baseUrl = "http://localhost:5100";//await GetMcpServerUrlAsync();

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

    public async Task<List<McpToolDefinition>> GetToolDefinitionsAsync()
    {
        var baseUrl = "http://localhost:5100";//await GetMcpServerUrlAsync();
        var url = $"{baseUrl}/tools";

        try
        {
            using var httpClient = _httpClientFactory.CreateClient(needsAuthentication: true);
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                // Sanitize error message - don't expose server details
                await Console.Error.WriteLineAsync($"[MCP] Failed to fetch tool definitions with status: {response.StatusCode}");
                throw new Exception($"Failed to fetch tool definitions with status: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            
            // The API returns { tools: [...] } format
            var result = JsonSerializer.Deserialize<McpToolsResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Tools ?? new List<McpToolDefinition>();
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"[MCP] Error fetching tool definitions: {ex.Message}");
            throw;
        }
    }

    private class McpToolsResponse
    {
        public List<McpToolDefinition> Tools { get; set; }
    }
}

