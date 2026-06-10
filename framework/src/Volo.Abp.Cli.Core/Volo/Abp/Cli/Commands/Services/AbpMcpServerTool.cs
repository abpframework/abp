using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Volo.Abp.Cli.Commands.Services;

internal class AbpMcpServerTool : McpServerTool
{
    private readonly string _name;
    private readonly string _description;
    private readonly JsonElement _inputSchema;
    private readonly JsonElement? _outputSchema;
    private readonly Func<RequestContext<CallToolRequestParams>, CancellationToken, ValueTask<CallToolResult>> _handler;

    public AbpMcpServerTool(
        string name,
        string description,
        JsonElement inputSchema,
        JsonElement? outputSchema,
        Func<RequestContext<CallToolRequestParams>, CancellationToken, ValueTask<CallToolResult>> handler)
    {
        _name = name;
        _description = description;
        _inputSchema = inputSchema;
        _outputSchema = outputSchema;
        _handler = handler;
    }

    public override Tool ProtocolTool => new Tool
    {
        Name = _name,
        Description = _description,
        InputSchema = _inputSchema,
        OutputSchema = _outputSchema
    };

    public override IReadOnlyList<object> Metadata => Array.Empty<object>();

    public override ValueTask<CallToolResult> InvokeAsync(RequestContext<CallToolRequestParams> context, CancellationToken cancellationToken)
    {
        return _handler(context, cancellationToken);
    }
}
