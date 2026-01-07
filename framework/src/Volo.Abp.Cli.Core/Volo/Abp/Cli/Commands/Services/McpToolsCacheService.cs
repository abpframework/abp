using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Cli.Commands.Models;
using Volo.Abp.Cli.Memory;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;

namespace Volo.Abp.Cli.Commands.Services;

public class McpToolsCacheService : ITransientDependency
{
    private readonly McpHttpClientService _mcpHttpClient;
    private readonly MemoryService _memoryService;
    private readonly ILogger<McpToolsCacheService> _logger;

    public McpToolsCacheService(
        McpHttpClientService mcpHttpClient,
        MemoryService memoryService,
        ILogger<McpToolsCacheService> logger)
    {
        _mcpHttpClient = mcpHttpClient;
        _memoryService = memoryService;
        _logger = logger;
    }

    public async Task<List<McpToolDefinition>> GetToolDefinitionsAsync()
    {
        if (await IsCacheValidAsync())
        {
            var cachedTools = await LoadFromCacheAsync();
            if (cachedTools != null)
            {
                await Console.Error.WriteLineAsync("[MCP] Using cached tool definitions");
                return cachedTools;
            }
        }

        // Cache is invalid or missing, fetch from server
        try
        {
            await Console.Error.WriteLineAsync("[MCP] Fetching tool definitions from server...");
            var tools = await _mcpHttpClient.GetToolDefinitionsAsync();
            
            // Save to cache
            await SaveToCacheAsync(tools);
            await _memoryService.SetAsync(CliConsts.MemoryKeys.McpToolsLastFetchDate, DateTime.Now.ToString(CultureInfo.InvariantCulture));
            
            await Console.Error.WriteLineAsync($"[MCP] Successfully fetched and cached {tools.Count} tool definitions");
            return tools;
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Failed to fetch tool definitions from server: {ex.Message}");
            await Console.Error.WriteLineAsync($"[MCP] Failed to fetch from server, attempting to use cached data...");
            
            // Fall back to cache even if expired
            var cachedTools = await LoadFromCacheAsync();
            if (cachedTools != null)
            {
                await Console.Error.WriteLineAsync("[MCP] Using expired cache as fallback");
                return cachedTools;
            }

            await Console.Error.WriteLineAsync("[MCP] No cached data available, using empty tool list");
            return new List<McpToolDefinition>();
        }
    }

    private async Task<bool> IsCacheValidAsync()
    {
        try
        {
            // Check if cache file exists
            if (!File.Exists(CliPaths.McpToolsCache))
            {
                return false;
            }

            // Check timestamp in memory
            var lastFetchTimeString = await _memoryService.GetAsync(CliConsts.MemoryKeys.McpToolsLastFetchDate);
            if (string.IsNullOrEmpty(lastFetchTimeString))
            {
                return false;
            }

            if (DateTime.TryParse(lastFetchTimeString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var lastFetchTime))
            {
                // Check if less than 24 hours old
                if (DateTime.Now.Subtract(lastFetchTime).TotalHours < 24)
                {
                    return true;
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error checking cache validity: {ex.Message}");
            return false;
        }
    }

    private async Task<List<McpToolDefinition>> LoadFromCacheAsync()
    {
        try
        {
            if (!File.Exists(CliPaths.McpToolsCache))
            {
                return null;
            }

            var json = await FileHelper.ReadAllTextAsync(CliPaths.McpToolsCache);
            var tools = JsonSerializer.Deserialize<List<McpToolDefinition>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return tools;
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error loading cached tool definitions: {ex.Message}");
            return null;
        }
    }

    private Task SaveToCacheAsync(List<McpToolDefinition> tools)
    {
        try
        {
            // Ensure directory exists
            var directory = Path.GetDirectoryName(CliPaths.McpToolsCache);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(tools, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            File.WriteAllText(CliPaths.McpToolsCache, json);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error saving tool definitions to cache: {ex.Message}");
        }

        return Task.CompletedTask;
    }
}

