using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Services;

public class McpServerService : ITransientDependency
{
    private readonly McpHttpClientService _mcpHttpClient;
    private readonly ILogger<McpServerService> _logger;

    public McpServerService(
        McpHttpClientService mcpHttpClient,
        ILogger<McpServerService> logger)
    {
        _mcpHttpClient = mcpHttpClient;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Write to stderr to avoid corrupting stdout JSON-RPC stream
            await Console.Error.WriteLineAsync("[MCP] ABP MCP Server started successfully");

            await ProcessStdioAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            await Console.Error.WriteLineAsync("[MCP] ABP MCP Server stopped");
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"[MCP] Error running ABP MCP Server: {ex.Message}");
            throw;
        }
    }

    private async Task ProcessStdioAsync(CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(Console.OpenStandardInput());
        using var writer = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true };

        while (!cancellationToken.IsCancellationRequested)
        {
            string line;
            try
            {
                var readTask = reader.ReadLineAsync();
                var completedTask = await Task.WhenAny(readTask, Task.Delay(Timeout.Infinite, cancellationToken));
                
                if (completedTask != readTask)
                {
                    // Cancellation requested
                    break;
                }
                
                line = await readTask;
            }
            catch (OperationCanceledException)
            {
                break;
            }
            
            if (line == null)
            {
                // EOF reached
                break;
            }

            // Skip empty lines or lines that are not JSON
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            
            // Check if line looks like JSON (starts with '{')
            if (!line.TrimStart().StartsWith("{"))
            {
                // Not JSON, probably build output or other noise - log to stderr and skip
                await Console.Error.WriteLineAsync($"[MCP] Skipping non-JSON line: {line.Substring(0, Math.Min(50, line.Length))}...");
                continue;
            }

            try
            {
                var request = JsonSerializer.Deserialize<McpRequest>(line, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                var response = await HandleRequestAsync(request, cancellationToken);
                
                var responseJson = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                
                await writer.WriteLineAsync(responseJson);
            }
            catch (JsonException jsonEx)
            {
                // JSON parse error - log to stderr but don't send error response
                // (the line might be build output or other noise)
                await Console.Error.WriteLineAsync($"[MCP] JSON parse error: {jsonEx.Message} | Line: {line.Substring(0, Math.Min(100, line.Length))}");
            }
            catch (Exception ex)
            {
                // Other errors during request handling - send error response to client
                await Console.Error.WriteLineAsync($"[MCP] Error processing request: {ex.Message}");
                
                var errorResponse = new McpResponse
                {
                    Jsonrpc = "2.0",
                    Id = null,
                    Error = new McpError
                    {
                        Code = -32603,
                        Message = ex.Message
                    }
                };
                
                var errorJson = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                
                await writer.WriteLineAsync(errorJson);
            }
        }
        
        await Console.Error.WriteLineAsync("[MCP] Stdio processing loop ended");
    }

    private async Task<McpResponse> HandleRequestAsync(McpRequest request, CancellationToken cancellationToken)
    {
        if (request.Method == "initialize")
        {
            return new McpResponse
            {
                Jsonrpc = "2.0",
                Id = request.Id,
                Result = new
                {
                    protocolVersion = "2024-11-05",
                    capabilities = new
                    {
                        tools = new { }
                    },
                    serverInfo = new
                    {
                        name = "abp-mcp-server",
                        version = "1.0.0"
                    }
                }
            };
        }

        if (request.Method == "tools/list")
        {
            return new McpResponse
            {
                Jsonrpc = "2.0",
                Id = request.Id,
                Result = new
                {
                    tools = GetToolDefinitions()
                }
            };
        }

        if (request.Method == "tools/call")
        {
            var toolName = request.Params.GetProperty("name").GetString();
            var arguments = request.Params.GetProperty("arguments");
            
            var result = await _mcpHttpClient.CallToolAsync(toolName, arguments);
            var toolResponse = JsonSerializer.Deserialize<JsonElement>(result);
            
            return new McpResponse
            {
                Jsonrpc = "2.0",
                Id = request.Id,
                Result = toolResponse
            };
        }

        return new McpResponse
        {
            Jsonrpc = "2.0",
            Id = request.Id,
            Error = new McpError
            {
                Code = -32601,
                Message = $"Method not found: {request.Method}"
            }
        };
    }

    private object[] GetToolDefinitions()
    {
        return new object[]
        {
            new
            {
                name = "get_relevant_abp_documentation",
                description = "Search ABP framework technical documentation including official guides, API references, and framework documentation.",
                inputSchema = new
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
            },
            new
            {
                name = "get_relevant_abp_articles",
                description = "Search ABP blog posts, tutorials, and community-contributed content.",
                inputSchema = new
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
            },
            new
            {
                name = "get_relevant_abp_support_questions",
                description = "Search support ticket history containing real-world problems and their solutions.",
                inputSchema = new
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
            },
            new
            {
                name = "search_code",
                description = "Search for code across ABP repositories using regex patterns.",
                inputSchema = new
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
            },
            new
            {
                name = "list_repos",
                description = "List all available ABP repositories in SourceBot.",
                inputSchema = new
                {
                    type = "object",
                    properties = new { }
                }
            },
            new
            {
                name = "get_file_source",
                description = "Retrieve the complete source code of a specific file from an ABP repository.",
                inputSchema = new
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
            }
        };
    }
}

internal class McpRequest
{
    [System.Text.Json.Serialization.JsonPropertyName("jsonrpc")]
    public string Jsonrpc { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public object Id { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("method")]
    public string Method { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("params")]
    public JsonElement Params { get; set; }
}

internal class McpResponse
{
    [System.Text.Json.Serialization.JsonPropertyName("jsonrpc")]
    public string Jsonrpc { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public object Id { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("result")]
    public object Result { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("error")]
    public McpError Error { get; set; }
}

internal class McpError
{
    [System.Text.Json.Serialization.JsonPropertyName("code")]
    public int Code { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("message")]
    public string Message { get; set; }
}

