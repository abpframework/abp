# Application Modules

ABP is a **modular application framework** which consists of dozens of **NuGet & NPM packages**. It also provides a complete infrastructure to build your own application modules which may have entities, services, database integration, APIs, UI components and so on.

There are **two types of modules.** They don't have any structural difference but are categorized by functionality and purpose:

* [**Framework modules**](https://github.com/abpframework/abp/tree/dev/framework/src): These are **core modules of the framework** like caching, emailing, theming, security, serialization, validation, EF Core integration, MongoDB integration... etc. They do not have application/business functionalities but makes your daily development easier by providing common infrastructure, integration and abstractions.
* [**Application modules**](https://github.com/abpframework/abp/tree/dev/modules): These modules implement specific application/business functionalities like blogging, document management, identity management, tenant management... etc. They generally have their own entities, services, APIs and UI components.

## The Module List

Here are all the free and pro application modules developed and maintained as a part of the ABP platform:

* [**Account**](account.md): Provides UI for the account management and allows user to login/register to the application.
* **[Account (Pro)](account-pro.md)**: Login, register, forgot password, email activation, social logins and other account related functionalities.
* [**Audit Logging**](audit-logging.md): Persists audit logs to a database.
* **[Audit logging (Pro)](audit-logging-pro.md)**: Reporting the user audit logs and entity histories in details.
* [**Background Jobs**](background-jobs.md): Persist background jobs when using the default background job manager.
* **[Chat (Pro)](chat.md)**: Real time messaging between users of the application.
* [**CMS Kit**](cms-kit): A set of reusable *Content Management System* features.
* **[CMS Kit (Pro)](cms-kit-pro)**: A set of reusable CMS (Content Management System) building blocks.
* [**Docs**](docs.md): Used to create technical documentation website. ABP's [own documentation](../modules) already using this module.
* [**Feature Management**](feature-management.md): Used to persist and manage the [features](../framework/infrastructure/features.md).
* **[File Management (Pro)](file-management.md)**: Upload, download and organize files in a hierarchical folder structure.
* **[Forms (Pro)](forms.md)**: Create forms and surveys.
* **[GDPR (Pro)](gdpr.md)**: Personal data management.
* **[Identity](identity.md)**: Manages organization units, roles, users and their permissions, based on the Microsoft Identity library.
* **[Identity (Pro)](identity-pro.md)**: User, role, claims and permission management.
* [**IdentityServer**](identity-server.md): Integrates to IdentityServer4.
* **[Identity Server (Pro)](identity-server-pro.md)**: Managing the identity server objects like clients, API resources, identity resources, secrets, application URLs, claims and more.
* **[Language management (Pro)](language-management.md)**: Add or remove languages and localize the application UI on the fly.
* [**OpenIddict**](openiddict.md): Integrates to OpenIddict.
* **[OpenIddict (Pro)](openiddict-pro.md)**: Managing the openiddict objects like applications, scopes.
* **[Payment (Pro)](payment.md)**: Payment gateway integrations.
* [**Permission Management**](permission-management.md): Used to persist permissions.
* **[SaaS (Pro)](saas.md)**: Manage tenants, editions and features to create your multi-tenant / SaaS application.
* **[Setting Management](setting-management.md)**: Used to persist and manage the [settings](../framework/infrastructure/settings.md).
* [**Tenant Management**](tenant-management.md): Manages tenants for a [multi-tenant](../framework/architecture/multi-tenancy) application.
* **[Text template management (Pro)](text-template-management.md)**: Manage text templates in the system.
* **[Twilio SMS (Pro)](twilio-sms.md)**: Send SMS messages with [Twilio](https://www.twilio.com/) cloud service.
* [**Virtual File Explorer**](virtual-file-explorer.md): Provided a simple UI to view files in [virtual file system](../framework/infrastructure/virtual-file-system.md).
