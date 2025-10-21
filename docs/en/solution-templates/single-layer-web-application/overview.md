```json
//[doc-seo]
{
    "Description": "Explore the Single Layer solution template in ABP Framework, featuring pre-installed libraries and services for streamlined development and production."
}
```

# Single Layer Solution: Overview

````json
//[doc-nav]
{
  "Previous": {
    "Name": "Index",
    "Path": "solution-templates/single-layer-web-application/index"
  },
  "Next": {
    "Name": "Solution Structure",
    "Path": "solution-templates/single-layer-web-application/solution-structure"
  }
}
````

> Some of the features mentioned in this document may not be available in the free version. We're using the **\*** symbol to indicate that a feature is available in the **[Team](https://abp.io/pricing)** and **[Higher](https://abp.io/pricing)** licenses.

This document explains what the Single-Layer solution template offers.  

## Pre-Installed Libraries & Services

The following **libraries and services** come **pre-installed** and **configured** for both **development** and **production** environments. After creating your solution, you can **modify** or **remove** most of them as needed.

* **[Autofac](https://autofac.org/)** for [Dependency Injection](../../framework/fundamentals/dependency-injection.md).  
* **[Serilog](https://serilog.net/)** with File and Console [logging](../../framework/fundamentals/logging.md) providers.  
* **[Swagger](https://swagger.io/)** for exploring and testing HTTP APIs.  
* **[OpenIddict](https://github.com/openiddict/openiddict-core)** as the built-in authentication server.

## Pre-Configured Features

The solution comes with the following built-in and pre-configured features:  

* **Authentication** is fully configured based on best practices.
* **[Permission](../../framework/fundamentals/authorization.md)** (authorization), **[setting](../../framework/infrastructure/settings.md)**, **[feature](../../framework/infrastructure/features.md)** and the **[localization](../../framework/fundamentals/localization.md)** management systems are pre-configured and ready to use.
* **[Background job system](../../framework/infrastructure/background-jobs/index.md)**.
* **[BLOB storge](../../framework/infrastructure/blob-storing/index.md)** system is installed with the [database provider](../../framework/infrastructure/blob-storing/database.md).
* **On-the-fly database migration** system (services automatically migrated their database schema when you deploy a new version). **\***
* **[Swagger](https://swagger.io/)** authentication is configured to test the authorized HTTP APIs.

## Fundamental Modules

The following modules are pre-installed and configured for the solution:

* **[Account](../../modules/account.md)** to authenticate users (login, register, two factor auth **\***, etc)
* **[Identity](../../modules/identity.md)** to manage roles and users
* **[OpenIddict](../../modules/openiddict.md)** (the core part) to implement the OAuth authentication flows

In addition, [Feature Management](../../modules/feature-management.md), [Permission Management](../../modules/permission-management.md) and [Setting Management](../../modules/setting-management.md) modules are pre-installed as they are the fundamental feature modules of the ABP.

## Optional Modules

The following modules are optionally included in the solution, so you can select the ones you need:

* **[Audit Logging](../../modules/audit-logging.md)**
* **[Chat](../../modules/chat.md)** **\***
* **[File Management](../../modules/file-management.md)** **\***
* **[GDPR](../../modules/gdpr.md)** **\***
* **[Language Management](../../modules/language-management.md)** **\***
* **[OpenIddict (Management UI)](../../modules/openiddict.md)** **\***
* **[Tenant Management](../../modules/tenant-management.md) (Multi-Tenancy) or [SaaS](../../modules/saas.md)** **\*** 
* **[Text Template Management](../../modules/text-template-management.md)** **\***

## UI Theme

The **[LeptonX Lite](../../ui-themes/lepton-x-lite/index.md) or [LeptonX theme](https://leptontheme.com/)** **\*** is pre-configured for the solution. You can select one of the color palettes (System, Light, or Dark) as default, while the end-user dynamically change it on the fly.

## Other Options

Single-layer startup template asks for some preferences while creating your solution.

### Database Providers

There are two database provider options are provided on a new solution creation:

* **[Entity Framework Core](../../framework/data/entity-framework-core/index.md)** with SQL Server, MySQL and PostgreSQL DBMS options. You can [switch to another DBMS](../../framework/data/entity-framework-core/other-dbms.md) manually after creating your solution.
* **[MongoDB](../../framework/data/mongodb/index.md)**

### UI Frameworks

The solution comes with a main web application with the following UI Framework options:

* **None** (doesn't include a UI application to the solution)
* **Angular**
* **MVC / Razor Pages UI**
* **Blazor WebAssembly**
* **Blazor Server**

### Multi-Tenancy & SaaS Module **\***

The **[SaaS module](../../modules/saas.md)** is included as an option. When you select it, the **[multi-tenancy](../../framework/architecture/multi-tenancy/index.md)** system is automatically configured. Otherwise, the system will not include any multi-tenancy overhead.

## See Also

* [Quick Start: Creating a Single Layer Web Application with ABP Studio](../../get-started/single-layer-web-application.md)
