using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Services;

public class McpServerService : ITransientDependency
{
    private readonly McpHttpClientService _mcpHttpClient;
    private readonly ILogger<McpServerService> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public McpServerService(
        McpHttpClientService mcpHttpClient,
        ILogger<McpServerService> logger,
        ILoggerFactory loggerFactory)
    {
        _mcpHttpClient = mcpHttpClient;
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        // Log to stderr to avoid corrupting stdout JSON-RPC stream
        await Console.Error.WriteLineAsync("[MCP] Starting ABP MCP Server (stdio)");

        var options = new McpServerOptions();

        RegisterAllTools(options);

        var server = McpServer.Create(
            new StdioServerTransport("abp-mcp-server", _loggerFactory),
            options
        );

        await server.RunAsync(cancellationToken);

        await Console.Error.WriteLineAsync("[MCP] ABP MCP Server stopped");
    }

    private void RegisterAllTools(McpServerOptions options)
    {
        RegisterTool(
            options,
            "get_relevant_abp_documentation",
            "Search ABP framework technical documentation including official guides, API references, and framework documentation.",
            new
            {
                type = "object",
                properties = new
                {
                    query = new
                    {
                        type = "string",
                        description = "The search query to find relevant documentation"
                    }
                },
                required = new[] { "query" }
            }
        );

        RegisterTool(
            options,
            "get_relevant_abp_articles",
            "Search ABP blog posts, tutorials, and community-contributed content.",
            new
            {
                type = "object",
                properties = new
                {
                    query = new
                    {
                        type = "string",
                        description = "The search query to find relevant articles"
                    }
                },
                required = new[] { "query" }
            }
        );

        RegisterTool(
            options,
            "get_relevant_abp_support_questions",
            "Search support ticket history containing real-world problems and their solutions.",
            new
            {
                type = "object",
                properties = new
                {
                    query = new
                    {
                        type = "string",
                        description = "The search query to find relevant support questions"
                    }
                },
                required = new[] { "query" }
            }
        );

        RegisterTool(
            options,
            "search_code",
            "Search for code across ABP repositories using regex patterns.",
            new
            {
                type = "object",
                properties = new
                {
                    query = new
                    {
                        type = "string",
                        description = "The regex pattern or search query to find code"
                    },
                    repo_filter = new
                    {
                        type = "string",
                        description = "Optional repository filter to limit search scope"
                    }
                },
                required = new[] { "query" }
            }
        );

        RegisterTool(
            options,
            "list_repos",
            "List all available ABP repositories in SourceBot.",
            new
            {
                type = "object",
                properties = new { }
            }
        );

        RegisterTool(
            options,
            "get_file_source",
            "Retrieve the complete source code of a specific file from an ABP repository.",
            new
            {
                type = "object",
                properties = new
                {
                    repoId = new
                    {
                        type = "string",
                        description = "The repository identifier containing the file"
                    },
                    fileName = new
                    {
                        type = "string",
                        description = "The file path or name to retrieve"
                    }
                },
                required = new[] { "repoId", "fileName" }
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
