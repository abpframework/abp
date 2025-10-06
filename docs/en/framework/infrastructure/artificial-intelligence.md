# Artificial Intelligence

ABP provides a simple way to integrate AI capabilities into your applications by unifying two popular .NET AI stacks under a common concept called a "workspace":

- Microsoft.Extensions.AI `IChatClient`
- Microsoft.SemanticKernel `Kernel`

A workspace is just a named scope. You configure providers per workspace and then resolve either default services (for the "Default" workspace) or workspace-scoped services.

## Installation

> This package is not included by default. Install it to enable AI features.

It is suggested to use the ABP CLI to install the package. Open a command line window in the folder of the project (.csproj file) and type the following command:

```bash
abp add-package Volo.Abp.AI
```

### Manual Installation

Add nuget package to your project:

```bash
dotnet add package Volo.Abp.AI
```

Then add the module dependency to your module class:

```csharp
using Volo.Abp.AI;
using Volo.Abp.Modularity;

[DependsOn(typeof(AbpAIModule))]
public class MyProjectModule : AbpModule
{
}
```

## Usage

### Chat Client

#### Default configuration (quick start)

Configure the default workspace to inject `IChatClient` directly.

```csharp
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Volo.Abp.AI;
using Volo.Abp.Modularity;

public class MyProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpAIOptions>(options =>
        {
            options.Workspaces.ConfigureDefault(configuration =>
            {
                configuration.ConfigureChatClient(chatClientConfiguration =>
                {
                    chatClientConfiguration.Builder = new ChatClientBuilder(
                        sp => new OllamaApiClient("http://localhost:11434", "mistral")
                    );
                });

                // Chat client only in this quick start
            });
        });
    }
}
```

Once configured, inject the default chat client:

```csharp
using Microsoft.Extensions.AI;

public class MyService
{
    private readonly IChatClient _chatClient; // default chat client

    public MyService(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }
}
```

#### Workspace configuration

Workspaces allow multiple, isolated AI configurations. Define workspace types (optionally decorated with `WorkspaceNameAttribute`). If omitted, the type’s full name is used.

```csharp
using Volo.Abp.AI;

[WorkspaceName("GreetingAssistant")]
public class GreetingAssistant // ChatClient-only workspace
{
}
```

Configure a ChatClient workspace:

```csharp
public class MyProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpAIOptions>(options =>
        {
            options.Workspaces.Configure<GreetingAssistant>(configuration =>
            {
                configuration.ConfigureChatClient(chatClientConfiguration =>
                {
                    chatClientConfiguration.Builder = new ChatClientBuilder(
                        sp => new OllamaApiClient("http://localhost:11434", "mistral")
                    );

                    chatClientConfiguration.BuilderConfigurers.Add(builder =>
                    {
                        // Anything you want to do with the builder:
                        // builder.UseFunctionInvocation().UseLogging(); // For example
                    });
                });
            });
        });
    }
}
```

### Semantic Kernel

#### Default configuration


```csharp
public class MyProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpAIOptions>(options =>
        {
            options.Workspaces.ConfigureDefault(configuration =>
            {
                configuration.ConfigureKernel(kernelConfiguration =>
                {
                    kernelConfiguration.Builder = Kernel.CreateBuilder()
                        .AddAzureOpenAIChatClient("...", "...");
                });
                // Note: Chat client is not configured here
            });
        });
    }
}
```

Once configured, inject the default kernel:

```csharp
using System.Threading.Tasks;
using Volo.Abp.AI;

public class MyService
{
    private readonly IKernelAccessor _kernelAccessor;
    public MyService(IKernelAccessor kernelAccessor)
    {
        _kernelAccessor = kernelAccessor;
    }

    public async Task DoSomethingAsync()
    {
        var kernel = _kernelAccessor.Kernel; // Kernel might be null if no workspace is configured.

        var result = await kernel.InvokeAsync(/*... */);
    }
}
```

#### Workspace configuration

