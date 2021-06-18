# Application Modules

ABP is a **modular application framework** which consists of dozens of **NuGet & NPM packages**. It also provides a complete infrastructure to build your own application modules which may have entities, services, database integration, APIs, UI components and so on.

There are **two types of modules.** They don't have any structural difference but are categorized by functionality and purpose:

* [**Framework modules**](https://github.com/abpframework/abp/tree/master/framework/src): These are **core modules of the framework** like caching, emailing, theming, security, serialization, validation, EF Core integration, MongoDB integration... etc. They do not have application/business functionalities but makes your daily development easier by providing common infrastructure, integration and abstractions.
* [**Application modules**](https://github.com/abpframework/abp/tree/master/modules): These modules implement specific application/business functionalities like blogging, document management, identity management, tenant management... etc. They generally have their own entities, services, APIs and UI components.

## Open Source Application Modules

There are some **free and open source** application modules developed and maintained as a part of the ABP Framework.

* [**Account**](Account.md): Provides UI for the account management and allows user to login/register to the application.
* [**Audit Logging**](Audit-Logging.md): Persists audit logs to a database.
* [**Background Jobs**](Background-Jobs.md): Persist background jobs when using the default background job manager.
* [**CMS Kit**](Cms-Kit/Index.md): A set of reusable *Content Management System* features.
* [**Docs**](Docs.md): Used to create technical documentation website. ABP's [own documentation](https://docs.abp.io) already using this module.
* [**Feature Management**](Feature-Management.md): Used to persist and manage the [features](../Features.md).
* **[Identity](Identity.md)**: Manages organization units, roles, users and their permissions, based on the Microsoft Identity library.
* [**IdentityServer**](IdentityServer.md): Integrates to IdentityServer4.
* [**Permission Management**](Permission-Management.md): Used to persist permissions.
* **[Setting Management](Setting-Management.md)**: Used to persist and manage the [settings](../Settings.md).
* [**Tenant Management**](Tenant-Management.md): Manages tenants for a [multi-tenant](../Multi-Tenancy.md) application.
* [**Virtual File Explorer**](Virtual-File-Explorer.md): Provided a simple UI to view files in [virtual file system](../Virtual-File-System.md).

See [the GitHub repository](https://github.com/abpframework/abp/tree/master/modules) for source code of all modules.

## Commercial Application Modules

[ABP Commercial](https://commercial.abp.io/) license provides **additional pre-built application modules** on top of the ABP framework. See the [module list](https://commercial.abp.io/modules) provided by the ABP Commercial.