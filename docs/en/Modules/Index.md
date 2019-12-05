# Application Modules

ABP is a **modular application framework** which consists of dozens of **nuget packages**. It also provides a complete infrastructure to build your own application modules which may have entities, services, database integration, APIs, UI components and so on.

There are **two types of modules.** They don't have any structural difference but categorized by functionality and purpose:

* [**Framework modules**](https://github.com/abpframework/abp/tree/master/framework/src): These are **core modules of the framework** like caching, emailing, theming, security, serialization, validation, EF Core integration, MongoDB integration... etc. They do not have application/business functionalities but makes your daily development easier by providing common infrastructure, integration and abstractions.
* [**Application modules**](https://github.com/abpframework/abp/tree/master/modules): These modules implement specific application/business functionalities like blogging, document management, identity management, tenant management... etc. They generally have their own entities, services, APIs and UI components.

## Open Source Application Modules

There are some **free and open source** application modules developed and maintained by the ABP community:

* **Account**: Used to make user login/register to the application.
* **Audit Logging**: Used to persist audit logs to a database.
* **Background Jobs**: Used to persist background jobs when using default background job manager.
* **Blogging**: Used to create fancy blogs. ABP's [own blog](https://abp.io/blog/abp/) already using this module.
* [**Docs**](Docs.md): Used to create technical documentation pages. ABP's [own documentation](https://docs.abp.io) already using this module.
* **Identity**: Used to manage roles, users and their permissions.
* **Identity Server**: Integrates to IdentityServer4.
* **Permission Management**: Used to persist permissions.
* **[Setting Management](Setting-Management.md)**: Used to persist and manage the [settings](../Settings.md).
* **Tenant Management**: Used to manage tenants for a [multi-tenant](../Multi-Tenancy.md) application.
* **Users**: Used to abstract users, so other modules can depend on this instead of the Identity module.

Documenting the modules is in the progress. See [this repository](https://github.com/abpframework/abp/tree/master/modules) for source code of all modules.
