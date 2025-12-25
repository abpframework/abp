```json
//[doc-seo]
{
    "Description": "Explore ABP Framework's AI integration, enabling seamless AI capabilities, workspace management, and reusable modules for .NET developers."
}
```

# Artificial Intelligence (AI)
ABP Framework provides integration for AI capabilities to your application by using Microsoft's popular AI libraries. The main purpose of this integration is to provide a consistent and easy way to use AI capabilities and manage different AI providers, models and configurations in a single application.

ABP introduces a concept called **AI Workspace**. A workspace allows you to configure isolated AI configurations for a named scope. You can then resolve AI services for a specific workspace when you need to use them.

> ABP Framework can work with any AI library or framework that supports .NET development. However, the AI integration features explained in the following documents provide a modular and standard way to work with AI, which allows ABP developers to create reusable modules and components with AI capabilities in a standard way.

## Installation

Use the [ABP CLI](../../../cli/index.md) to install the [Volo.Abp.AI](https://www.nuget.org/packages/Volo.Abp.AI) NuGet package into your project. Open a command line window in the root directory of your project (`.csproj` file) and type the following command:

```bash
abp add-package Volo.Abp.AI
```

*For different installation options, check [the package definition page](https://abp.io/package-detail/Volo.Abp.AI).*

## Usage

The `Volo.Abp.AI` package provides integration with the following libraries:

* [Microsoft.Extensions.AI](https://learn.microsoft.com/en-us/dotnet/ai/microsoft-extensions-ai)
* [Microsoft.Agents.AI (Agent Framework)](https://learn.microsoft.com/en-us/agent-framework/overview/agent-framework-overview)
* [Microsoft.SemanticKernel](https://learn.microsoft.com/en-us/semantic-kernel/overview/)

The **Microsoft.Extensions.AI** library is suggested for library developers to keep the library dependency minimum and simple (since it provides basic abstractions and fundamental AI provider integrations). For applications, **Microsoft Agent Framework** is the recommended choice as it combines the best of both AutoGen and Semantic Kernel (it's direct successor of these two frameworks), offering simple abstractions for single- and multi-agent patterns along with advanced features like thread-based state management, type safety, filters, and telemetry. **Semantic Kernel** can still be used if you need its specific AI integration features.

Check the following documentation to learn how to use these libraries with the ABP integration:

- [ABP Microsoft.Extensions.AI integration](./microsoft-extensions-ai.md)
- [ABP Microsoft.Agents.AI (Agent Framework) integration](./microsoft-agent-framework.md)
- [ABP Microsoft.SemanticKernel integration](./microsoft-semantic-kernel.md)

