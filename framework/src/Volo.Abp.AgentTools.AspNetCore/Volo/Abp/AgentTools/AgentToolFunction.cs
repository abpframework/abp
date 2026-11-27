using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;

namespace Volo.Abp.AgentTools;

/// <summary>
/// One agent tool as an MEAI function. Invoking it goes through <see cref="IAgentToolInvoker"/> (unit of work, ABP's
/// authorization/validation, error strings, observers) with the context the tool list was built for.
/// </summary>
public class AgentToolFunction : AIFunction
{
    private readonly IAgentToolInvoker _invoker;

    public AgentToolDefinition Definition { get; }

    public AgentToolContext Context { get; }

    public override string Name => Definition.Name;

    public override string Description => Definition.Description;

    public override JsonElement JsonSchema => AgentToolJsonSchema.Get(Definition);

    /// <summary>MCP-style annotations: readOnlyHint, destructiveHint, idempotentHint.</summary>
    public override IReadOnlyDictionary<string, object?> AdditionalProperties { get; }

    public override MethodInfo UnderlyingMethod => Definition.InterfaceMethod;

    /// <summary>
    /// <paramref name="requiresApproval"/> is the approval flag the tool is listed with; the invoker refuses the call when the
    /// policy requires approval by then but this was false.
    /// </summary>
    public AgentToolFunction(AgentToolDefinition definition, IAgentToolInvoker invoker, AgentToolContext context, bool requiresApproval)
    {
        Definition = definition;
        Context = new AgentToolContext
        {
            SupportsApproval = context.SupportsApproval,
            ClientId = context.ClientId,
            ClientName = context.ClientName,
            ListedRequiresApproval = requiresApproval
        };
        _invoker = invoker;
        AdditionalProperties = new Dictionary<string, object?>
        {
            ["readOnlyHint"] = definition.ReadOnly,
            ["destructiveHint"] = definition.Destructive,
            ["idempotentHint"] = definition.Idempotent
        };
    }

    protected override async ValueTask<object?> InvokeCoreAsync(AIFunctionArguments arguments, CancellationToken cancellationToken)
    {
        return await _invoker.InvokeAsync(Name, arguments, Context, cancellationToken);
    }
}
