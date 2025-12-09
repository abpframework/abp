# Microsoft.Extensions.AI
[Microsoft.Extensions.AI](https://learn.microsoft.com/en-us/dotnet/ai/microsoft-extensions-ai) is a library that provides a unified API for integrating AI services. It is a part of the Microsoft AI Extensions Library. It is used to integrate AI services into your application. This documentation is about the usage of this library with ABP Framework. Make sure you have read the [Artificial Intelligence](./index.md) documentation before reading this documentation.

## Usage

You can resolve `IChatClient` to access configured chat client from your service and use it directly.

```csharp
public class MyService
{
    private readonly IChatClient _chatClient;
    public MyService(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<string> GetResponseAsync(string prompt)
    {
        return await _chatClient.GetResponseAsync(prompt);
    }
}
```

You can also resolve `IChatClientAccessor` to access the `IChatClient` optionally configured scenarios such as developing a module or a service that may use AI capabilities **optionally**.


```csharp
public class MyService
{
    private readonly IChatClientAccessor _chatClientAccessor;
    public MyService(IChatClientAccessor chatClientAccessor)
    {
        _chatClientAccessor = chatClientAccessor;
    }

    public async Task<string> GetResponseAsync(string prompt)
    {
        var chatClient = _chatClientAccessor.ChatClient;
        if (chatClient is null)
        {
            return "No chat client configured";
        }
        return await chatClient.GetResponseAsync(prompt);
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


Example of resolving a typed chat client:
```csharp
public class MyService
{
    private readonly IChatClient<CommentSummarization> _chatClient;

    public MyService(IChatClient<CommentSummarization> chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<string> GetResponseAsync(string prompt)
    {
        return await _chatClient.GetResponseAsync(prompt);
    }
}
```

Example of resolving a typed chat client accessor:
```csharp
public class MyService
{
    private readonly IChatClientAccessor<CommentSummarization> _chatClientAccessor;
}
    public async Task<string> GetResponseAsync(string prompt)
    {
        var chatClient = _chatClientAccessor.ChatClient;
        if (chatClient is null)
        {
            return "No chat client configured";
        }
        return await chatClient.GetResponseAsync(prompt);
    }
}
```

## Configuration

`AbpAIWorkspaceOptions` configuration is used to configure AI workspaces and their configurations. You can configure the default workspace and also configure isolated workspaces by using the this options class.It has to be configured **before the services are configured** in the `PreConfigure` method of your module class. It is important since the services are registered after the configuration is applied.

- `AbpAIWorkspaceOptions` has a `Workspaces` property that is type of `WorkspaceConfigurationDictionary` which is a dictionary of workspace names and their configurations. It provides `Configure<T>` and `ConfigureDefault` methods to configure the default workspace and also configure isolated workspaces by using the workspace type.

- Configure method passes `WorkspaceConfiguration` object to the configure action. You can configure the `ChatClient` by using the `ConfigureChatClient` method.

- `ConfigureChatClient()` method passes `ChatClientConfiguration` parameter to the configure action. You can configure the `Builder` and `BuilderConfigurers` by using the `ConfigureBuilder` method.
  - `Builder` is set once and is used to build the `ChatClient` instance.
  - `BuilderConfigurers` is a list of actions that are applied to the `Builder` instance for incremental changes.These actions are executed in the order they are added.

To configure a chat client, you'll need a LLM provider package such as [Microsoft.Extensions.AI.OpenAI](https://www.nuget.org/packages/Microsoft.Extensions.AI.OpenAI) or [OllamaSharp](https://www.nuget.org/packages/OllamaSharp/) to configure a chat client.

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
            options.Workspaces.Configure<CommentSummarization>(configuration =>
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

- [Usage of Semantic Kernel](./microsoft-semantic-kernel.md)
- [AI Samples for .NET](https://learn.microsoft.com/en-us/samples/dotnet/ai-samples/ai-samples/)