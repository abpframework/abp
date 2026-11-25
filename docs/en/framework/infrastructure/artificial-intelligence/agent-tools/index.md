```json
//[doc-seo]
{
    "Description": "Turn the application services and controllers of an ABP application into governed AI tools for chat clients and MCP agents with the open source ABP Agent Tools packages."
}
```

# Agent Tools

Agent Tools turns the application services and controllers of an ABP application into governed AI tools. Each tool is one HTTP API action of your application, discovered from ABP's API description model and called in-process through its application service interface. Tools are handed out as [Microsoft.Extensions.AI](../microsoft-extensions-ai.md) `AIFunction`s, so any `IChatClient` can use them, and they can be served to external agents (such as Claude Code) over the [Model Context Protocol (MCP)](https://modelcontextprotocol.io/).

Nothing is exposed unless the application opts in.

## What a Tool Call Guarantees

Every tool call runs through ABP's own infrastructure for the application service:

- **Permissions**: The call runs as the current user. ABP's authorization checks the action's permissions, and the tool list leaves out the tools whose declared permissions and features the user does not have.
- **Tenant**: The call runs in the current tenant. The MCP endpoint takes the tenant from the access token and refuses requests that name another tenant.
- **Unit of work**: Each call runs in its own transactional unit of work. A failure rolls the call back.
- **Audit**: The call is audit-logged (when an audit log scope is active, as in HTTP requests) with a comment that names the tool and the client. Registered observers receive one record per call.
- **Policy**: A per-tool policy decides whether a tool is enabled and whether it requires human approval. You set it in `appsettings.json` or in code.

By default only read (`GET`) tools run without approval. Every write requires approval, and MCP has no approval step, so the MCP endpoint is read-only until you change the policy of a write tool.

MVC filters and endpoint policies of the HTTP pipeline are not applied to tool calls. See [Security Notes and Known Limits](./security.md) for the details and the other limits.

## Packages

| Package | Module class | Purpose |
| --- | --- | --- |
| `Volo.Abp.AgentTools.Abstractions` | `AbpAgentToolsAbstractionsModule` | `[AgentTool]`, `[DisableAgentTool]`, `AgentToolKind`, the feature name, error codes and their localization. No ASP.NET Core dependency. |
| `Volo.Abp.AgentTools` | `AbpAgentToolsModule` | `AbpAgentToolOptions`, the tool abstractions (`IAgentToolProvider`, `IAgentToolInvoker`, `IAgentToolCatalog`, `IAgentToolCallObserver`) and the [tool policy](./policy.md). Depends on `Microsoft.Extensions.AI.Abstractions`, not on ASP.NET Core MVC. |
| `Volo.Abp.AgentTools.AspNetCore` | `AbpAgentToolsAspNetCoreModule` | The runtime: discovery from the API description model, the invoker and the provider. |
| `Volo.Abp.AgentTools.Mcp` | `AbpAgentToolsMcpModule` | The [MCP endpoint](./mcp.md) for external agents. Includes the runtime. |
| `Volo.Abp.OpenIddict.AgentTools` | `AbpOpenIddictAgentToolsModule` | [OpenIddict configuration](./openiddict.md) for the MCP endpoint: scope, resource, client seeding and a startup check. |

The types are in the namespaces `Volo.Abp.AgentTools`, `Volo.Abp.AgentTools.Mcp` and `Volo.Abp.OpenIddict.AgentTools`.

| Project of a layered solution | Package | When |
| --- | --- | --- |
| `{ProjectName}.Application.Contracts` | `Volo.Abp.AgentTools.Abstractions` | You use the attributes on application service interfaces. |
| The host that runs the application services (`{ProjectName}.HttpApi.Host`, `{ProjectName}.Web` or the single application) | `Volo.Abp.AgentTools.AspNetCore` | Your own chat client uses the tools. |
| The same host | `Volo.Abp.AgentTools.Mcp` | External agents use the tools over MCP. |
| The host that runs the OpenIddict server (the auth server, or the single application), and the `DbMigrator` | `Volo.Abp.OpenIddict.AgentTools` | The MCP endpoint's tokens come from the application's OpenIddict server. |
| A class library that only consumes tools | `Volo.Abp.AgentTools` | It needs the abstractions without MVC. |

Tools are called in-process, so the runtime must run in the same host as the application services it exposes.

## First Steps

Install the runtime into the host that runs your application services. Open a command line window in the directory of its project (`.csproj`) file and type the following command of the [ABP CLI](../../../../cli/index.md):

```bash
abp add-package Volo.Abp.AgentTools.AspNetCore
```

Depend on the module and choose what to expose:

```csharp
[DependsOn(
    //...
    typeof(AbpAgentToolsAspNetCoreModule)
)]
public class MyProjectNameHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAgentToolOptions>(options =>
        {
            options.IncludedServiceTypes.Add(type => type == typeof(IBookAppService));
        });
    }
}
```

With nothing included the catalog is empty. See [Exposing Tools](./exposing-tools.md) for the other ways to include and exclude services.

Give the current user's tools to a chat client:

```csharp
var tools = await _toolProvider.GetToolsAsync(new AgentToolContext
{
    SupportsApproval = false // no approval UI: tools that require approval are left out
});

var options = new ChatOptions
{
    Tools = tools.Select(tool => (AITool)tool.Function).ToList()
};
```

`_toolProvider` is an injected `IAgentToolProvider`. See [Using Tools from Your Own Chat Client](./chat-clients.md) for a complete sample.

Disable a tool, or change whether it requires approval, in `appsettings.json`:

```json
{
  "AgentTools": {
    "Tools": {
      "Book_Delete": { "Enabled": false },
      "Book_Create": { "RequiresApproval": false }
    }
  }
}
```

To serve the same tools to external agents, use `Volo.Abp.AgentTools.Mcp` instead of `Volo.Abp.AgentTools.AspNetCore` and follow [MCP Endpoint](./mcp.md) and [OpenIddict Setup](./openiddict.md).

## Feature

The `AgentTools.Enable` [feature](../../features.md) (a toggle in the `AgentTools` group, `true` by default; the constant is `AgentToolsFeatures.Enable`) switches the tools off per tenant or edition: the tool list is empty, and every call is refused with the error code `AgentTools:FeatureDisabled`. Without the [Feature Management module](../../../../modules/feature-management.md) the default applies.

## Documentation

- [Exposing Tools](./exposing-tools.md): which actions become tools, naming, descriptions, attributes and input schemas.
- [Using Tools from Your Own Chat Client](./chat-clients.md): `IAgentToolProvider`, approvals and call observers.
- [Tool Policy](./policy.md): enabling, disabling and approval in `appsettings.json` or code.
- [MCP Endpoint](./mcp.md): serving the tools to external agents.
- [OpenIddict Setup](./openiddict.md): registering MCP clients such as Claude Code.
- [Security Notes and Known Limits](./security.md).

## Management Module

The [Agent Tools module (Pro)](../../../../modules/agent-tools/index.md) builds on these packages. It adds a tool policy per host and tenant that can be changed without a restart, a management page with an HTTP API, presets for MCP clients in the OpenIddict Pro UI and an integration with AI Management. The tool policy of this page stays the base of it.

## See Also

- [Artificial Intelligence Infrastructure](../index.md)
- [Microsoft.Extensions.AI Integration](../microsoft-extensions-ai.md)
- [OpenIddict Module](../../../../modules/openiddict.md)
