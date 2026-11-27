```json
//[doc-seo]
{
    "Description": "Add the signed-in user's governed agent tools to AI Management chat and govern the MCP tools of AI Management workspaces with the Volo.AIManagement.AgentTools package."
}
```

# Agent Tools: AI Management Integration (Pro)

> You must have an [ABP Team or a higher license](https://abp.io/pricing) to use this module.

The `Volo.AIManagement.AgentTools` package connects [Agent Tools](../../framework/infrastructure/artificial-intelligence/agent-tools/index.md) with the [AI Management module](../ai-management/index.md). It does two things:

- It adds the signed-in user's agent tools to AI Management chat, for the workspaces you opt in. Each call is permission-checked, tenant-scoped, audited and subject to the [tool policy](../../framework/infrastructure/artificial-intelligence/agent-tools/policy.md), as everywhere else.
- It governs the MCP tools attached to AI Management workspaces: an optional permission per MCP server, and a [call record](../../framework/infrastructure/artificial-intelligence/agent-tools/chat-clients.md#observing-calls) for every call.

Without the package, AI Management behaves as before.

## Installation

Add the `Volo.AIManagement.AgentTools` package to the host that runs AI Management's chat, and depend on its module. The Agent Tools runtime (the open source package `Volo.Abp.AgentTools.AspNetCore`) must be in the same process, because the tools are called in-process as the current user in the current tenant:

```csharp
[DependsOn(
    typeof(AIManagementAgentToolsModule),
    typeof(AbpAgentToolsAspNetCoreModule) // the Agent Tools runtime
)]
public class MyProjectNameHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAgentToolOptions>(options =>
        {
            options.IncludedRemoteServiceNames.Add("Default"); // which services are tools
        });

        Configure<AIManagementAgentToolsOptions>(options =>
        {
            options.EnabledWorkspaces.Add("Support");
        });
    }
}
```

When a chat turn needs Agent Tools tools and the runtime is not loaded, the turn fails with an exception that names the missing module.

The package does not need the other packages of the Agent Tools module. Without them, the policy of the tools comes from the [configuration](../../framework/infrastructure/artificial-intelligence/agent-tools/policy.md#configuration) and the code defaults; with them, also from the stored [host and tenant values](./policy.md#defaults-and-inheritance).

## Options

```csharp
Configure<AIManagementAgentToolsOptions>(options =>
{
    options.EnabledWorkspaces.Add("Support");                           // by name (case-insensitive)
    options.WorkspaceSelector = name => name.StartsWith("Agent-");      // or by predicate
    options.MaxToolsPerTurn = 32;
    options.McpServerPermissions[new Guid("0b1f6c0e-3c1a-4f55-9a52-2d1d8f6a7c11")] = "MyProjectName.UseBillingMcp"; // the MCP server's id
});
```

| Property | Default | Meaning |
| --- | --- | --- |
| `EnabledWorkspaces`, `WorkspaceSelector` | none | A workspace gets Agent Tools tools when it is listed or the predicate returns `true`. No workspace gets them by default. |
| `MaxToolsPerTurn` | `64` | At most this many Agent Tools tools per turn; the smaller of this and `AbpAgentToolOptions.MaxTools` applies. When more are available, a warning is logged once per workspace. `0` or less adds none. |
| `McpServerPermissions` | empty | MCP server id to permission name. A user without the permission does not get that server's tools. |

## Read-Only in Chat

AI Management's chat UIs have no approval step for these tools yet. So the bridge never offers a tool that is disabled or requires approval: by default the chat gets the read tools only. To offer a write tool in chat, turn its **Requires approval** switch off on the [management page](./policy.md#management-page), set `RequiresApproval` to `false` in the [configuration](../../framework/infrastructure/artificial-intelligence/agent-tools/policy.md#configuration), or use `[AgentTool(RequiresApproval = false)]`. The same tools then become available over [MCP](../../framework/infrastructure/artificial-intelligence/agent-tools/mcp.md) too.

Other rules:

- A turn without an authenticated user gets no Agent Tools tools, and continues. This applies to turns that run anonymously and to turns authenticated with a client credentials token.
- A tool that is already in the turn (`search_documents`, an MCP tool or a tool the caller added) wins over an Agent Tools tool of the same name. The Agent Tools tool is skipped and a warning is logged. Rename it with `[AgentTool(Name = ...)]` to offer both.
- `search_documents` is reserved: an Agent Tools tool of that name is never added.

## MCP Server Permissions

Once the package is installed, it governs the MCP tools of every workspace, whether the workspace is opted in or not. Each MCP tool call produces one call record (tool, workspace, MCP server, user, tenant, outcome, duration; arguments redacted like Agent Tools does). There is no approval step for MCP tools.

`McpServerPermissions` restricts an MCP server to users that have a permission. It is keyed by the MCP server's id (a `Guid`), not by its name, so renaming the server keeps the restriction. A permission name that is not defined is granted to no one, so that server's tools disappear for everyone; a warning is logged.

Server ids are generated per database, so the same server has a different id in each environment. Keep the map in configuration per environment. Do not bind the configuration directly into the `Guid`-keyed dictionary: the configuration binder skips `Guid` keys without an error, and the restriction would silently not apply. Bind string keys and parse them instead:

```json
{
  "AIManagementAgentTools": {
    "McpServerPermissions": {
      "0b1f6c0e-3c1a-4f55-9a52-2d1d8f6a7c11": "MyProjectName.UseBillingMcp"
    }
  }
}
```

```csharp
var configuration = context.Services.GetConfiguration();

Configure<AIManagementAgentToolsOptions>(options =>
{
    var permissions = configuration
        .GetSection("AIManagementAgentTools:McpServerPermissions")
        .Get<Dictionary<string, string>>() ?? new();

    foreach (var (id, permission) in permissions)
    {
        options.McpServerPermissions[Guid.Parse(id)] = permission;
    }
});
```

A mistyped id then throws a `FormatException` on the first chat turn, instead of silently not applying. Find a server's id in the AI Management MCP server list or its API.

An anonymous turn is not granted any permission, so a permission on every MCP server also keeps anonymous turns away from MCP tools. A turn with a client credentials token has only the permissions granted to the client.

## Not Covered

The bridge works through a hook of AI Management's chat client, which runs in every chat turn (normal and streaming). It does not cover:

- A workspace without an active configuration that falls back to the plain `IChatClient` registered in dependency injection.
- Custom workspace executors that do not use AI Management's chat client.
- MCP tools added to the turn by code that runs after the bridge.
- A replaced chat client resolver that does not pass the registered tool contributors to AI Management's chat client: then no Agent Tools tools are added and `McpServerPermissions` is not enforced.

The host must run the AI Management chat and the application services in the same process. A chat host that calls the application services of another host remotely cannot use the bridge for them.

## Extending the Chat Tools

The bridge is an implementation of AI Management's `IChatToolContributor` interface (namespace `Volo.AIManagement.Workspaces`, package `Volo.AIManagement.Abstractions`), which any module can implement to add, remove or wrap the tools of a chat turn:

```csharp
public interface IChatToolContributor
{
    Task ContributeAsync(ChatToolContributionContext context);
}
```

All registered contributors run in registration order, once per turn, after AI Management added its RAG and MCP tools. The context carries `WorkspaceName`, `WorkspaceId`, the mutable `Options` (with `Options.Tools`), `SupportsApproval` (always `false` for now: do not add tools that require approval) and the `CancellationToken`. The user and tenant are the ambient `ICurrentUser` and `ICurrentTenant`, and can be empty.

An exception from a contributor fails the turn, because skipping a contributor that governs tools would hand ungoverned tools to the model.
