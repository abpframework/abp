```json
//[doc-seo]
{
    "Description": "Turn the application services and controllers of an ABP application into governed AI tools for chat clients and MCP agents with the ABP Agent Tools module."
}
```

# Agent Tools Module (Pro)

> You must have an [ABP Team or a higher license](https://abp.io/pricing) to use this module.

The Agent Tools module turns the application services and controllers of an ABP application into governed AI tools. Each tool is one HTTP API action of your application, discovered from ABP's API description model and called in-process through its application service interface. Tools are handed out as [Microsoft.Extensions.AI](../../framework/infrastructure/artificial-intelligence/microsoft-extensions-ai.md) `AIFunction`s, so any `IChatClient` can use them, and they can be served to external agents (such as Claude Code) over the [Model Context Protocol (MCP)](https://modelcontextprotocol.io/).

Nothing is exposed unless the application opts in.

## What a Tool Call Guarantees

Every tool call runs through the same ABP infrastructure as an HTTP request to the action:

- **Permissions**: The call runs as the current user. ABP's authorization checks the action's permissions, and the tool list only offers tools the user may call.
- **Tenant**: The call runs in the current tenant. The MCP endpoint takes the tenant from the access token and refuses requests that name another tenant.
- **Unit of work**: Each call runs in its own transactional unit of work. A failure rolls the call back.
- **Audit**: The call is audit-logged with a comment that names the tool and the client. Registered observers receive one record per call.
- **Policy**: A per-tool policy decides whether a tool is enabled and whether it requires human approval. Host administrators set defaults, and each tenant can override them.

By default only read (`GET`) tools run without approval. Every write requires approval, and MCP has no approval step, so the MCP endpoint is read-only until an administrator changes the policy of a write tool.

## Packages

| Package | Layer | Purpose |
| --- | --- | --- |
| `Volo.AgentTools.Domain.Shared` | Domain.Shared | `[AgentTool]`, `[DisableAgentTool]`, `AgentToolKind`, the feature, error codes and localization. |
| `Volo.AgentTools.Domain` | Domain | `AbpAgentToolOptions`, the tool abstractions (`IAgentToolProvider`, `IAgentToolInvoker`, `IAgentToolCatalog`, `IAgentToolCallObserver`) and the policy store. Depends on `Microsoft.Extensions.AI.Abstractions`, not on ASP.NET Core MVC. |
| `Volo.AgentTools.AspNetCore` | Host | The runtime: discovery from the API description model, the invoker and the provider. |
| `Volo.AgentTools.Application.Contracts` | Application.Contracts | `IAgentToolPolicyAppService`, its DTOs and the permissions. |
| `Volo.AgentTools.Application` | Application | The policy application service. |
| `Volo.AgentTools.HttpApi` | HttpApi | The policy HTTP API. It depends on `Volo.AgentTools.AspNetCore`. |
| `Volo.AgentTools.HttpApi.Client` | HttpApi.Client | Dynamic C# client proxies for the policy HTTP API. |
| `Volo.AgentTools.EntityFrameworkCore` | EntityFrameworkCore | The policy table and repository. |
| `Volo.AgentTools.MongoDB` | MongoDB | The policy collection and repository. |
| `Volo.AgentTools.Web` | Web (MVC) | The **Agent tools** management page. |
| `Volo.AgentTools.Mcp` | Host | The MCP endpoint for external agents. |
| `Volo.AgentTools.OpenIddict` | Auth server | OpenIddict configuration for the MCP endpoint: scope, resource, client seeding and a startup check. It keeps the MCP [resource permission](./openiddict.md#the-resource-permission) in step with the `AgentTools` scope, so a web host needs the MCP URL (`App:SelfUrl` or `McpResource`) or, with the sync on (the default), fails at startup. |

A package that only consumes tools (a chat UI, a policy store) can depend on the Domain layer without MVC. Discovery and invocation need the MVC application model, so they are in `Volo.AgentTools.AspNetCore`.

## Relation to AI Management

Agent Tools is a separate module. It does not need [AI Management](../ai-management/index.md): you can use the tools from your own `IChatClient` or serve them over MCP only.

The `Volo.AIManagement.AgentTools` package connects the two modules. It adds the signed-in user's tools to AI Management chat for the workspaces you opt in, and it governs the MCP tools attached to AI Management workspaces. See [AI Management Integration](./ai-management.md).

## User Interface

The module provides an MVC / Razor Pages management page under **Administration** > **Agent tools**. Blazor and Angular UIs are not available yet; the policy HTTP API can be used from any client.

## Documentation

- [Installation](./installation.md): packages per layer, module dependencies and the database.
- [Exposing Tools](./exposing-tools.md): which actions become tools, naming, descriptions, attributes and input schemas.
- [Tool Policy and Management Page](./policy.md): defaults, host and tenant overrides, permissions, the feature, the page and the HTTP API.
- [Using Tools from Your Own Chat Client](./chat-clients.md): `IAgentToolProvider`, approvals and call observers.
- [MCP Endpoint](./mcp.md): serving the tools to external agents.
- [OpenIddict Setup](./openiddict.md): registering MCP clients such as Claude Code.
- [AI Management Integration](./ai-management.md): the bridge package.
- [Security Notes and Known Limits](./security.md).

## See Also

- [Artificial Intelligence Infrastructure](../../framework/infrastructure/artificial-intelligence/index.md)
- [AI Management Module](../ai-management/index.md)
- [OpenIddict Module (Pro)](../openiddict-pro.md)
