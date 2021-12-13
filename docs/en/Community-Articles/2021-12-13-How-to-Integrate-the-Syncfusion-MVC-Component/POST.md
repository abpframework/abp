# How to Integrate the Syncfusion MVC Components to the ABP MVC UI?

## Introduction

Hi, in this step by step article we will see how we can integrate the Syncfusion MVC Components to our ABP MVC UI.


## Prerequisites

* [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

    * In this article, I will create a new startup template in v5.0.0-rc.2 and if you follow this article from top to end and create a new startup template with me, you need to install the [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) before starting.


Also update your ABP CLI to the v5.0.0-rc.2, you can use the below command to update your CLI version:

```bash
dotnet tool update Volo.Abp.Cli -g --version 5.0.0-rc.2
```

or install if you haven't installed before:

```bash
dotnet tool install Volo.Abp.Cli -g --version 5.0.0-rc.2
```

## Creating the Solution

In this article, I will create a new startup template with EF Core as a database provider and MVC for UI framework. But if you already have a project with MVC UI, you don't need to create a new startup template, you can directly implement the following steps to your existing project.

> If you already have a project, you can skip this section and starts to follow from TODO: step x.

We can create a new startup template by using the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI):

```bash
abp new SyncfusionComponentsDemo -t app --preview
```

Our project boilerplate will be ready after the download is finished. Then, we can open the solution and start developing.

## Starting the Development

