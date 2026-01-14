using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Cli.Commands.Models;
using Volo.Abp.Cli.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;

namespace Volo.Abp.Cli.Commands.Services;

public class McpHttpClientService : ITransientDependency
{
    private static readonly JsonSerializerOptions JsonSerializerOptionsWeb = new(JsonSerializerDefaults.Web);
    
    private static class ErrorMessages
    {
        public const string NetworkConnectivity = "The tool execution failed due to a network connectivity issue. Please check your internet connection and try again.";
        public const string Timeout = "The tool execution timed out. The operation took too long to complete. Please try again.";
        public const string Unexpected = "The tool execution failed due to an unexpected error. Please try again later.";
    }
    
    private const string LogSource = nameof(McpHttpClientService);
    
    private readonly CliHttpClientFactory _httpClientFactory;
    private readonly ILogger<McpHttpClientService> _logger;
    private readonly IMcpLogger _mcpLogger;
    private readonly Lazy<Task<string>> _cachedServerUrlLazy;
    private List<string> _validToolNames;
    private bool _toolDefinitionsLoaded;

    public McpHttpClientService(
        CliHttpClientFactory httpClientFactory,
        ILogger<McpHttpClientService> logger,
        IMcpLogger mcpLogger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _mcpLogger = mcpLogger;
        _cachedServerUrlLazy = new Lazy<Task<string>>(GetMcpServerUrlInternalAsync);
    }

    private async Task<string> GetMcpServerUrlAsync()
    {
        return "http://localhost:5100";//await _cachedServerUrlLazy.Value;
    }

    private async Task<string> GetMcpServerUrlInternalAsync()
    {
        // Check config file
        if (File.Exists(CliPaths.McpConfig))
        {
            try
            {
                var json = await FileHelper.ReadAllTextAsync(CliPaths.McpConfig);
                var config = JsonSerializer.Deserialize<McpConfig>(json, JsonSerializerOptionsWeb);
                if (!string.IsNullOrWhiteSpace(config?.ServerUrl))
                {
                    return config.ServerUrl.TrimEnd('/');
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to read MCP config file");
            }
        }

        // Return default
        return CliConsts.DefaultMcpServerUrl;
    }

    private class McpConfig
    {
        public string ServerUrl { get; set; }
    }

    public async Task<string> CallToolAsync(string toolName, JsonElement arguments)
    {
        if (!_toolDefinitionsLoaded)
        {
            throw new CliUsageException("Tool definitions have not been loaded yet. This is an internal error.");
        }
        
        // Validate toolName against whitelist to prevent malicious input
        if (_validToolNames != null && !_validToolNames.Contains(toolName))
        {
            _mcpLogger.Warning(LogSource, $"Attempted to call unknown tool: {toolName}");
            return CreateErrorResponse($"Unknown tool: {toolName}");
        }

        var baseUrl = await GetMcpServerUrlAsync();
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
        var baseUrl = await GetMcpServerUrlAsync();

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
        var baseUrl = await GetMcpServerUrlAsync();
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
                throw new CliUsageException($"Failed to fetch tool definitions: {errorMessage}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            
            // The API returns { tools: [...] } format
            var result = JsonSerializer.Deserialize<McpToolsResponse>(responseContent, JsonSerializerOptionsWeb);
            var tools = result?.Tools ?? new List<McpToolDefinition>();
            
            // Cache tool names for validation
            _validToolNames = tools.Select(t => t.Name).ToList();
            _toolDefinitionsLoaded = true;
            
            return tools;
        }
        catch (HttpRequestException ex)
        {
            throw CreateHttpExceptionWithInner(ex, "Network error fetching tool definitions");
        }
        catch (TaskCanceledException ex)
        {
            throw CreateHttpExceptionWithInner(ex, "Timeout fetching tool definitions");
        }
        catch (JsonException ex)
        {
            throw CreateHttpExceptionWithInner(ex, "JSON parsing error");
        }
        catch (CliUsageException)
        {
            // Already sanitized, rethrow as-is
            throw;
        }
        catch (Exception ex)
        {
            throw CreateHttpExceptionWithInner(ex, "Unexpected error fetching tool definitions");
        }
    }

    private CliUsageException CreateHttpExceptionWithInner(Exception ex, string context)
    {
        _mcpLogger.Error(LogSource, context, ex);
        
        var userMessage = ex switch
        {
            HttpRequestException => "Network connectivity issue. Please check your internet connection and try again.",
            TaskCanceledException => "Request timed out. Please try again.",
            JsonException => "Invalid response format received.",
            _ => "An unexpected error occurred. Please try again later."
        };
        
        return new CliUsageException($"Failed to fetch tool definitions: {userMessage}", ex);
    }

    private class McpToolsResponse
    {
        public List<McpToolDefinition> Tools { get; set; }
    }
}

