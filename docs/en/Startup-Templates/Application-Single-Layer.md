# Application (Single Layer) Startup Template

## Introduction

This template provides a single-layered application for a quick start with ABP Framework. 

This document explains the **solution structure** and project in detail.

### What is the Difference Between the Application Startup Template?

ABP's [Application Startup Template](Application.md) provides a well-organized and layered solution to create maintainable business applications based on the [Domain Driven Design](../Domain-Driven-Design.md) (DDD) practices. However, some developers find this template a little bit complex (or unnecessary) for simple and short-time applications.

At this point, a single-layer application template has been created for such applications. This template has the same functionality, features and modules on runtime with the [Application Startup Template](Application.md) but the development model is minimal and everything is in a single project (`.csproj`).

## How to Start with?

You can use the [ABP CLI](../CLI.md) to create a new project using this startup template. Alternatively, you can directly create & download this startup template from the [Get Started](https://abp.io/get-started) page. How to download via CLI is explained in this section.

Firstly, install the ABP CLI if you haven't installed it before:

```bash
dotnet tool install -g Volo.Abp.Cli
```

Then, use the `abp new` command in an empty folder to create a new solution:

```bash
abp new Acme.BookStore -t app-nolayers
```

* `Acme.BookStore` is the solution name, like *YourCompany.YourProduct*. You can use single-level, two-level or three-level naming.
* In this example, the `-t` option (or `--template` option) specifies the template name.
 
### Specify the UI Framework

This template provides multiple UI frameworks:

* `mvc`: ASP.NET Core MVC UI with Razor Pages (default)
* `blazor-server`: Blazor Server UI
* `angular`: Angular UI
* `none`: Without UI

> Currently, this template doesn't have Blazor WASM UI, because it requires 3 projects at least (server-side, UI and shared library between these two projects). 

Use the `-u` or `--ui` option to specify the UI framework while creating the solution:

```bash
abp new Acme.BookStore -t app-nolayers -u angular
```

* This example specifies the UI type (`-u` option) as `angular`, you can also specify `mvc` or `blazor-server` for UI type.

### Specify the Database Provider

This template supports the following database providers:

- `ef`: Entity Framework Core (default)
- `mongodb`: MongoDB

Use the `-d` (or `--database-provider`) option to specify the database provider while creating the solution:

```bash
abp new Acme.BookStore -t app-nolayers -d mongodb
```

## Solution Structure

If you don't specify any additional options while creating an `app-nolayers` template, you will have a solution like shown below:

![](../images/bookstore-single-layer-solution-structure.png)

It's a single-layer template rather than a layered-architecture solution (like the `Application Startup Template`). It's helpful to create a running application quickly without considering the layered architecture.

### Folder Structure

Since this template provides single-layer solution, we've separated concerns into folders instead of layers and you can see the pre-defined folders like shown below:

![](../images/single-layer-folder-structure.png)

* You can define your `entities`, `application services`, `DTOs`, etc. in this single project (in the related folders). 
* For example, you can define your `application services` and `DTOs` under the **Services** folder.

### How to Run?

Before running the application, you need to create the database and seed the initial data. To do that, you can run the following command in the directory of your project:

```bash
dotnet run --migrate-database
```

* This command will create the database and seed the initial data for you. Then you can run the application with any IDE that supports .NET or by running the `dotnet run` CLI command in the directory of your project. The default username is `admin` and the password is `1q2w3E*`.

> While creating a database & applying migrations seem only necessary for relational databases, this project comes even if you choose a NoSQL database provider (like MongoDB). In that case, it still seeds the initial data which is necessary for the application.

### Angular UI 

If you choose `Angular` as the UI framework, the solution is being separated into two folders:

* `angular` folder contains the Angular UI application, the client-side code.
* `aspnet-core` folder contains the ASP.NET Core solution (a single project), the server-side code.

The server-side is similar to the solution described in the [Solution Structure](#solution-structure) section above. This project serves the API, so the `Angular` application can consume it.

The client-side application consumes the HTTP APIs as mentioned. You can see the folder structure of the Angular project shown below:

![](../images/single-layer-angular-folder-structure.png)
