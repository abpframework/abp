# MVC Application Startup Template

## Introduction

This template provides a layered (or tiered, based on the preference) application structure based on the [Domain Driven Design](../Domain-Driven-Design.md) (DDD) practices.

## How to Start With

You can use the [ABP CLI](../CLI.md) to create a new project using this startup template. Alternatively, you can directly create & download from the [Get Started](https://abp.io/get-started) page. CLI approach is used here.

First, install the ABP CLI as described in [its document](../CLI.md). Then use the `abp new` command in an empty folder to create a new solution:

````bash
abp new Acme.BookStore -t mvc
````

* `Acme.BookStore` is the solution name, like *YourCompany.YourProduct*. See the [CLI document](../CLI.md) for different naming styles.
* This example specified the template name (`-t` or `--template` option). However, `mvc` is the default template and used even if you don't specify it.

### Specify Database Provider

This template supports the following database providers:

- `ef`: Entity Framework Core (default)
- `mongodb`: MongoDB

Use the `-d` (or `--database-provider`) to specify the database provider:

````bash
abp new Acme.BookStore -t mvc -d mongodb
````

### Create a Tiered Solution

`--tiered` option is used to create a tiered solution where Web and Http API layers are physically separated. If not specified, it creates a layered solution which is less complex and suitable for most scenarios.

````bash
abp new Acme.BookStore --tiered
````

## Solution Structure

Based on the options you've specified, you will get a slightly different solution structure.

### Default Structure

If you don't specify any option, you will have a solution like shown below:

![bookstore-visual-studio-solution-v3](../images/bookstore-visual-studio-solution-v3.png)

Projects are organized in `src` and `test` folders. `src` folder contains the actual application which is layered based on [DDD](../Domain-Driven-Design.md) principles as mentioned before.

--------------------

**TODO: Add a graphic to illustrate dependencies between projects.**

------------------

Each section below will explain the related project.

#### .Domain Project

This is the domain layer of the solution. It mainly contains [entities, aggregate roots](../Entities.md), [domain services](../Domain-Services.md), [value types](../Value-Types.md), [repository interfaces](../Repositories.md) and other domain objects of the solution.

A `Book` entity and a `IBookRepository` interface are good candidates for this project.

* Depends on the `.Domain.Shared` because it uses constants, enums and other objects defined in that project.

#### .Domain.Shared Project

This project contains constants, enums and other objects these are actually a part of the domain layer, but needed to be used by all layers/projects in the solution.

A `BookType` enum and a `BookConts` class (which may have some constant fields for the `Book` entity, like `MaxNameLength`) are good candidates for this project.

This project has no dependency to other projects in the solution.

#### .Application.Contracts Project

This project mainly contains [application service](../Application-Services.md) **interfaces** and [Data Transfer Objects](../Data-Transfer-Objects.md) (DTO) of the application layer. It does exists to separate interface & implementation of the application layer. In this way, the interface project can be shared to the clients as a contract package.

* Depends on the `.Domain.Shared` because it may use constants, enums and other shared objects in the application service interfaces and DTOs.

#### .Application Project

This project contains the [application service](../Application-Services.md) implementations of the interfaces defined in the `.Application.Contracts` project.

* Depends on the `.Application.Contracts` project to be able to implement the interfaces and use the DTOs.
* Depends on the `.Domain` project to be able to use domain objects (entities, repository interfaces... etc.) to perform the application logic.

#### .EntityFrameworkCore Project.

