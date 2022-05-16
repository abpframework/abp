# Application (Single Layer) Startup Template

## Introduction

This template provides a single-layered application for quick start with ABP Framework. 

This document explains the **solution structure** and project in details. If you want to start quickly, you can follow the guides below:

<!-- TODO: Add quick start tutorial link!!! (https://github.com/abpframework/abp/issues/12273) -->

### What is the Difference Between the Application Startup Template?

ABP's [Application Startup Template](Application.md) provides a well-organized and layered solution to create maintainable business applications based on the [Domain Driven Design](../Domain-Driven-Design.md) (DDD) practises. However, some developers find this template little bit complex (or unneccessary) for simple and short-time applications.

In that point, for such applications a single-layered application template is created. Time template has the same functionality, features and modules on runtime with the [Application Startup Template](Application.md) but the development model is minimal and everything is in the single project (`.csproj`).

## How to Start with?

You can use the [ABP CLI](../CLI.md) to create a new project using this startup template. Alternatively, you can directly create & download from the [Get Started](https://abp.io/get-started) page. How to download via CLI is explained in this section.

First, install the ABP CLI if you haven't installed before:

```bash
dotnet tool install -g Volo.Abp.Cli
```

Then, use the `abp new` command in an empty folder to create a new solution:

```bash
abp new Acme.BookStore -t app-nolayers
```

* `Acme.BookStore` is the solution name, like *YourCompany.YourProduct*. You can use single level, two-levels or three-levels naming.
* This example specifies the template name (`-t` or `--template` option).

### Specify the UI Framework

This template provides multiple UI frameworks:

* `mvc`: ASP.NET Core MVC UI with Razor Pages
* `blazor-server`: Blazor Server UI
* `angular`: Angular UI
* `none`: Without UI <!-- TODO: try this!!! -!>

> Currently, this template doesn't have Blazor WASM UI, because it requries 3 projects at least (server-side, UI and shared library between these two projects). We'll consider to add Blazor WASM UI support based on your feedback.

Use the `-u` or `ui` option to specify the UI framework, while creating the solution:

```bash
abp new Acme.BookStore -u angular
```

* This example specifies the UI type (`-u` option) as `angular`, you can also specify the `mvc` or `blazor-server` for UI type.

### Specify the Database Provider

This template supports the following database providers:

- `ef`: Entity Framework Core (default)
- `mongodb`: MongoDB

Use the `-d` (or `--database-provider`) option to specify the database provider while creating the solution:

```bash
abp new Acme.BookStore -d mongodb
```

## Solution Structure