```json
//[doc-seo]
{
    "Description": "Prepare your development environment for ABP applications with essential prerequisites and IDE recommendations for .NET development."
}
```

# Prerequisites for Developing ABP Applications

This document will guide you through preparing your development environment for ABP based application development.

## Notices

The prerequisites mentioned in this document are not necessary for every project type;

* You don't need to install the EF Core CLI if your application uses MongoDB instead of EF Core.
* You don't need to install Helm, NGINX Ingress, or mkcert if you are developing a non-microservice application.

`README.MD` files in new solutions contain specific requirements for your solution. Please refer to the `README.MD` file of your solution.

## IDE

You need to use an IDE that supports .NET development. The following IDEs are the most popular ones for .NET development.

### Visual Studio

Visual Studio is Microsoft's IDE and is the de facto tool for developing .NET projects. You can download Visual Studio from the [Visual Studio official website](https://visualstudio.microsoft.com/). It also has a **free Community edition** which is more than enough for ABP projects.

### Visual Studio Code

Visual Studio Code is a **free and cross-platform** lightweight code editor that supports .NET development. You can [download from here](https://code.visualstudio.com/download).

### JetBrains Rider

[JetBrains Rider](https://www.jetbrains.com/rider/download) is a cross-platform IDE by [JetBrains](https://www.jetbrains.com/) that supports .NET development. It is **[free for non-commercial use](https://blog.jetbrains.com/blog/2024/10/24/webstorm-and-rider-are-now-free-for-non-commercial-use/)**.

## .NET SDK

ABP is based on NET, so you need to install the .NET SDK. You can download the .NET SDK from the [.NET official website](https://dotnet.microsoft.com/en-us/download/dotnet/9.0).

> Installing Visual Studio or JetBrains Rider may automatically install the .NET SDK.

### EF Core CLI

If you are using [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) as your database access provider, you need to install the [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet). You can install it by running the following command:

```bash
dotnet tool install --global dotnet-ef
```

If you have already installed the `EF Core CLI`, you can update it by running the following command:

```bash
dotnet tool update --global dotnet-ef
```

## Node.js

ABP projects include some frontend resource packages, so you need to install Node.js/NPM manage these resource packages. You can download Node.js from the [official Node.js website](https://nodejs.org/). We recommend installing version v20.11+.

## Yarn (Required Only for Angular Projects)

ABP Angular projects use Yarn as the package manager to manage frontend dependencies and run build tasks, You can download `Yarn` from the [Yarn official website](https://classic.yarnpkg.com/en/docs/install). We recommend installing Yarn v1.22+ (make sure to install the Classic version, not v2+).

To install Yarn using npm, run the following command:

```bash
npm install --global yarn
```

## Docker Engine or Docker Desktop

ABP's [Layered Solution](../solution-templates/layered-web-application/index.md) and [Microservice Solution](../solution-templates/microservice/index.md) use Docker to run infrastructure services (e.g. SQL Server, Redis, RabbitMQ) required by your application. You can install Docker Engine or Docker Desktop (recommended) on Windows, macOS and Linux.

* [Docker Desktop](https://www.docker.com/products/docker-desktop/) (recommended)
* [Docker Engine](https://docs.docker.com/engine/install/)

### Is Docker Engine or Docker Desktop Free?

Docker Engine is an open-source and free containerization technology for building and containerizing your applications. [`Docker Engine` follows the Apache License 2.0](https://docs.docker.com/engine/#licensing).

Docker Desktop is free [for small businesses (fewer than 250 employees and less than $10 million in annual revenue), personal use, education, and non-commercial open-source projects](https://docs.docker.com/subscription/desktop-license/).

## PowerShell

ABP startup solution templates and tools use some PowerShell scripts (`*.ps1`) to perform certain tasks. You can refer to the [PowerShell documentation](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell) for guidance on how to install PowerShell on Windows, macOS, and Linux.

* [Install PowerShell on Windows](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows)
* [Install PowerShell on macOS](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-macos)
* [Install PowerShell on Linux](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-linux)

## MicroService Solution

The following tools are only required to develop ABP's [microservice solution](../solution-templates/microservice/index.md)

### Helm

[Helm](https://helm.sh/) is a package manager for Kubernetes. You can install Helm by following the [Helm installation guide](https://helm.sh/docs/intro/install/). 

See [Helm Deployment on Local Kubernetes Cluster](../solution-templates/microservice/helm-charts-and-kubernetes.md) for more information.

### NGINX Ingress or NGINX Ingress using Helm

[NGINX Ingress](https://kubernetes.github.io/ingress-nginx/deploy/) is an Ingress controller for Kubernetes. You can install NGINX Ingress by following the [NGINX Ingress installation guide](https://kubernetes.github.io/ingress-nginx/deploy/). 

If you are using Helm, you can install NGINX Ingress using the following commands:

```cs
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update
helm upgrade --install --version=4.0.19 ingress-nginx ingress-nginx/ingress-nginx
```

### mkcert

Use mkcert to generate trusted certificates for local development. You can install mkcert by following the [official mkcert installation guide](https://github.com/FiloSottile/mkcert#installation).
