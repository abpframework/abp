# Microsoft.Agents.AI (Agent Framework)

[Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/overview/agent-framework-overview) is an open-source development kit for **building AI agents** and **multi-agent workflows**. It is the direct successor to both *AutoGen* and [*Semantic Kernel*](./microsoft-semantic-kernel.md), combining their strengths while adding new capabilities, and is the suggested framework for building AI agent applications. This documentation is about the usage of this library with ABP Framework. Make sure you have read the [Artificial Intelligence](./index.md) documentation before reading this documentation.

## Usage

**Microsoft Agent Framework** works on top of `IChatClient` from **Microsoft.Extensions.AI**. After obtaining an `IChatClient` instance, you can create an AI agent using the `CreateAIAgent` extension method:

```csharp
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

public class MyService
{
    private readonly IChatClient _chatClient;
    
    public MyService(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }
    
    public async Task<string> GetResponseAsync(string userMessage)
    {
        AIAgent agent = _chatClient.CreateAIAgent(
            instructions: "You are a helpful assistant that provides concise answers."
        );
        
        AgentRunResponse response = await agent.RunAsync(userMessage);
        
        return response.Text;
    }
}
```

You can also use `IChatClientAccessor` to access the `IChatClient` in scenarios where AI capabilities are **optional**, such as when developing a module or a service that may use AI capabilities optionally:

```csharp
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Volo.Abp.AI;

public class MyService
{
    private readonly IChatClientAccessor _chatClientAccessor;
    
    public MyService(IChatClientAccessor chatClientAccessor)
    {
        _chatClientAccessor = chatClientAccessor;
    }
    
    public async Task<string> GetResponseAsync(string userMessage)
    {
        var chatClient = _chatClientAccessor.ChatClient;
        if (chatClient is null)
        {
            return "No chat client configured";
        }
        
        AIAgent agent = chatClient.CreateAIAgent(
            instructions: "You are a helpful assistant that provides concise answers."
        );
        
        var response = await agent.RunAsync(userMessage);
        
        return response.Text;
    }
}
```

### Workspaces

Workspaces are a way to configure isolated AI configurations for a named scope. You can define a workspace by decorating a class with the `WorkspaceNameAttribute` attribute that carries the workspace name.
- Workspace names must be unique.
- Workspace names cannot contain spaces _(use underscores or camelCase)_.
- Workspace names are case-sensitive.

```csharp
using Volo.Abp.AI;

[WorkspaceName("CommentSummarization")]
public class CommentSummarization
{
}
```

> [!NOTE]
> If you don't specify the workspace name, the full name of the class will be used as the workspace name.

You can resolve generic versions of `IChatClient` and `IChatClientAccessor` services for a specific workspace as generic arguments. If Chat Client is not configured for a workspace, you will get `null` from the accessor services. You should check the accessor before using it. This applies only for specified workspaces. Another workspace may have a configured Chat Client.

`IChatClient<TWorkSpace>` or `IChatClientAccessor<TWorkSpace>` can be resolved to access a specific workspace's chat client. This is a typed chat client and can be configured separately from the default chat client.

Example of resolving a typed chat client for a workspace:

```csharp
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Volo.Abp.AI;

public class MyService
{
    private readonly IChatClient<CustomerSupport> _chatClient;
    
    public MyService(IChatClient<CustomerSupport> chatClient)
    {
        _chatClient = chatClient;
    }
    
    public async Task<string> GetResponseAsync(string userMessage)
    {
        AIAgent agent = _chatClient.CreateAIAgent(
            instructions: "You are a customer support assistant. Be polite and helpful."
        );
        
        var response = await agent.RunAsync(userMessage);
        return response.Text;
    }
}
```

Example of resolving a typed chat client accessor for a workspace:

```csharp
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Volo.Abp.AI;

public class MyService
{
    private readonly IChatClientAccessor<CustomerSupport> _chatClientAccessor;
    
    public MyService(IChatClientAccessor<CustomerSupport> chatClientAccessor)
    {
        _chatClientAccessor = chatClientAccessor;
    }
    
    public async Task<string> GetResponseAsync(string userMessage)
    {
        var chatClient = _chatClientAccessor.ChatClient;
        if (chatClient is null)
        {
            return "No chat client configured";
        }
        
        AIAgent agent = chatClient.CreateAIAgent(
            instructions: "You are a customer support assistant. Be polite and helpful."
        );
        
        var response = await agent.RunAsync(userMessage);
        return response.Text;
    }
}
```

## Configuration

**Microsoft Agent Framework** uses `IChatClient` from **Microsoft.Extensions.AI** as its foundation. Therefore, the configuration process for workspaces is the same as described in the [Microsoft.Extensions.AI documentation](./microsoft-extensions-ai.md#configuration).

You need to configure the Chat Client for your workspace using `AbpAIWorkspaceOptions`, and then you can use the `CreateAIAgent` extension method to create AI agents from the configured chat client.

To configure a chat client, you'll need a LLM provider package such as [Microsoft.Extensions.AI.OpenAI](https://www.nuget.org/packages/Microsoft.Extensions.AI.OpenAI) or [OllamaSharp](https://www.nuget.org/packages/OllamaSharp/).

_The following example requires [OllamaSharp](https://www.nuget.org/packages/OllamaSharp/) package to be installed._

Demonstration of the default workspace configuration:

```csharp
[DependsOn(typeof(AbpAIModule))]
public class MyProjectModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpAIWorkspaceOptions>(options =>
        {
            options.Workspaces.ConfigureDefault(configuration =>
            {
                configuration.ConfigureChatClient(chatClientConfiguration =>
                {
                    chatClientConfiguration.Builder = new ChatClientBuilder(
                        sp => new OllamaApiClient("http://localhost:11434", "mistral")
                    );
                });
            });
        });
    }
}
```

Demonstration of the isolated workspace configuration:

```csharp
[DependsOn(typeof(AbpAIModule))]
public class MyProjectModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpAIWorkspaceOptions>(options =>
        {
            options.Workspaces.Configure<CustomerSupport>(configuration =>
            {
                configuration.ConfigureChatClient(chatClientConfiguration =>
                {
                    chatClientConfiguration.Builder = new ChatClientBuilder(
                        sp => new OllamaApiClient("http://localhost:11434", "mistral")
                    );
                });
            });
        });
    }
}
```

## See Also

- [Usage of Microsoft.Extensions.AI](./microsoft-extensions-ai.md)
- [Usage of Semantic Kernel](./microsoft-semantic-kernel.md)
- [Microsoft Agent Framework Overview](https://learn.microsoft.com/en-us/agent-framework/overview/agent-framework-overview)
- [AI Samples for .NET](https://learn.microsoft.com/en-us/samples/dotnet/ai-samples/ai-samples/)