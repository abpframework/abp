```json
//[doc-seo]
{
    "Description": "Enable or disable ABP agent tools and choose which of them require approval, in appsettings.json, environment variables or code, and learn how the settings are validated at startup."
}
```

# Agent Tools: Tool Policy

The tool policy decides, per tool, two things:

- **Enabled**: whether the tool is offered and can be called.
- **Requires approval**: whether a human must approve each call. A client that cannot ask a human does not get such a tool at all. The [MCP endpoint](./mcp.md) is such a client: a tool that requires approval is not offered over MCP.

Every tool has a code default: enabled, and requiring approval as its [HTTP verb](./exposing-tools.md#kind-annotations-and-approval) or its `[AgentTool(RequiresApproval = ...)]` value says. So by default only `GET` tools run without approval.

The policy can only narrow what a user may do, never widen it: a user still needs the permissions of the application service method behind the tool.

## Configuration

Set the policy in the `AgentTools:Tools` section of the [configuration](../../../fundamentals/configuration.md), by tool name:

```json
{
  "AgentTools": {
    "Tools": {
      "Book_Delete": { "Enabled": false },
      "Book_GetSales": { "RequiresApproval": true },
      "Book_Create": { "RequiresApproval": false }
    }
  }
}
```

| Field | `true` | `false` |
| --- | --- | --- |
| `Enabled` | The tool is enabled (the code default). | The tool is not listed, and a call by name is refused with `AgentTools:ToolDisabled`. |
| `RequiresApproval` | The tool requires approval, also when it is a read tool. It is not listed over MCP, and a client without approval support gets `AgentTools:ApprovalNotSupported` when it calls the tool. | The tool runs without approval, also when it is a write tool. It can be listed over MCP. |

- Both fields are optional. A field that is not set keeps the value set [in code](#configuration-in-code), or else the code default of the tool.
- Tool names and field names are case-insensitive.
- The policy applies to the whole application: it is the same for the host and every tenant.
- The section is read once, at application startup. Restart the application after a change.

The section works with every configuration source. For example, as environment variables:

```bash
AgentTools__Tools__Book_Delete__Enabled=false
AgentTools__Tools__Book_Create__RequiresApproval=false
```

A tool name with two consecutive underscores cannot be written as an environment variable, because `__` separates the sections; such a variable stops startup. A tool name that consists of digits only cannot be set in the section.

### Null Values

`null` and an empty object mean "not set", in the section, a tool and a field. .NET merges the configuration sources key by key, and the last source that has a key wins, also with `null`. This has three consequences:

- A `null` or `{}` for a **field** in a later source undoes the value of an earlier one. With `"Enabled": false` in `appsettings.json` and `"Enabled": null` (or `"Enabled": {}`) in `appsettings.Production.json`, the field is not set in production, and the tool is enabled there unless code disables it.
- A `{}` for a **field** also undoes the value that the same file sets before it.
- A `null` or `{}` for a whole **tool** or for the **section** in a later source does not remove the fields that an earlier source sets. To undo `"RequiresApproval": false`, set the field itself to `true`, or to `null` for the value below it.

## Configuration in Code

`AbpAgentToolPolicyOptions` takes the same settings in code:

```csharp
Configure<AbpAgentToolPolicyOptions>(options =>
{
    options.Tools["Book_Delete"] = new AgentToolPolicyConfiguration
    {
        Enabled = false
    };
});
```

An assignment like this replaces the whole entry of the tool, with both fields, also one that an earlier module has set. To set one field only, change the existing entry.

The configuration section overrides these settings per field. It is applied after every `Configure` action, so a module cannot re-enable with `Configure` what `appsettings.json` disables. Only a `PostConfigure` action that is registered after the one of `AbpAgentToolsModule` can still change the result.

## Startup Checks

The checks run when an application with `AbpAgentToolsAspNetCoreModule` starts (the MCP module includes it), also when no service is included as a tool and whatever policy store is registered.

### What Stops Startup

Anything in the `AgentTools:Tools` section that is not a valid policy stops startup with an `AbpException` that names the configuration key (not the value):

| Example | Problem |
| --- | --- |
| `"Book_Delete": false` | A tool must be an object. |
| `"Book_Delete": { "Enable": false }` | An unknown field. |
| `"Book_Delete": { "Enabled": "flase" }` | A value other than `true` and `false`. |
| `"Tools": [ ... ]`, `"Tools": false` | The section must be an object with tool names as keys. |

### What Is Only Logged

The following settings have no effect. An error that names the tools or keys is logged at startup, and the application starts, because the configuration may serve several applications or hold settings of your own:

- A tool name that is not in the catalog, in the configuration or in code: a misspelled name, or a tool that this application does not include.
- A key named `Enabled` or `RequiresApproval` under a child section of `AgentTools` other than `Tools` and `Mcp`, such as a misspelled section (`AgentTools:Tool:Book_Delete:Enabled`) or a tool directly under `AgentTools` (`AgentTools:Book_Delete:Enabled`).
- A value in place of the whole section, such as `"AgentTools": false`.

### What Is Not Checked

The following settings have no effect either, and nothing is logged:

- `AgentTools:Enabled`, directly under `AgentTools`. It does not switch the tools off; use the [feature](./index.md#feature).
- Other values in a misspelled section, such as `"AgentTools": { "Tool": { "Book_Delete": false } }`.
- Anything under a misspelled root section, such as `AgentTool:Tools`.
- Anything under `AgentTools:Mcp`. The [client seed](./openiddict.md#client-seed) reads its own section there.

Check the policy with a tool list (for example over MCP) after you change it.

## Replacing the Policy Store

The policy is read through `IAgentToolPolicyStore`. The default implementation, `ConfigurationAgentToolPolicyStore`, returns the policy described above. Replace it to decide in another way, for example per tenant or at runtime. This store additionally requires approval for every destructive tool:

```csharp
public class MyAgentToolPolicyStore : ConfigurationAgentToolPolicyStore
{
    public MyAgentToolPolicyStore(IOptions<AbpAgentToolPolicyOptions> options)
        : base(options)
    {
    }

    public override async Task<EffectiveAgentToolPolicy> GetAsync(
        AgentToolDefinition definition,
        CancellationToken cancellationToken = default)
    {
        return Adjust(definition, await base.GetAsync(definition, cancellationToken));
    }

    public override async Task<IReadOnlyDictionary<string, EffectiveAgentToolPolicy>> GetListAsync(
        IReadOnlyList<AgentToolDefinition> definitions,
        CancellationToken cancellationToken = default)
    {
        var policies = await base.GetListAsync(definitions, cancellationToken);
        return definitions.ToDictionary(x => x.Name, x => Adjust(x, policies[x.Name]));
    }

    private static EffectiveAgentToolPolicy Adjust(AgentToolDefinition definition, EffectiveAgentToolPolicy policy)
    {
        return definition.Destructive
            ? policy with { RequiresApproval = true }
            : policy;
    }
}
```

```csharp
context.Services.Replace(
    ServiceDescriptor.Transient<IAgentToolPolicyStore, MyAgentToolPolicyStore>()
);
```

`GetListAsync` is used for tool lists and `GetAsync` for calls, so implement the same rule in both. The store is called in the context of the current tenant (`ICurrentTenant`).

The [Agent Tools module (Pro)](../../../../modules/agent-tools/policy.md#custom-policy-stores) replaces only the default store. A store that your application registers stays, whatever the order of the modules, and the stored policies of that module are then not in use.

## With the Agent Tools Module (Pro)

The [Agent Tools module (Pro)](../../../../modules/agent-tools/policy.md) stores policies per host and tenant in the database. Each field of each tool is then resolved on its own, first match wins:

1. The current tenant's stored value.
2. The host's stored value.
3. The `AgentTools:Tools` configuration section.
4. The value set in code with `Configure<AbpAgentToolPolicyOptions>`.
5. The code default.

Without stored values the configuration applies as described on this page, so installing the module does not re-enable a tool by itself. A host or tenant administrator can change either field back, in both directions, so a stored value can re-enable a tool that the configuration disables.
