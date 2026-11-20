```json
//[doc-seo]
{
    "Description": "Enable, disable and require approval for each agent tool per host and tenant with the ABP Agent Tools policy, its permissions, feature, management page and HTTP API."
}
```

# Agent Tools: Tool Policy and Management Page

> You must have an [ABP Team or a higher license](https://abp.io/pricing) to use this module.

The tool policy decides, per tool, two things:

- **Enabled**: whether the tool is offered and can be called.
- **Requires approval**: whether a human must approve each call. A client that cannot ask a human (the MCP endpoint, AI Management chat) does not get such a tool at all.

## Defaults and Inheritance

Every tool has a code default: enabled, and requiring approval as its [HTTP verb](./exposing-tools.md#kind-annotations-and-approval) or its `[AgentTool(RequiresApproval = ...)]` value says. So by default only `GET` tools run without approval.

On top of the code default, the policy is stored per scope: the host, or one tenant. Each stored value can be empty, which means "inherit". Each field of each tool is resolved on its own, first match wins:

1. The current tenant's value.
2. The host's value.
3. The code default.

A host value applies to every tenant that does not set the field itself. For example, a host that disables a tool disables it for all tenants that did not enable it explicitly. In the host context only the host value and the code default apply.

Values for tools that are no longer in the catalogue (renamed, removed or no longer included) are kept as **orphans** and never used. The management page lists them and can purge them.

Without the [EF Core or MongoDB package](./installation.md#database) the policy has only the code defaults.

## Permissions

| Permission | Task |
| --- | --- |
| `AgentTools.Policy` | View the tool policy of the current scope (host or tenant) and the menu item |
| `AgentTools.Policy.Manage` | Change, reset and purge the tool policy of the current scope |

Both permissions are available on the host and the tenant side. The host edits host values; a tenant edits only its own values. `AgentTools.Policy.Manage` is a child of `AgentTools.Policy`.

The permissions are constants of `AgentToolsPermissions`:

```csharp
AgentToolsPermissions.Policy.Default  // "AgentTools.Policy"
AgentToolsPermissions.Policy.Manage   // "AgentTools.Policy.Manage"
```

Tool permissions are separate: a user can call a tool only if the user has the permissions of the application service method behind it. The tool policy can only narrow what a user may do, never widen it.

## Feature

The `AgentTools.Enable` feature (a toggle in the `AgentTools` group, `true` by default; the constant is `AgentToolsFeatures.Enable`) switches the module off per tenant or edition:

- The tool list is empty, and every call is refused.
- The policy application service and the management page are refused, and the permissions are not granted.

See [Features](../../framework/infrastructure/features.md) to set it per tenant or edition.

## Management Page

The `Volo.AgentTools.Web` package adds the **Agent tools** page under **Administration** (route `/AgentTools`). The menu item and the page require the `AgentTools.Policy` permission and the `AgentTools.Enable` feature.

The page shows:

- Counts: tools, enabled, requiring approval and hidden from external agents.
- A text filter (name, description, remote service) and quick filters (kind, enabled, requires approval, changed here).
- The tools grouped by remote service and controller, each with an **Enabled** and a **Requires approval** switch. Switches save at once.
- A source column. In the host: **Code default** or **Host**. In a tenant: **Default** or **Changed here**. A changed row has a **Reset** link that removes the override of this scope.
- **MCP: hidden** on a tool that is disabled or requires approval, so the MCP endpoint does not offer it.
- **Reset all**, with a confirmation, which removes every override of this scope.
- Orphaned settings, in a collapsible list with a **Purge** button, when the scope has values for tools that are not in the catalogue.

Clicking a tool name opens a detail panel: the description the model sees, the HTTP method and URL, the remote service and controller, the parameters, the exact JSON schema the model gets, the annotations, and the effective values with their source.

Without `AgentTools.Policy.Manage` the page is read-only: the switches are disabled and Reset, Reset all and Purge are not shown.

A tenant is never told whether an inherited value comes from the code or from the host. See [Security Notes](./security.md#policy) for what a tenant can still infer.

## Application Service

`IAgentToolPolicyAppService` (namespace `Volo.AgentTools.Policies`) works on the current scope: in the host context on host values, in a tenant on that tenant's values.

| Method | Permission | Behavior |
| --- | --- | --- |
| `GetListAsync()` | `AgentTools.Policy` | Every tool of the catalogue with its effective values and their source, this scope's own values and concurrency stamp, the description, kind, annotations, HTTP method and URL, parameters and JSON schema; counts and the number of orphans. |
| `UpdateAsync(toolName, input)` | `AgentTools.Policy.Manage` | Replaces this scope's two values for the tool (`null` inherits; both `null` removes the override). |
| `ResetAsync()` | `AgentTools.Policy.Manage` | Removes every override of this scope, orphans included. |
| `GetOrphansAsync()` | `AgentTools.Policy` | Overrides of this scope for tools that are not in the catalogue. |
| `PurgeOrphansAsync()` | `AgentTools.Policy.Manage` | Removes those overrides. |

`UpdateAgentToolPolicyDto` has `Enabled` (`bool?`), `RequiresApproval` (`bool?`) and an optional `ConcurrencyStamp`. A stamp that does not match the stored value fails with ABP's concurrency error (HTTP 409); so does a first change of a tool that loses a race against another first change of the same tool. An unknown tool name fails with the error code `AgentTools:ToolNotFound` (HTTP 404).

Every method also requires the `AgentTools.Enable` feature.

## HTTP API

| Verb | Route | Method | Body | Success |
| --- | --- | --- | --- | --- |
| GET | `/api/agent-tools/policies` | `GetListAsync` | | 200 |
| PUT | `/api/agent-tools/policies/{toolName}` | `UpdateAsync` | `{ "enabled": bool?, "requiresApproval": bool?, "concurrencyStamp": string? }` | 200, the tool |
| DELETE | `/api/agent-tools/policies` | `ResetAsync` | | 204 |
| GET | `/api/agent-tools/policies/orphans` | `GetOrphansAsync` | | 200 |
| DELETE | `/api/agent-tools/policies/orphans` | `PurgeOrphansAsync` | | 204 |

The remote service name is `AgentTools`. A missing permission gives 403 (401 when anonymous).

## Caching

The policy store caches each scope's values in the [distributed cache](../../framework/fundamentals/caching.md) for at most 10 minutes. A change removes the scope's entry when its unit of work completes, so the next tool list and the next call use the new values without a restart.

With more than one application instance, configure a shared distributed cache (for example Redis). Otherwise other instances use the old values until their entry expires.

## Custom Policy Stores

The policy is read through `IAgentToolPolicyStore`. The module registers:

- `CodeDefaultAgentToolPolicyStore`: code defaults only. Registered by default.
- `AgentToolPolicyStore`: the cached, table-backed store. Registered by the EF Core and MongoDB packages.
- `InMemoryAgentToolPolicyStore`: for tests and samples. It is not registered; register it as a singleton `IAgentToolPolicyStore` and use `Set(toolName, tenantId, enabled, requiresApproval)` and `Clear()`.

To write your own store, derive from `AgentToolPolicyStoreBase` and implement `GetOverridesAsync(tenantId, cancellationToken)`, which returns all overrides of one scope. The base class applies the inheritance rules above.
