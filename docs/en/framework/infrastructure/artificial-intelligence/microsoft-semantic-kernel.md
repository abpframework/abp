# Microsoft.SemanticKernel
[Microsoft.SemanticKernel](https://learn.microsoft.com/en-us/semantic-kernel/overview/) is a library that provides a unified SDK for integrating AI services. This documentation is about the usage of this library with ABP Framework. Make sure you have read the [Artificial Intelligence](./index.md) documentation before reading this documentation.

## Usage

Semantic Kernel can be used by resolving `IKernelAccessor` service that carries the `Kernel` instance. Kernel might be `null` if no workspace is configured. You should check the kernel before using it.

```csharp
public class MyService
{
    private readonly IKernelAccessor _kernelAccessor;
    public MyService(IKernelAccessor kernelAccessor)
    {
        _kernelAccessor = kernelAccessor;
    }

    public async Task<string> GetResponseAsync(string prompt)
    {
        var kernel = _kernelAccessor.Kernel;
        if (kernel is null)
        {
            return "No kernel configured";
        }
        return await kernel.InvokeAsync(prompt);
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

You can resolve generic versions of `IKernelAccessor` service for a specific workspace as generic arguments. If Kernel is not configured for a workspace, you will get `null` from the accessor service. You should check the accessor before using it. This applies only for specified workspaces. Another workspace may have a configured Kernel.


`IKernelAccessor<TWorkSpace>` can be resolved to access a specific workspace's kernel. This is a typed kernel accessor and each workspace can have its own kernel configuration.

Example of resolving a typed kernel accessor:
```csharp
public class MyService
{
    private readonly IKernelAccessor<CommentSummarization> _kernelAccessor;
}
    public async Task<string> GetResponseAsync(string prompt)
    {
        var kernel = _kernelAccessor.Kernel;
        if (kernel is null)
        {
            return "No kernel configured";
        }
        return await kernel.InvokeAsync(prompt);
    }
}
```

## Configuration

`AbpAIWorkspaceOptions` configuration is used to configure AI workspaces and their configurations. You can configure the default workspace and also configure isolated workspaces by using the this options class.It has to be configured **before the services are configured** in the `PreConfigure` method of your module class. It is important since the services are registered after the configuration is applied.

- `AbpAIWorkspaceOptions` has a `Workspaces` property that is type of `WorkspaceConfigurationDictionary` which is a dictionary of workspace names and their configurations. It provides `Configure<T>` and `ConfigureDefault` methods to configure the default workspace and also configure isolated workspaces by using the workspace type.

- Configure method passes `WorkspaceConfiguration` object to the configure action. You can configure the `Kernel` by using the `ConfigureKernel` method.

- `ConfigureKernel()` method passes `KernelConfiguration` parameter to the configure action. You can configure the `Builder` and `BuilderConfigurers` by using the `ConfigureBuilder` method.
  - `Builder` is set once and is used to build the `Kernel` instance.
  - `BuilderConfigurers` is a list of actions that are applied to the `Builder` instance for incremental changes.These actions are executed in the order they are added.

To configure a kernel, you'll need a kernel connector package such as [Microsoft.SemanticKernel.Connectors.OpenAI](Microsoft.SemanticKernel.Connectors.OpenAI) to configure a kernel to use a specific LLM provider.

_The following example requires [Microsoft.SemanticKernel.Connectors.AzureOpenAI](Microsoft.SemanticKernel.Connectors.AzureOpenAI) package to be installed._

Demonstration of the default workspace configuration:
```csharp
[DependsOn(typeof(AbpAIModule))]
public class MyProjectModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpAIOptions>(options =>
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

Demonstration of the isolated workspace configuration:
```csharp
[DependsOn(typeof(AbpAIModule))]
public class MyProjectModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpAIOptions>(options =>
        {
            options.Workspaces.Configure<CommentSummarization>(configuration =>
            {
                configuration.ConfigureKernel(kernelConfiguration =>
                {
                    kernelConfiguration.Builder = Kernel.CreateBuilder()
                        .AddAzureOpenAIChatClient("...", "...");
                });
            });
        });
    }
}
```

## See Also

- [Usage of Microsoft.Extensions.AI](./microsoft-extensions-ai.md)
- [AI Samples for .NET](https://learn.microsoft.com/en-us/samples/dotnet/ai-samples/ai-samples/)