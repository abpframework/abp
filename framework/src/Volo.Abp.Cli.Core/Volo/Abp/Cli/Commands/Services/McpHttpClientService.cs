using System;
using System.Collections.Generic;
using System.Net;
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
    private const string LogSource = nameof(McpHttpClientService);
    private static readonly JsonSerializerOptions JsonSerializerOptionsWeb = new(JsonSerializerDefaults.Web);
    
    private static class ErrorMessages
    {
        public const string NetworkConnectivity = "The tool execution failed due to a network connectivity issue. Please check your internet connection and try again.";
        public const string Timeout = "The tool execution timed out. The operation took too long to complete. Please try again.";
        public const string Unexpected = "The tool execution failed due to an unexpected error. Please try again later.";
    }
    
    private readonly CliHttpClientFactory _httpClientFactory;
    private readonly ILogger<McpHttpClientService> _logger;
    private readonly IMcpLogger _mcpLogger;
    private readonly MemoryService _memoryService;
    private string _cachedServerUrl;

    public McpHttpClientService(
        CliHttpClientFactory httpClientFactory,
        ILogger<McpHttpClientService> logger,
        IMcpLogger mcpLogger,
        MemoryService memoryService)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _mcpLogger = mcpLogger;
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

            var jsonContent = JsonSerializer.Serialize(
                new { name = toolName, arguments },
                JsonSerializerOptionsWeb);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                _mcpLogger.Error(LogSource, $"API call failed with status: {response.StatusCode}");
                
                // Return sanitized error message to client
                var errorMessage = GetSanitizedHttpErrorMessage(response.StatusCode);
                return CreateErrorResponse(errorMessage);
            }

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            _mcpLogger.Error(LogSource, $"Network error calling tool '{toolName}'", ex);
            
            // Return sanitized error to client
            return CreateErrorResponse(ErrorMessages.NetworkConnectivity);
        }
        catch (TaskCanceledException ex)
        {
            _mcpLogger.Error(LogSource, $"Timeout calling tool '{toolName}'", ex);
            
            // Return sanitized error to client
            return CreateErrorResponse(ErrorMessages.Timeout);
        }
        catch (Exception ex)
        {
            _mcpLogger.Error(LogSource, $"Unexpected error calling tool '{toolName}'", ex);
            
            // Return generic sanitized error to client
            return CreateErrorResponse(ErrorMessages.Unexpected);
        }
    }

    private string CreateErrorResponse(string errorMessage)
    {
        return JsonSerializer.Serialize(new
        {
            content = new[]
            {
                new
                {
                    type = "text",
                    text = errorMessage
                }
            },
            isError = true
        }, JsonSerializerOptionsWeb);
    }

    private Exception CreateToolDefinitionException(string userMessage)
    {
        return new Exception($"Failed to fetch tool definitions: {userMessage}");
    }

    private string GetSanitizedHttpErrorMessage(HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.Unauthorized => "Authentication failed. Please ensure you are logged in with a valid account.",
            HttpStatusCode.Forbidden => "Access denied. You do not have permission to use this tool.",
            HttpStatusCode.NotFound => "The requested tool could not be found. It may have been removed or is temporarily unavailable.",
            HttpStatusCode.BadRequest => "The tool request was invalid. Please check your input parameters and try again.",
            (HttpStatusCode)429 => "Rate limit exceeded. Please wait a moment before trying again.", // TooManyRequests not available in .NET Standard 2.0
            HttpStatusCode.ServiceUnavailable => "The service is temporarily unavailable. Please try again later.",
            HttpStatusCode.InternalServerError => "The tool execution encountered an internal error. Please try again later.",
            _ => "The tool execution failed. Please try again later."
        };
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
                _mcpLogger.Error(LogSource, $"Failed to fetch tool definitions with status: {response.StatusCode}");
                
                // Throw sanitized exception
                var errorMessage = GetSanitizedHttpErrorMessage(response.StatusCode);
                throw CreateToolDefinitionException(errorMessage);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            
            // The API returns { tools: [...] } format
            var result = JsonSerializer.Deserialize<McpToolsResponse>(responseContent, JsonSerializerOptionsWeb);

            return result?.Tools ?? new List<McpToolDefinition>();
        }
        catch (HttpRequestException ex)
        {
            _mcpLogger.Error(LogSource, "Network error fetching tool definitions", ex);
            
            // Throw sanitized exception
            throw CreateToolDefinitionException("Network connectivity issue. Please check your internet connection and try again.");
        }
        catch (TaskCanceledException ex)
        {
            _mcpLogger.Error(LogSource, "Timeout fetching tool definitions", ex);
            
            // Throw sanitized exception
            throw CreateToolDefinitionException("Request timed out. Please try again.");
        }
        catch (JsonException ex)
        {
            _mcpLogger.Error(LogSource, "JSON parsing error", ex);
            
            // Throw sanitized exception
            throw CreateToolDefinitionException("Invalid response format received.");
        }
        catch (Exception ex) when (ex.Message.StartsWith("Failed to fetch tool definitions:"))
        {
            // Already sanitized, rethrow as-is
            throw;
        }
        catch (Exception ex)
        {
            _mcpLogger.Error(LogSource, "Unexpected error fetching tool definitions", ex);
            
            // Throw sanitized exception
            throw CreateToolDefinitionException("An unexpected error occurred. Please try again later.");
        }
    }

    private class McpToolsResponse
    {
        public List<McpToolDefinition> Tools { get; set; }
    }
}

