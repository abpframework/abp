```json
//[doc-seo]
{
    "Description": "Explore essential concepts and terms used in ABP Studio to enhance your understanding and streamline your development experience."
}
```

# ABP Studio Concepts

````json
//[doc-nav]
{
  "Next": {
    "Name": "Overview",
    "Path": "studio/overview"
  }
}
````

We use some concepts and terms in ABP Studio and the documentation that may not be clear when you first see. Some of them may seem new to you, or might be used in other meaning in different domains.

In this document, we are trying to clearly define these terms and concepts clearly, so you don't confuse.

## All Concepts and Terms

### Solution

*Typically referred as "ABP Studio Solution", or just "Solution".*

An ABP Studio solution is the most-top container that contains all the applications, modules and packages of your product or solution.

An ABP Studio solution may contain zero, one or many .NET solutions. For example, in a microservice solution, it may contain a separate .NET solution for each microservice, so you have multiple .NET solutions in your microservice solution. In such a scenario, each microservice (typically each separate .NET solution) will be a separate ABP Studio module.

### Module

*Typically referred as "ABP Studio Module", or just "Module".*

An ABP Studio module is a sub-solution that contains zero, one or multiple packages. A module typically have a corresponding .NET solution with one or multiple .NET projects (e.g. `csproj`). A .NET project is called as *Package* in ABP Studio.

### Package

*Typically referred as "ABP Studio Package", or just "Package".*

An ABP Studio Package typically matches to a .NET project (`csproj`).

### Metadata

Metadata is a collection of key-value pairs that provide additional information for various ABP Studio features. Metadata follows a hierarchical structure where values defined at lower levels override those at higher levels:

**Hierarchy (from highest to lowest priority):**
1. **Helm Chart Metadata** - Defined in chart properties (Kubernetes context only)
2. **Kubernetes Profile Metadata** / **Run Profile Metadata** - Defined in profile settings (context-dependent)
3. **Solution Metadata** - Defined via *Solution Explorer* → right-click solution → *Manage Metadata*
4. **Global Metadata** - Defined via *Tools* → *Global Metadata*

**Common Metadata Keys:**

| Key | Description | Used By |
|-----|-------------|---------|
| `k8ssuffix` | Appends a suffix to Kubernetes namespace (e.g., for multi-developer scenarios) | Kubernetes integration |
| `dotnetEnvironment` | Specifies the .NET environment (e.g., `Development`, `Staging`) | Helm chart installation |
| `projectPath` | Path to the project for Docker image building | Docker image build |
| `imageName` | Docker image name | Docker image build |
| `projectType` | Project type (`dotnet` or `angular`) | Docker image build |

> Metadata defined in *Global Metadata* is available for all solutions but will not be shared with team members. Metadata defined at *Solution* or *Profile* level will be shared through solution files.

### Secrets

Secrets are key-value pairs designed for storing sensitive information such as passwords, API keys, and connection strings. Unlike metadata, secrets are stored in the local file system and are not included in solution files for security reasons.

**Hierarchy (from highest to lowest priority):**
1. **Kubernetes Profile Secrets** / **Run Profile Secrets** - Defined in profile settings (context-dependent)
2. **Solution Secrets** - Defined via *Solution Explorer* → right-click solution → *Manage Secrets*
3. **Global Secrets** - Defined via *Tools* → *Global Secrets*

**Common Secret Keys:**

| Key | Description | Used By |
|-----|-------------|---------|
| `wireGuardPassword` | Password for WireGuard VPN connection to Kubernetes cluster | Kubernetes integration |

> Secrets are stored locally and are not shared with team members by default. Each developer needs to configure their own secrets.

## ABP Studio vs .NET Terms

Some ABP Studio terms may seem conflict with .NET and Visual Studio. To make them even more clear, you can use the following table.

| ABP Studio | .NET / Visual Studio   |
| ---------- | ---------------------- |
| Solution   | *- no matching term -* |
| Module     | Solution               |
| Package    | Project                |

In essence, ABP Studio uses the solution term to cover all the .NET solutions and other components of your product. ABP Studio is used to build and manage the relations of these multiple .NET solutions and provides a high-level view of the whole system.
