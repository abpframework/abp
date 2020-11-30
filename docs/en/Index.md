# ABP Documentation

ABP Framework is a complete **infrastructure** based on the **ASP.NET Core** to create **modern web applications** and **APIs** by following the software development **best practices** and the **latest technologies**.

## Getting Started

* [Getting Started Guide](Getting-Started.md) is the easiest way to start a new web application with the ABP Framework.
* [Web Application Development Tutorial](Tutorials/Part-1.md) is a complete tutorial to develop a full stack web application.

### UI Framework Options

<img width="500" src="images/ui-options.png">

### Database Provider Options

<img width="500" src="images/db-options.png">

## Exploring the Documentation

ABP has a **comprehensive documentation** that not only explains the ABP Framework, but also includes **guides** and **samples** to help you on creating a **maintainable solution** by introducing and discussing common **software development principle and best practices**.

### Architecture

ABP offers a complete, modular and layered software architecture based on [Domain Driven Design](Domain-Driven-Design.md) principles and patterns. It also provides the necessary infrastructure to implement this architecture.

* See the [Modularity](Module-Development-Basics.md) document to understand the module system.
* [Implementing Domain Driven Design](Domain-Driven-Design-Implementation-Guide.md) document is an ultimate guide for who want to understand and implement the DDD.
* [Microservice Architecture](Microservice-Architecture.md) document explains how ABP helps to create a microservice solution.

### Infrastructure

There are a lot of features provided by the ABP Framework to achieve real world scenarios easier, like [Event Bus](Event-Bus.md), [Background Job System](Background-Jobs.md), [Audit Logging](Audit-Logging.md), [BLOB Storing](Blob-Storing.md), [Data Seeding](Data-Seeding.md), [Data Filtering](Data-Filtering.md).

### Cross Cutting Concerns

ABP also simplifies (and even automates wherever possible) cross cutting concerns and common non-functional requirements like [Exception Handling](Exception-Handling.md), [Validation](Validation.md), [Authorization](Authorization.md), [Localization](Localization.md), [Caching](Caching.md), [Dependency Injection](Dependency-Injection.md), [Setting Management](Settings.md), etc. 

### Application Modules

Application Modules provides pre-built application functionalities;

* [**Account**](Modules/Account.md): Provides UI for the account management and allows user to login/register to the application.
* **[Identity](Modules/Identity.md)**: Manages organization units, roles, users and their permissions, based on the Microsoft Identity library.
* [**IdentityServer**](Modules/IdentityServer.md): Integrates to IdentityServer4.
* [**Tenant Management**](Modules/Tenant-Management.md): Manages tenants for a [multi-tenant](Multi-Tenancy.md) (SaaS) application.

See the [Application Modules](Modules/Index.md) document for all pre-built modules.

### Startup Templates

The [Startup templates](Startup-Templates/Index.md) are pre-built Visual Studio solution templates. You can create your own solution based on these templates to **immediately start your development**.

## ABP Community

### The Source Code

ABP is hosted on GitHub. See [the source code](https://github.com/abpframework).

### ABP Community Web Site

The [ABP Community](https://community.abp.io/) is a website to publish articles and share knowledge about the ABP Framework. You can also create content for the community!

### Blog

Follow the [ABP Blog](https://blog.abp.io/) to learn the latest happenings in the ABP Framework.

### Samples

See the [sample projects](Samples/Index.md) built with the ABP Framework.

### Want to Contribute?

ABP is a community-driven open source project. See [the contribution guide](Contribution/Index.md) if you want to be a part of this project.
