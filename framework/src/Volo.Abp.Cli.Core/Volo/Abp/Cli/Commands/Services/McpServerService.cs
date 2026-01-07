using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Volo.Abp.Cli.Commands.Models;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Services;

public class McpServerService : ITransientDependency
{
    private readonly McpHttpClientService _mcpHttpClient;
    private readonly McpToolsCacheService _toolsCacheService;

    public McpServerService(
        McpHttpClientService mcpHttpClient,
        McpToolsCacheService toolsCacheService)
    {
        _mcpHttpClient = mcpHttpClient;
        _toolsCacheService = toolsCacheService;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        // Log to stderr to avoid corrupting stdout JSON-RPC stream
        await Console.Error.WriteLineAsync("[MCP] Starting ABP MCP Server (stdio)");

        var options = new McpServerOptions();

        await RegisterAllToolsAsync(options);

        // Use NullLoggerFactory to prevent ModelContextProtocol library from logging to stdout
        // All our logging goes to stderr via Console.Error
        var server = McpServer.Create(
            new StdioServerTransport("abp-mcp-server", NullLoggerFactory.Instance),
            options
        );

        await server.RunAsync(cancellationToken);

        await Console.Error.WriteLineAsync("[MCP] ABP MCP Server stopped");
    }

    private async Task RegisterAllToolsAsync(McpServerOptions options)
    {
        // Get tool definitions from cache (or fetch from server)
        var toolDefinitions = await _toolsCacheService.GetToolDefinitionsAsync();

        await Console.Error.WriteLineAsync($"[MCP] Registering {toolDefinitions.Count} tools");

        // Register each tool dynamically
        foreach (var toolDef in toolDefinitions)
        {
            RegisterToolFromDefinition(options, toolDef);
        }
    }

    private void RegisterToolFromDefinition(McpServerOptions options, McpToolDefinition toolDef)
    {
        // Convert McpToolDefinition to the input schema format expected by MCP
        var inputSchemaObject = new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = ConvertProperties(toolDef.InputSchema?.Properties),
            ["required"] = toolDef.InputSchema?.Required ?? new List<string>()
        };

        RegisterTool(options, toolDef.Name, toolDef.Description, inputSchemaObject);
    }

    private Dictionary<string, object> ConvertProperties(Dictionary<string, McpToolProperty> properties)
    {
        if (properties == null)
        {
            return new Dictionary<string, object>();
        }

        return properties.ToDictionary(
            kvp => kvp.Key,
            kvp => (object)new Dictionary<string, object>
            {
                ["type"] = kvp.Value.Type,
                ["description"] = kvp.Value.Description
            }
        );
    }

    private void RegisterTool(
        McpServerOptions options,
        string name,
        string description,
        object inputSchema)
    {
        if (options.ToolCollection == null)
        {
            options.ToolCollection = new McpServerPrimitiveCollection<McpServerTool>();
        }

        var tool = new AbpMcpServerTool(
            name,
            description,
            JsonSerializer.SerializeToElement(inputSchema),
            async (context, cancellationToken) =>
            {
                // Log to stderr to avoid corrupting stdout JSON-RPC stream
                await Console.Error.WriteLineAsync($"[MCP] Tool '{name}' called with arguments: {context.Params.Arguments}");

                try
                {
                    var argumentsDict = context.Params.Arguments;
                    var argumentsJson = JsonSerializer.SerializeToElement(argumentsDict);
                    var resultJson = await _mcpHttpClient.CallToolAsync(
                        name,
                        argumentsJson
                    );

                    await Console.Error.WriteLineAsync($"[MCP] Tool '{name}' executed successfully");

                    // Try to deserialize the response as CallToolResult
                    // The HTTP client should return JSON in the format expected by MCP
                    try
                    {
                        var callToolResult = JsonSerializer.Deserialize<CallToolResult>(resultJson);
                        if (callToolResult != null)
                        {
                            return callToolResult;
                        }
                    }
                    catch (Exception deserializeEx)
                    {
                        await Console.Error.WriteLineAsync($"[MCP] Failed to deserialize response as CallToolResult: {deserializeEx.Message}");
                        await Console.Error.WriteLineAsync($"[MCP] Response was: {resultJson.Substring(0, Math.Min(500, resultJson.Length))}");
                    }

                    // Fallback: return empty result if deserialization fails
                    return new CallToolResult
                    {
                        Content = new List<ContentBlock>()
                    };
                }
                catch (Exception ex)
                {
                    await Console.Error.WriteLineAsync($"[MCP] Tool '{name}' execution failed: {ex.Message}");
                    return new CallToolResult
                    {
                        Content = new List<ContentBlock>(),
                        IsError = true
                    };
                }
            }
        );

        options.ToolCollection.Add(tool);
    }

    private class AbpMcpServerTool : McpServerTool
    {
        private readonly string _name;
        private readonly string _description;
        private readonly JsonElement _inputSchema;
        private readonly Func<RequestContext<CallToolRequestParams>, CancellationToken, ValueTask<CallToolResult>> _handler;

        public AbpMcpServerTool(
            string name,
            string description,
            JsonElement inputSchema,
            Func<RequestContext<CallToolRequestParams>, CancellationToken, ValueTask<CallToolResult>> handler)
        {
            _name = name;
            _description = description;
            _inputSchema = inputSchema;
            _handler = handler;
        }

        public override Tool ProtocolTool => new Tool
        {
            Name = _name,
            Description = _description,
            InputSchema = _inputSchema
        };

        public override IReadOnlyList<object> Metadata => Array.Empty<object>();

        public override ValueTask<CallToolResult> InvokeAsync(RequestContext<CallToolRequestParams> context, CancellationToken cancellationToken)
        {
            return _handler(context, cancellationToken);
        }
    }

}
