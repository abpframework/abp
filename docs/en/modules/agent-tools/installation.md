```json
//[doc-seo]
{
    "Description": "Install the ABP Agent Tools module: NuGet packages per layer, module dependencies, the tool policy table for EF Core and MongoDB, and the open source host packages."
}
```

# Agent Tools: Installation (Pro)

> You must have an [ABP Team or a higher license](https://abp.io/pricing) to use this module.

The Agent Tools module is not included in [the startup templates](../../solution-templates/layered-web-application/index.md). Install it into an existing solution with the ABP CLI, ABP Studio or manually.

## Using ABP CLI

Run the following command in the solution directory:

```bash
abp add-module Volo.AgentTools
```

**Using ABP Studio:** Open ABP Studio, navigate to your solution explorer, **Right Click** on the project and select **Import Module**. Choose `Volo.AgentTools` from the `NuGet` tab and check the "Install this Module" checkbox.

Then check the [host packages](#host-packages) below: they belong to specific hosts, and which ones you need depends on how agents reach your application. They are not installed by this command.

## Manual Installation

Add the packages to the matching projects of your solution and add each module class to the `DependsOn` attribute of the project's module. For a layered solution:

| Project | Package | Module class |
| --- | --- | --- |
| `{ProjectName}.Domain.Shared` | `Volo.AgentTools.Domain.Shared` | `AgentToolsDomainSharedModule` |
| `{ProjectName}.Domain` | `Volo.AgentTools.Domain` | `AgentToolsDomainModule` |
| `{ProjectName}.Application.Contracts` | `Volo.AgentTools.Application.Contracts` | `AgentToolsApplicationContractsModule` |
| `{ProjectName}.Application` | `Volo.AgentTools.Application` | `AgentToolsApplicationModule` |
| `{ProjectName}.EntityFrameworkCore` | `Volo.AgentTools.EntityFrameworkCore` | `AgentToolsEntityFrameworkCoreModule` |
| `{ProjectName}.MongoDB` | `Volo.AgentTools.MongoDB` | `AgentToolsMongoDbModule` |
| `{ProjectName}.HttpApi` | `Volo.AgentTools.HttpApi` | `AgentToolsHttpApiModule` |
| `{ProjectName}.HttpApi.Client` | `Volo.AgentTools.HttpApi.Client` | `AgentToolsHttpApiClientModule` |
| `{ProjectName}.Web` (MVC UI) | `Volo.AgentTools.Web` | `AgentToolsWebModule` |

For example, add the package to the `{ProjectName}.Application.csproj` file:

```xml
<PackageReference Include="Volo.AgentTools.Application" Version="x.x.x" />
```

And the module to `{ProjectName}ApplicationModule`:

```csharp
[DependsOn(
    //...
    typeof(AgentToolsApplicationModule)
)]
public class MyProjectNameApplicationModule : AbpModule
{
}
```

In a single-layer solution, add the packages you need to the single project and their modules to its module class.

Notes on the layers:

- `AgentToolsHttpApiModule` depends on `AbpAgentToolsAspNetCoreModule` (package `Volo.Abp.AgentTools.AspNetCore`), the open source [runtime](../../framework/infrastructure/artificial-intelligence/agent-tools/index.md#packages) that discovers and calls the tools. The policy application service needs it, so the host that runs the application services must load the HTTP API module (or `AbpAgentToolsAspNetCoreModule` directly).
- Tools are called in-process. The runtime must run in the same host as the application services it exposes.
- The management page calls the policy service through ABP's dynamic JavaScript proxy, so an MVC application that hosts its own APIs also needs `AgentToolsHttpApiModule`.
- `AgentToolsHttpApiClientModule` registers dynamic C# client proxies for `IAgentToolPolicyAppService` under the remote service name `AgentTools` (configure `RemoteServices:AgentTools:BaseUrl` when it differs from `Default`).
- Without a database package the stored policy is not available: the tools then use the configuration and the code defaults only (see [Tool Policy](./policy.md)).

## Host Packages

`Volo.Abp.AgentTools.Mcp` and `Volo.Abp.OpenIddict.AgentTools` are open source ABP packages. Add them to the hosts that need them, for example with `abp add-package`, and add the module class to the `DependsOn` attribute of the host's module.

| Host | Package | Module class | When |
| --- | --- | --- | --- |
| The host that runs the application services (`{ProjectName}.HttpApi.Host`, or the single application) | `Volo.Abp.AgentTools.Mcp` | `AbpAgentToolsMcpModule` | External agents use the tools over MCP. See [MCP Endpoint](../../framework/infrastructure/artificial-intelligence/agent-tools/mcp.md). |
| The host that runs the OpenIddict server (the auth server, or the single application), and the `DbMigrator` | `Volo.Abp.OpenIddict.AgentTools` | `AbpOpenIddictAgentToolsModule` | The MCP endpoint's tokens come from the application's OpenIddict server. The module keeps the MCP resource permission in step with the `AgentTools` scope, so a web host needs the MCP URL (`App:SelfUrl` or `McpResource`) or, with the sync on (the default), fails at startup. See [OpenIddict Setup](../../framework/infrastructure/artificial-intelligence/agent-tools/openiddict.md#the-resource-permission). |
| The host that runs AI Management chat | `Volo.AIManagement.AgentTools` | `AIManagementAgentToolsModule` | AI Management chat uses the tools. See [AI Management Integration](./ai-management.md). |

The `DbMigrator` needs `AbpOpenIddictAgentToolsModule` because the OpenIddict scope and the MCP clients are created by data seed contributors of that module.

## Database

The tool policy is stored in one table (EF Core) or collection (MongoDB) named `AgentToolsPolicies`. The connection string name is `AgentTools`; when it is not configured, ABP uses the `Default` connection string.

### Entity Framework Core

Configure the module in the `OnModelCreating` method of your application's `DbContext`, and replace the module's `DbContext` interface with your own so that the table is part of your migrations:

```csharp
[ReplaceDbContext(typeof(IAgentToolsDbContext))]
[ConnectionStringName("Default")]
public class MyProjectNameDbContext :
    AbpDbContext<MyProjectNameDbContext>,
    IAgentToolsDbContext
{
    public DbSet<AgentToolPolicy> Policies { get; set; }

    //...

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //...
        builder.ConfigureAgentTools();
    }
}
```

Then add a migration and update the database, for example:

```bash
dotnet ef migrations add Added_AgentTools
```

Run the `DbMigrator` (or `dotnet run --migrate-database` in a single-layer solution) to apply the migration and run the data seeders.

`AgentToolsDbProperties.DbTablePrefix` (default `AgentTools`) and `AgentToolsDbProperties.DbSchema` (default `null`) change the table prefix and schema. Set them before the model is created, as for other ABP modules.

### MongoDB

Configure the module in the `CreateModel` method of your application's MongoDB context:

```csharp
[ReplaceDbContext(typeof(IAgentToolsMongoDbContext))]
[ConnectionStringName("Default")]
public class MyProjectNameMongoDbContext : AbpMongoDbContext, IAgentToolsMongoDbContext
{
    public IMongoCollection<AgentToolPolicy> Policies => Collection<AgentToolPolicy>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureAgentTools();
    }
}
```

MongoDB needs no migration. The collection has a unique index on `(TenantId, ToolName)`, created when the database is initialized, as for other modules.

## Next Steps

With the packages installed the tool catalog is still empty. Choose what to expose in [Exposing Tools](../../framework/infrastructure/artificial-intelligence/agent-tools/exposing-tools.md), then manage the tools on the [management page](./policy.md#management-page).
