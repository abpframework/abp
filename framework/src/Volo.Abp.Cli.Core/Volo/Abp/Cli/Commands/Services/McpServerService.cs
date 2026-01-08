using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Volo.Abp.Cli.Commands.Models;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Services;

public class McpServerService : ITransientDependency
{
    private const string LogSource = nameof(McpServerService);
    
    private static class ToolErrorMessages
    {
        public const string InvalidResponseFormat = "The tool execution completed but returned an invalid response format. Please try again.";
        public const string UnexpectedError = "The tool execution failed due to an unexpected error. Please try again later.";
    }
    
    private readonly McpHttpClientService _mcpHttpClient;
    private readonly McpToolsCacheService _toolsCacheService;
    private readonly IMcpLogger _mcpLogger;

    public McpServerService(
        McpHttpClientService mcpHttpClient,
        McpToolsCacheService toolsCacheService,
        IMcpLogger mcpLogger)
    {
        _mcpHttpClient = mcpHttpClient;
        _toolsCacheService = toolsCacheService;
        _mcpLogger = mcpLogger;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        _mcpLogger.Info(LogSource, "Starting ABP MCP Server (stdio)");

        var options = new McpServerOptions();

        await RegisterAllToolsAsync(options);

        // Use NullLoggerFactory to prevent ModelContextProtocol library from logging to stdout
        // All our logging goes to file and stderr via IMcpLogger
        var server = McpServer.Create(
            new StdioServerTransport("abp-mcp-server", NullLoggerFactory.Instance),
            options
        );

        await server.RunAsync(cancellationToken);

        _mcpLogger.Info(LogSource, "ABP MCP Server stopped");
    }

    private async Task RegisterAllToolsAsync(McpServerOptions options)
    {
        // Get tool definitions from cache (or fetch from server)
        var toolDefinitions = await _toolsCacheService.GetToolDefinitionsAsync();

        _mcpLogger.Info(LogSource, $"Registering {toolDefinitions.Count} tools");

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

    private static CallToolResult CreateErrorResult(string errorMessage)
    {
        return new CallToolResult
        {
            Content = new List<ContentBlock>
            {
                new TextContentBlock
                {
                    Text = errorMessage
                }
            },
            IsError = true
        };
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
            (context, cancellationToken) => HandleToolInvocationAsync(name, context, cancellationToken)
        );

        options.ToolCollection.Add(tool);
    }

    private async ValueTask<CallToolResult> HandleToolInvocationAsync(
        string toolName,
        RequestContext<CallToolRequestParams> context,
        CancellationToken cancellationToken)
    {
        _mcpLogger.Debug(LogSource, $"Tool '{toolName}' called with arguments: {context.Params.Arguments}");

        try
        {
            var argumentsJson = JsonSerializer.SerializeToElement(context.Params.Arguments);
            var resultJson = await _mcpHttpClient.CallToolAsync(toolName, argumentsJson);

            var callToolResult = TryDeserializeResult(resultJson, toolName);
            if (callToolResult != null)
            {
                LogToolResult(toolName, callToolResult);
                return callToolResult;
            }

            return CreateErrorResult(ToolErrorMessages.InvalidResponseFormat);
        }
        catch (Exception ex)
        {
            _mcpLogger.Error(LogSource, $"Tool '{toolName}' execution failed", ex);
            return CreateErrorResult(ToolErrorMessages.UnexpectedError);
        }
    }

    private CallToolResult TryDeserializeResult(string resultJson, string toolName)
    {
        try
        {
            return JsonSerializer.Deserialize<CallToolResult>(resultJson);
        }
        catch (Exception ex)
        {
            _mcpLogger.Error(LogSource, $"Failed to deserialize response as CallToolResult: {ex.Message}");
            _mcpLogger.Debug(LogSource, $"Response was: {resultJson.Substring(0, Math.Min(500, resultJson.Length))}");
            return null;
        }
    }

    private void LogToolResult(string toolName, CallToolResult result)
    {
        if (result.IsError == true)
        {
            _mcpLogger.Warning(LogSource, $"Tool '{toolName}' returned an error");
        }
        else
        {
            _mcpLogger.Debug(LogSource, $"Tool '{toolName}' executed successfully");
        }
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
