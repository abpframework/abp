```json
//[doc-seo]
{
    "Description": "Enable, disable and require approval for each agent tool per host and tenant with the ABP Agent Tools module: inheritance, permissions, feature, management page and HTTP API."
}
```

# Agent Tools: Tool Policy and Management Page (Pro)

> You must have an [ABP Team or a higher license](https://abp.io/pricing) to use this module.

The [tool policy](../../framework/infrastructure/artificial-intelligence/agent-tools/policy.md) decides, per tool, two things:

- **Enabled**: whether the tool is offered and can be called.
- **Requires approval**: whether a human must approve each call. A client that cannot ask a human (the MCP endpoint, AI Management chat) does not get such a tool at all.

## Defaults and Inheritance

Every tool has a code default: enabled, and requiring approval as its [HTTP verb](../../framework/infrastructure/artificial-intelligence/agent-tools/exposing-tools.md#kind-annotations-and-approval) or its `[AgentTool(RequiresApproval = ...)]` value says. So by default only `GET` tools run without approval.

The open source packages let you change the code default in the [`AgentTools:Tools` configuration section](../../framework/infrastructure/artificial-intelligence/agent-tools/policy.md#configuration) or in code. This module stores the policy on top of that, per scope: the host, or one tenant. Each stored value can be empty, which means "inherit". Each field of each tool is resolved on its own, first match wins:

1. The current tenant's value.
2. The host's value.
3. The configuration: the `AgentTools:Tools` section, then the value set in code with `Configure<AbpAgentToolPolicyOptions>`.
4. The code default.

A host value applies to every tenant that does not set the field itself. For example, a host that disables a tool disables it for all tenants that did not enable it explicitly. In the host context only the host value, the configuration and the code default apply.

A stored value can set either field back, in both directions. So a host or tenant administrator can re-enable a tool that the configuration disables, and turn off an approval that it requires. To keep a tool away from everyone, exclude it instead, with [`ExcludedServiceTypes`](../../framework/infrastructure/artificial-intelligence/agent-tools/exposing-tools.md#choosing-what-to-expose) or [`[DisableAgentTool]`](../../framework/infrastructure/artificial-intelligence/agent-tools/exposing-tools.md#disableagenttool).

Installing the module changes nothing by itself: without stored values the configuration and the code defaults apply as before. The [startup checks](../../framework/infrastructure/artificial-intelligence/agent-tools/policy.md#startup-checks) of the configuration also run with this module.

Values for tools that are no longer in the catalog (renamed, removed or no longer included) are kept as **orphans** and never used. The management page lists them and can purge them.

Without the [EF Core or MongoDB package](./installation.md#database) only the configuration and the code defaults apply.

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

The [`AgentTools.Enable` feature](../../framework/infrastructure/artificial-intelligence/agent-tools/index.md#feature) of the open source packages switches the tools off per tenant or edition. With this module, the policy application service and the management page are then refused too, and the permissions are not granted.

See [Features](../../framework/infrastructure/features.md) to set it per tenant or edition.

## Management Page

The `Volo.AgentTools.Web` package adds the **Agent tools** page under **Administration** (route `/AgentTools`). The menu item and the page require the `AgentTools.Policy` permission and the `AgentTools.Enable` feature.

The page shows:

- Counts: tools, enabled, requiring approval and hidden from external agents.
- A text filter (name, description, remote service) and quick filters (kind, enabled, requires approval, changed here).
- The tools grouped by remote service and controller, each with an **Enabled** and a **Requires approval** switch. Switches save at once.
- A source column. In the host: **Code default**, **Configuration** (an unchanged tool with at least one configured value) or **Host**. In a tenant: **Default** or **Changed here**. A changed row has a **Reset** link that removes the override of this scope.
- **MCP: hidden** on a tool that is disabled or requires approval, so the MCP endpoint does not offer it.
- **Reset all**, with a confirmation, which removes every override of this scope.
- Orphaned settings, in a collapsible list with a **Purge** button, when the scope has values for tools that are not in the catalog.

Clicking a tool name opens a detail panel: the description the model sees, the HTTP method and URL, the remote service and controller, the parameters, the exact JSON schema the model gets, the annotations, and the effective values with their source.

Without `AgentTools.Policy.Manage` the page is read-only: the switches are disabled and Reset, Reset all and Purge are not shown.

The page is also read-only, and shows a notice, when the application uses a [policy store of its own](#custom-policy-stores).

A tenant is never told whether an inherited value comes from the code, the configuration or the host. See [Security Notes and Known Limits](#security-notes-and-known-limits) for what a tenant can still infer.

## Application Service

`IAgentToolPolicyAppService` (namespace `Volo.AgentTools.Policies`) works on the current scope: in the host context on host values, in a tenant on that tenant's values.

| Method | Permission | Behavior |
| --- | --- | --- |
| `GetListAsync()` | `AgentTools.Policy` | Every tool of the catalog with its effective values and their source, this scope's own values and concurrency stamp, the description, kind, annotations, HTTP method and URL, parameters and JSON schema; counts and the number of orphans; `PolicyTableInUse` (see [Custom Policy Stores](#custom-policy-stores)). |
| `UpdateAsync(toolName, input)` | `AgentTools.Policy.Manage` | Replaces this scope's two values for the tool (`null` inherits; both `null` removes the override). |
| `ResetAsync()` | `AgentTools.Policy.Manage` | Removes every override of this scope, orphans included. |
| `GetOrphansAsync()` | `AgentTools.Policy` | Overrides of this scope for tools that are not in the catalog. |
| `PurgeOrphansAsync()` | `AgentTools.Policy.Manage` | Removes those overrides. |

`UpdateAgentToolPolicyDto` has `Enabled` (`bool?`), `RequiresApproval` (`bool?`) and an optional `ConcurrencyStamp`. A stamp that does not match the stored value fails with ABP's concurrency error (HTTP 409); so does a first change of a tool that loses a race against another first change of the same tool. An unknown tool name fails with the error code `AgentTools:ToolNotFound` (HTTP 404).

Every method also requires the `AgentTools.Enable` feature.

`UpdateAsync`, `ResetAsync` and `PurgeOrphansAsync` are refused with the error code `AgentTools:PolicyTableNotInUse` when the application uses a [policy store of its own](#custom-policy-stores).

The interface is marked with `[DisableAgentTool]`, so an agent can never change the tool policy.

The source of an effective value is an `AgentToolPolicySource`: `Code`, `Configuration` or `Host` in the host; `Tenant` or `Inherited` in a tenant.

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

The EF Core and MongoDB packages replace the [default `IAgentToolPolicyStore`](../../framework/infrastructure/artificial-intelligence/agent-tools/policy.md#replacing-the-policy-store) with `AgentToolPolicyStore`, the cached, database-backed store. They replace only the default store: a store that your application registers stays, whatever the order of the modules.

With a store of your own the policy table is not in use, so the stored values have no effect:

- An error is logged at startup.
- `AgentToolPolicyListDto.PolicyTableInUse` is `false`.
- The management page and the page of the React Admin Console show a notice and are read-only.
- Writes are refused with the error code `AgentTools:PolicyTableNotInUse`.

A store that derives from `AgentToolPolicyStore` counts as the table, so derive from it to adjust the stored policy. A store that wraps `AgentToolPolicyStore` without deriving from it is a store of your own; the stored values then apply only as far as it uses them.

To write your own store with host and tenant values, derive from `AgentToolPolicyStoreBase` (namespace `Volo.AgentTools`) and implement `GetOverridesAsync(tenantId, cancellationToken)`, which returns all overrides of one scope. The base class applies the inheritance rules above. Such a store is a store of your own: the policy table is not in use.

## Security Notes and Known Limits

See also the [security notes of the open source packages](../../framework/infrastructure/artificial-intelligence/agent-tools/security.md).

- The tool policy can only narrow what a user may do. A user still needs the permissions of the application service method behind a tool.
- A tenant never sees host or configured values as such; inherited values are reported as **Default**. A tenant can still infer one thing: every tool is enabled in code, so an inherited disabled tool can only have been disabled by the host or the configuration. The HTTP method and kind also hint at the code default for approval.
- On databases where `NULL` values are distinct in unique indexes (PostgreSQL, SQLite, MySQL), two administrators changing the same tool in the host at the same moment can store two host values. The oldest one is used, and the next change of that tool removes the other.
- After writing policy values, do not read the policy store in the same transactional unit of work. The store reads committed values on a connection of its own and can wait for your open transaction on some databases. The module itself never does this.
- The management page is available for MVC / Razor Pages and in the React Admin Console. Blazor and Angular pages, and an approval step in AI Management chat, are not available yet.
