```json
//[doc-seo]
{
    "Description": "Manage which agent tools the host and each tenant may use with the ABP Agent Tools module: a stored tool policy, a management page, an HTTP API and AI Management integration."
}
```

# Agent Tools Module (Pro)

> You must have an [ABP Team or a higher license](https://abp.io/pricing) to use this module.

The Agent Tools module adds management to ABP's open source [Agent Tools](../../framework/infrastructure/artificial-intelligence/agent-tools/index.md) packages.

The two parts have different responsibilities:

- The **open source packages** turn application services into AI tools: discovery, permission-checked and audited calls, the tool list for your own chat client, the MCP endpoint, the OpenIddict configuration and a tool policy in `appsettings.json`.
- The **Agent Tools module** manages the policy of these tools per host and tenant.

Read the [open source documentation](../../framework/infrastructure/artificial-intelligence/agent-tools/index.md) first to expose and use tools.

## Features

- **Tool policy per host and tenant**: whether a tool is enabled and whether it requires approval is stored in the database, with host defaults that each tenant can override per field. Changes apply without a restart. The [configuration policy](../../framework/infrastructure/artificial-intelligence/agent-tools/policy.md) and the code defaults apply to everything that is not stored. See [Tool Policy and Management Page](./policy.md).
- **Management page**: a page that lists every tool with its policy, its source and what the model sees, for the host and for tenants. See [Management Page](./policy.md#management-page).
- **Application service and HTTP API** for the policy, with their own permissions. See [HTTP API](./policy.md#http-api).
- **AI Management integration**: the `Volo.AIManagement.AgentTools` package adds the signed-in user's tools to AI Management chat and governs the MCP tools attached to its workspaces. See [AI Management Integration](./ai-management.md).
- **MCP client preset**: the OpenIddict Pro UI fills the form for MCP clients. See [MCP Client Preset](./openiddict-preset.md).

## Packages

| Package | Layer | Purpose |
| --- | --- | --- |
| `Volo.AgentTools.Domain.Shared` | Domain.Shared | Constants, the policy sources and the localization of the permissions and the page. |
| `Volo.AgentTools.Domain` | Domain | The policy entity and the stored policy. Depends on `Volo.Abp.AgentTools`. |
| `Volo.AgentTools.Application.Contracts` | Application.Contracts | `IAgentToolPolicyAppService`, its DTOs and the permissions. |
| `Volo.AgentTools.Application` | Application | The policy application service. |
| `Volo.AgentTools.HttpApi` | HttpApi | The policy HTTP API. Depends on `Volo.Abp.AgentTools.AspNetCore`. |
| `Volo.AgentTools.HttpApi.Client` | HttpApi.Client | Dynamic C# client proxies for the policy HTTP API. |
| `Volo.AgentTools.EntityFrameworkCore` | EntityFrameworkCore | The policy table and repository. |
| `Volo.AgentTools.MongoDB` | MongoDB | The policy collection and repository. |
| `Volo.AgentTools.Web` | Web (MVC) | The **Agent tools** management page. |

The attributes, options, MCP endpoint and OpenIddict configuration are in the open source `Volo.Abp.AgentTools.*` and `Volo.Abp.OpenIddict.AgentTools` packages.

## Relation to AI Management

Agent Tools does not need [AI Management](../ai-management/index.md): you can use the tools from your own `IChatClient` or serve them over MCP only. The `Volo.AIManagement.AgentTools` package connects the two. See [AI Management Integration](./ai-management.md).

## User Interface

The module provides an MVC / Razor Pages management page under **Administration** > **Agent tools**, and the [React Admin Console](../../framework/ui/react/admin-console.md) has a page for it when the module is in the host. Blazor and Angular UIs are not available yet; the policy HTTP API can be used from any client.

## Documentation

- [Installation](./installation.md): packages per layer, module dependencies and the database.
- [Tool Policy and Management Page](./policy.md): host and tenant values, permissions, the feature, the page and the HTTP API.
- [AI Management Integration](./ai-management.md): the bridge package.
- [MCP Client Preset](./openiddict-preset.md): registering MCP clients in the OpenIddict Pro UI.

## See Also

- [Agent Tools (open source)](../../framework/infrastructure/artificial-intelligence/agent-tools/index.md)
- [AI Management Module](../ai-management/index.md)
- [OpenIddict Module (Pro)](../openiddict-pro.md)
