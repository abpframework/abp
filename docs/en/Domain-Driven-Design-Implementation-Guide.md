# Implementing Domain Driven Design

## Introduction

This is a **practical guide** for implementing the Domain Driven Design (DDD). While the implementation details relies on the ABP Framework infrastructure, core concepts, principles and patterns are applicable in any kind of solution, even if it is not a .NET solution.

### Goals

The goals of this document are;

* **Introduce and explain** the DDD architecture, concepts, principles, patterns and building blocks.
* Explain the **layered architecture** & solution structure offered by the ABP Framework.
* Introduce **explicit rules** to implement DDD patterns and best practices by giving **concrete examples**.
* Show what **ABP Framework provides** you as the infrastructure for implementing DDD in a proper way.

### Simple Code!

> **Playing football** is very **simple**, but **playing simple football** is the **hardest thing** there is.
> &mdash; <cite>Johan Cruyff</cite>

If we take this famous quote for programming, we can say;

> **Writing code** is very **simple**, but **writing simple code** is the **hardest thing** there is.
> &mdash; <cite>???</cite>

In this document, we will introduce **simple rules** those are **easy to implement**.

Once your **application grows**, it will be **hard to follow** these rules. Sometimes you find **breaking rules** will save your time in a short term. However, the saved time in the short term will bring much **more time loss** in the middle and long term. Your code base becomes **complicated** and hard to maintain. Most of the business applications are **re-written** just because you **can't maintain** it anymore.

If you **follow the rules and best practices**, your code base will be simpler and easier to maintain. Your application **react to changes** faster.

## What is the Domain Driven Design?

Domain-driven design (DDD) is an approach to software development for **complex** needs by connecting the implementation to an **evolving** model;

DDD is suitable for **complex domains** and **large-scale** applications rather than simple CRUD applications. It focuses on the **core domain logic** rather than the infrastructure details. It helps to build a **flexible**, modular and **maintainable** code base.

### OOP & SOLID