```csharp
public class MyProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpAIOptions>(options =>
        {
            options.Workspaces.Configure<ContentPlanner>(configuration =>
            {
                configuration.ConfigureKernel(kernelConfiguration =>
                {
                    kernelConfiguration.Builder = Kernel.CreateBuilder()
                        .AddOpenAIChatCompletion("...", "...");
                });
            });
        });
    }
}
```

#### Workspace usage

```csharp
using Microsoft.Extensions.AI;
using Volo.Abp.AI;
using Microsoft.SemanticKernel;

public class PlanningService
{
    private readonly IKernelAccessor<ContentPlanner> _kernelAccessor;
    private readonly IChatClient<ContentPlanner> _chatClient; // available even if only Kernel is configured

    public PlanningService(
        IKernelAccessor<ContentPlanner> kernelAccessor,
        IChatClient<ContentPlanner> chatClient)
    {
        _kernelAccessor = kernelAccessor;
        _chatClient = chatClient;
    }

    public async Task<string> PlanAsync(string topic)
    {
        var kernel = _kernelAccessor.Kernel; // Microsoft.SemanticKernel.Kernel
        // Use Semantic Kernel APIs if needed...

        var response = await _chatClient.GetResponseAsync(
            [new ChatMessage(ChatRole.User, $"Create a content plan for: {topic}")]
        );
        return response?.Message?.Text ?? string.Empty;
    }
}
```

## Options

`AbpAIOptions` configuration pattern offers `ConfigureChatClient(...)` and `ConfigureKernel(...)` methods for configuration. These methods are defined in the `WorkspaceConfiguration` class. They are used to configure the `ChatClient` and `Kernel` respectively.

`Builder` is set once and is used to build the `ChatClient` or `Kernel` instance. `BuilderConfigurers` is a list of actions that are applied to the `Builder` instance for incremental changes. These actions are executed in the order they are added.

If a workspace configures only the Kernel, a chat client may still be exposed for that workspace through the Kernel’s service provider (when available).


## Advanced Usage and Customizations

### Addding Your Own DelegatingChatClient

If you want to build your own decorator, implement a `DelegatingChatClient` derivative and provide an extension method that adds it to the `ChatClientBuilder` using `builder.Use(...)`.

Example sketch:

```csharp
using Microsoft.Extensions.AI;

public class SystemMessageChatClient : DelegatingChatClient
{
    public SystemMessageChatClient(IChatClient inner, string systemMessage) : base(inner) 
    {
         SystemMessage = systemMessage;
    }

    public string SystemMessage { get; set; }

    public override Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        // Mutate messages/options as needed, then call base
        return base.GetResponseAsync(messages, options, cancellationToken);
    }
}

public static class SystemMessageChatClientExtensions
{
    public static ChatClientBuilder UseSystemMessage(this ChatClientBuilder builder, string systemMessage)
    {
        return builder.Use(client => new SystemMessageChatClient(client, systemMessage));
    }
}
```


```cs
chatClientConfiguration.BuilderConfigurers.Add(builder =>
{
    builder.UseSystemMessage("You are a helpful assistant that greets users in a friendly manner with their names.");
});
```

## Technical Anatomy

- `AbpAIModule`: Wires up configured workspaces, registers keyed services and default services for the `"Default"` workspace.
- `AbpAIOptions`: Holds `Workspaces` and provides helper methods for internal keyed service naming.
- `WorkspaceConfigurationDictionary` and `WorkspaceConfiguration`: Configure per-workspace Chat Client and Kernel.
- `ChatClientConfiguration` and `KernelConfiguration`: Hold builders and a list of ordered builder configurers.
- `WorkspaceNameAttribute`: Names a workspace; falls back to the type’s full name if not specified.
- `IChatClient<TWorkspace>`: Typed chat client for a workspace.
- `IKernelAccessor<TWorkspace>`: Provides access to the workspace’s `Kernel` instance if configured.
- `AbpAIWorkspaceOptions`: Exposes `ConfiguredWorkspaceNames` for diagnostics.

There are no database tables for this feature; it is a pure configuration and DI integration layer.

## See Also

- Microsoft.Extensions.AI (Chat Client)
- Microsoft Semantic Kernel