Implementing DDD highly relies on the Object Oriented Programming (OOP) and [SOLID](https://en.wikipedia.org/wiki/SOLID) principles. Actually, it **implements** and **extends** these principles. So, a **good understanding** of OOP & SOLID helps you a lot while truly implementing the DDD.

### DDD Layers & Clean Architecture

There are four fundamental layers of a Domain Driven Based Solution;

![domain-driven-design-layers](images/domain-driven-design-layers.png)

**Business Logic** places into two layers, the *Domain layer* and the *Application Layer*, while they contains different kinds of business logic;

* **Domain Layer** implements the core, use-case independent business logic of the domain/system.
* **Application Layer** implements the use cases of the application based on the domain. A use case can be thought as a user interaction on the User Interface (UI).
* **Presentation Layer** contains the UI elements (pages, components) of the application.
* **Infrastructure Layer** supports other layer by implementing the abstractions and integrations to 3rd-party library and systems.

The same layering can be shown as the diagram below and known as the **Clean Architecture**, or sometimes the **Onion Architecture**:

![domain-driven-design-clean-architecture](images/domain-driven-design-clean-architecture.png)

In the Clean Architecture, each layer only **depends on the layer directly inside it**. The most independent layer is shown in the most inner circle and it is the Domain Layer.

### Core Building Blocks

DDD mostly **focuses on the Domain & Application Layers** and ignores the Presentation and Infrastructure. They are seen as *details* and the business layers should not depend on them.

That doesn't mean the Presentation and Infrastructure layers are not important. They are very important. UI frameworks and database providers have their own rules and best practices that you need to know and apply. However these are not in the topics of DDD.

This section introduces the essential building blocks of the Domain & Application Layers.

#### Domain Layer Building Blocks

* **Entity**: An [Entity](Entities.md) is an object with its own properties (state, data) and methods that implements the business logic that is executed on these properties. An entity is represented by its unique identifier (Id). Two entity object with different Ids are considered as different entities.
* **Value Object**: A [Value Object](Value-Objects.md) is another kind of domain object that is identified by its properties rather than a unique Id. That means two Value Objects with same properties are considered as the same object. Value objects are generally implemented as immutable and mostly are much simpler than the Entities.
* **Aggregate & Aggregate Root**: An [Aggregate](Entities.md) is a cluster of objects (entities and value objects) bound together by an **Aggregate Root** object. The Aggregate Root is a specific type of an entity with some additional responsibilities.
* **Repository** (interface): A [Repository](Repositories.md) is a collection-like interface that is used by the Domain and Application Layers to access to the data persistence system (the database). It hides the complexity of the DBMS from the business code. Domain Layer contains the `interface`s of the repositories.
* **Domain Service**: A [Domain Service](Domain-Services.md) is a stateless service that implements core business rules of the domain. It is useful to implement domain logic that depends on multiple aggregate (entity) type or some external services.
* **Specification**: A [Specification](Specifications.md) is used to define named, reusable and combinable filters for entities and other business objects.
* **Domain Event**: A [Domain Event](Event-Bus.md) is a way of informing other services in a loosely coupled manner, when a domain specific event occurs.

#### Application Layer Building Blocks

* **Application Service**: An [Application Service](Application-Services.md) is a stateless service that implements use cases of the application. An application service typically gets and returns DTOs. It is used by the Presentation Layer. It uses and coordinates the domain objects to implement the use cases. A use case is typically considered as a Unit Of Work.
* **Data Transfer Object (DTO)**: A [DTO](Data-Transfer-Objects.md) is a simple object without any business logic that is used to transfer state (data) between the Application and Presentation Layers.
* **Unit of Work (UOW)**: A [Unit of Work](Unit-Of-Work.md) is an atomic work that should be done as a transaction unit. All the operations inside a UOW should be committed on success or rolled back on a failure.

## Implementation: The Big Picture

### Layering of a .NET Solution

The picture below shows a Visual Studio Solution created using the ABP's [application startup template](Startup-Templates/Application.md):

![domain-driven-design-vs-solution](images/domain-driven-design-vs-solution.png)

The solution name is `IssueTracking` and it consists of multiple projects. The solution is layered by considering **DDD principles** as well as **development** and **deployment** practicals. The sub sections below explains the projects in the solution;

#### The Domain Layer

The Domain Layer is splitted into two projects;

* `IssueTracking.Domain` is the **essential domain layer** that contains all the **building blocks** (entities, value objects, domain services, specifications, repository interfaces, etc.) introduced before.
* `IssueTracking.Domain.Shared` is a thin project that contains some types those belong to the Domain Layer, but shared with all other layers. For example, it may contain some constants and `enum`s related to the Domain Objects but need to be **reused by other layers**.

#### The Application Layer

The Application Layer is also splitted into two projects;

* `IssueTracking.Application.Contracts` contains the application service **interfaces** and the **DTO**s used by these interfaces. This project can be shared by the client applications (including the UI).
* `IssueTracking.Application` is the **essential application layer** that **implements** the interfaces defined in the Contracts project.

#### The Presentation Layer

* `IssueTracking.Web` is an ASP.NET Core MVC / Razor Pages application for this example.

> ABP Framework also supports different kind of UI frameworks including [Angular](UI/Angular/Quick-Start.md) and [Blazor](UI/Blazor/Overall.md). In these cases, the `IssueTracking.Web` doesn't exist in the solution. Instead, an `IssueTracking.HttpApi.Host` application will be in the solution to serve the HTTP APIs as a standalone endpoint to be consumed by the UI applications via HTTP API calls.

#### The Remote Service Layer

* `IssueTracking.HttpApi` project contains HTTP APIs defined by the solution. It typically contains MVC `Controller`s and related models, if available. So, you write your HTTP APIs in this project.

> Most of the time, API Controllers are just wrappers around the Application Services to expose them to the remote clients. Since ABP Framework's [Automatic API Controller System](API/Auto-API-Controllers.md) **automatically configures and exposes your Application Services as API Controllers**, you typically don't create Controllers in this project. However, the startup solution includes it for the cases you need to manually create API controllers.

* `IssueTracking.HttpApi.Client` project is useful when you have a C# application that needs to consume your HTTP APIs. Once the client application references this project, it can directly [inject](Dependency-Injection.md) & use the Application Services. This is possible by the help of the ABP Framework's [Dynamic C# Client API Proxies System](API/Dynamic-CSharp-API-Clients.md).

> There is a Console Application in `test` folder the solution, named `IssueTracking.HttpApi.Client.ConsoleTestApp`. It simply uses the `IssueTracking.HttpApi.Client` project to consume the APIs exposed by the application. It is just a demo application and you can safely delete it. You can even delete the `IssueTracking.HttpApi.Client` project if you think that you don't need to them.

#### The Infrastructure Layer

In a DDD implementation, you may have a single Infrastructure project to implement all the abstractions and integrations, or you may have different projects for each dependency.

We suggest a balanced approach; Create separate projects for main infrastructure dependencies (like Entity Framework Core) and a common infrastructure project for other infrastructure.

ABP's startup solution has two projects for the Entity Framework Core integration;

* `IssueTracking.EntityFrameworkCore` is the essential integration package for the EF Core. Your application's `DbContext`, database mappings and other EF Core related stuff are located here.
* `IssueTracking.EntityFrameworkCore.DbMigrations` is a special project to manage the Code First database migrations. There is a separate `DbContext` in this project to track the migrations. You typically don't touch this project much except you need to create a new database migration or add an [application module](Modules/Index.md) that has some database tables and naturally requires to create a new database migration.

> You may wonder why there are two projects for the EF Core. It is mostly related to [modularity](Module-Development-Basics.md). Each module has its own independent `DbContext` and your application has also one `DbContext`. `DbMigrations` project contains a **union** of the modules to track and apply a **single migration path**. While most of the times you don't need to know it, you can see the [EF Core migrations](Entity-Framework-Core-Migrations.md) document for more information. 

#### Other Projects

There is one more project, `IssueTracking.DbMigrator`, that is a simple Console Application that **migrates** the database schema and **[seeds](Data-Seeding.md) the initial** data when you execute it. It is a useful **utility application** that you can use it in development as well as in production environment.

### Dependencies of the Projects in the Solution

TODO

### Execution Flow a DDD Based Application

TODO

### Common Principles

TODO