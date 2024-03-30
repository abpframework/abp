# Version History

This document contains **brief release notes** for each release. Release notes only include **major features** and **visible enhancements**. They don't include all the development done in the related version. **To see raw and detailed change logs for every release, please check [the full change logs document](https://commercial.abp.io/releases).**

> Release notes in this document don't cover the features made in the open-source ABP Framework. For the ABP Framework features, check the blog post for the related version.

## 8.1 (2024-02-14)

> This version is currently in preview. The final release date is planned for June, 2024.

See the detailed **[blog post / announcement](https://blog.abp.io/abp/announcing-abp-8-1-release-candidate)** for the v8.1 release.

* Suite: Bulk Delete
* Suite: Filterable Properties
* Suite: Customizable Page Title
* Suite: Allowing Establishing Relationships with Installed ABP Modules' Entities
* Suite: Support `BasicAggregateRoot` Base Class
* ABP Studio v0.6.5 Has Been Released!

## 8.0 (2023-12-19)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/abp-8-0-stable-release-with-dotnet-8-0)** for the v8.0 release.

* Suite: Generating Master/Detail Relationship
* Getting profile picture from social/external logins.
* Switch Ocelot to YARP for the API Gateway for Microservice Solution Template.
* Password complexity indicators for MVC & Blazor UIs.
* Readonly view & export/import support for Identity/Users page.

## 7.4 (2023-08-16)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP.IO-Platform-7-4-RC-Has-Been-Published)** for the v7.4 release.

* Preserving customizations on code re-generation with ABP Suite
* Support custom text-templates in distributed scenarios.
* MAUI & React Native mobile applications are re-designed and revised for functionality.
* A new CMS Kit feature to collect feedback from users about the site's contents.

## 7.3 (2023-06-12)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP.IO-Platform-7-3-Final-Has-Been-Released)** for the v7.3 release.

* [Account Module](./modules/account.md): Using Authenticator App for Two-Factor Authentication.
* Support for the [Module Entity Extensions](https://docs.abp.io/en/abp/latest/Module-Entity-Extensions) in the [CMS Kit Pro Module](./modules/cms-kit/index.md).
* New Account Layout Design for [LeptonX Theme](./themes/lepton-x/index.md).
* Many enhancements and fixes for the 7.3 version.

## 7.2 (2023-05-03)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP.IO-Platform-7-2-Final-Has-Been-Released)** for the v7.2 release.

* **[LeptonX Theme](./themes/lepton-x/index.md)** - Navigation Menu Item Grouping.
* Support for the **Authority Delegation** in the **[Account Module](./modules/account.md)**.
* Forcing Password Change at Next Logon.
* Periodic Password Changes / Password Aging.
* Suite: Show/Hide Properties on Create/Update/List Pages
* **CMS Kit Comments**: Disallowing External URLs.

## 7.1 (2023-03-22)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP-IO-Platform-7-1-Final-Has-Been-Released)** for the v7.1 release.

* **Blazor WebAssembly** option for the single-layer startup template.
* **ABP Suite** code generation for **MAUI Blazor Hybrid** solutions.
* Allow to **impersonate** an arbitrary **user** in the SaaS module.
* Many enhancements and fixes for the 7.1 version.

## 7.0 (2023-01-05)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP.IO-Platform-7.0-Final-Has-Been-Released)** for the v7.0 release.

* Upgraded to **.NET 7.0**.
* Upgraded to **OpenIddict 4.0**.
* New **MAUI Blazor Hybrid** UI.
* Implemented **external localization**, **dynamic feature** and **dynamic permission** systems to allow more advanced microservice scenarios. All they are applied to the **microservice startup template**.
* **WeChat** and **Alipay** integrations for the **Payment** module.
* Allow host users to **change the password** of a user of a tenant.
* Allow host users to **test connection string** of a tenant database on the UI.
* Introduce **permission** for **searching other users** in the chat module.

## 6.0 (2022-10-05)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP.IO-Platform-6.0-Final-Has-Been-Released)** for the v6.0 release.

* New **OpenIddict** integration module (replacing the IdentityServer integration module).
* The **[LeptonX theme](https://x.leptontheme.com/)** is the default theme now, allowing to use the [old Lepton](https://leptontheme.com/) theme too.
* New **.NET MAUI mobile application**.
* **Blazor UI** for the **chat** module.
* **Blazor admin UI** for the **CMS Kit** module.
* Allow to add **poll widgets** in blog/page contents in the **CMS Kit** module.
* **Cookie consent** feature for the **GDPR** module.
* Optional **PWA** support.
* Exporting to **excel** for **ABP Suite** code generation.

## 5.3 (2022-06-14)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP.IO-Platform-5.3-Final-Has-Been-Released)** for the v5.3 release.

* New module: **GDPR** (currently, allows to download/delete user's personal data).
* **Polling** feature for the [CMS Kit module](modules/cms-kit/index.md).
* OAuth as **external login provider** for the [Identity module](modules/identity.md).
* **ABP Suite**: Support for the no-layers startup template, concurrency stamp support on code generation, downloading Suite logs, using ABP CLI to trigger code generation.
* **Docker-compose** configuration for the no-layers startup template.
* **PWA** support for Blazor WASM and Angular UI.

## 5.2 (2022-04-05)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP-IO-Platform-5-2-Final-Has-Been-Released)** for the v5.2 release.

* Code generation with **many to many relation** support for the [ABP Suite](abp-suite/index.md).
* The new **single-layer**, simpler startup solution template.
* Migrated to **Blazorise 1.0** for the Blazor UI.
* Improvements on the microservice startup solution, pre-built application modules and other existing features.

## 5.1 (2022-01-12)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP.IO-Platform-v5-1-Has-Been-Released)** for the v5.1 release.

* Upgraded to **Angular 13**.
* Changed the application startup solution to use the new ASP.NET Core **minimal hosting model**.
* New **URL Forwarding** feature for the CKS Kit Pro module.
* Improvements and fixes for the features shipped with the 5.0 release.

## 5.0 (2021-12-14)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP-IO-Platform-5.0-RC-1-Has-Been-Released)** for the v5.0.

* Upgraded to **.NET 6.0**.
* Upgraded to **Bootstrap 5.1**.
* **User impersonation** (passwordless login with another user's account).
* **Tenant impersonation** (passwordless login as a tenant).
* Added **Helm charts** to the microservice startup template to deploy to **Kubernetes**.
* Added host and tenant **dashboards** to the microservice startup template.
* **Generate entities** and CRUD pages from **database tables** with ABP Suite.
* Pre-configured **social logins** for the microservice startup template.
* Switched to **static C# and JavaScript proxies** for all the modules.
* **Removed NGXS** and states from the Angular UI.
* Many improvements on existing modules and ABP Suite.

## 4.4 (2021-08-02)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP.IO-Platform-4.4-Final-Has-Been-Released!)** for the v4.4.

* **Subscription** system & **payment** integration for the [SaaS module](modules/saas.md).
* SaaS module: Allow to make a **tenant active/passive** and **limit user count**.
* [ABP Suite](abp-suite/index.md) **code generation** for the [microservice solution](startup-templates/microservice/index.md).
* Allow to set **multiple connection strings** for each tenant, to separate a tenant's database per module/microservice.
* Angular UI: **Two-factor** authentication for resource owner password flow.
* **New localizations**: Hindi, Italian, Arabic, Finnish, French.
* A lot of small improvements and fixes for the current modules, themes and the tooling.

## 4.3 (2021-04-23)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP.IO-Platform-v4-3-Has-Been-Released)** for the v4.3.

* New module: **CMS Kit (pro)**
* New module: **Forms**
* **Blazor Server Side** support
* **Extensibility** system for the Blazor UI
* A lot of improvements done to ripen the **Microservice Startup Template**, including "new service" template, automatic database migrations, solution structure improvements, Tye, Prometheus, Grafana integrations, and more
* Allow to use a **separate database schema** for tenants to not include host-related empty tables in tenant databases
* Creating & **migrating tenant databases on the fly**.
* **Enabling/disabling modules** per edition/tenant
* **Email settings** page
* **Required** navigation properties on Suite code generation

## 4.2 (2021-01-28)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP.IO-Platform-4-2-Final-Has-Been-Released)** for the v4.2.

* **Microservice startup template** (initial) to create microservice solutions.
* **Public website** application in the application startup template.
* **Blazor UI** for the Easy CRM sample application.
* Added login / **authorization** to the **Swagger UI** to test authorized APIs.
* **DBMS selection** on new application creation.
* Infrastructure for **Angular Unit Testing**.
* Iyzico integration for the **Payment** module.
* **Performance** optimization and other enhancements.

## 4.1 (2021-01-06)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP-IO-Platform-v4-1-Final-Has-Been-Released)** for the v4.1.

* **Organization Unit** Management for the Blazor UI.
* **Identity Server** Management for the Blazor UI.
* ABP Suite: **Navigation Property Selection** with Typeahead (supported by all UI types).
* **Spanish** language translation.

## 4.0 (2020-12-03)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP.IO-Platform-4.0-with-.NET-5.0-in-the-4th-Year)** for the v4.0.

* Upgraded to **.NET 5.0**.
* The **Blazor UI** option is now stable and officially supported.
* Completed the Blazor UI for the **file management** module.
* Upgraded to the **Identity Server 4.1.1** and revised the management UI.
* ABP Suite: Blazor UI **code generation**.
* ABP Suite: **Navigation property selection** supports dropdowns with auto-complete & lazy load.
* ABP Suite: **Generate new modules** inside an application solution.
* ABP Suite: Made the **backend code generation optional** to allow re-generate the UI with a different UI framework.

## 3.3 (2020-10-27)

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP-Framework-ABP-Commercial-3.3-Final-Have-Been-Released)** for the v3.3.

* Completed fundamental features, modules and the theme integration for the **Blazor UI**.
* Multi-Tenant **social/external logins** with options configurable on runtime.
* **Linked Accounts** system to link multiple accounts and switch between them easily.
* **Paypal** & **Stripe** integrations for the Payment Module.
* **reCAPTCHA** option for login & register forms.
* **ABP Suite** improvements.

## 3.2 (2020-10-01)

### Blog Post

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP-Framework-ABP-Commercial-3.2-RC-With-The-New-Blazor-UI)** for the v3.2.

### Major Features / Changes

* Released the preview (experimental) **Blazor UI** option.
* **Angular** UI for the [file management](https://commercial.abp.io/modules/Volo.FileManagement) module.
* Managing the **application features** for the **host** side.
* User **profile picture** for the account module.
* Options to enable, disable or force **two factor authentication** for tenants and users.

## 3.1 (2020-09-03)

### Blog Post

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP-Framework-v3.1-Final-Has-Been-Released)** for the v3.1 release.

### Major Features / Changes

* Completely re-written the ABP Suite **Angular UI code generation**, using the Angular Schematics system.
* Implemented **Authorization Code Authentication Flow** for the Angular UI.
* Revised and documented **social/external logins** for the account module and tested with major providers.
* Introduced the new external login system supporting to login via **LDAP / Active Directory**. Also, added a setting page to configure the LDAP options.
* Created a new **security log system** and the user interface to save and report all the authentication related operations (login, logout, change password...) for users.
* Implemented **email & phone number verification**.
* Implementing **locking a user** for a given period of time (locked users can not login to the application).
* Added breadcrumb and file icons for the file management module.

## 3.0 (2020-07-01)

### Blog Post

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP-Framework-v3.0-Has-Been-Released)** for the v3.0 release.

### Major Features

* New **File Management Module** that is used to store and manage files in your application.
* Migrated the Angular UI to the **Angular 10**.
* Published an **[API documentation](https://docs.abp.io/api-docs/commercial/2.9/api/index.html)** web site to explore the classes of the ABP Commercial.

## 2.9 (2020-06-04)

### Blog Post

See the detailed **[blog post / announcement](https://blog.abp.io/abp/ABP-Framework-v2.9.0-Has-Been-Released)** for the v2.9 release.

### Major Features / Enhancements

* New **Organization Unit** Management UI for the [Identity Module](https://commercial.abp.io/modules/Volo.Identity.Pro) to create hierarchical organization units and manage their members and roles.
* Created **Angular UI** for the [Chat Module](https://commercial.abp.io/modules/Volo.Chat).
* Implemented **Angular UI** for the [Easy CRM](https://docs.abp.io/en/commercial/latest/samples/easy-crm) application.
* [ABP Suite](https://commercial.abp.io/tools/suite) code generation support for **module development**.
* New [leptontheme.com](http://leptontheme.com/) web site to show the **[Lepton Theme](https://commercial.abp.io/themes) components**.

## 2.8 (2020-05-21)

### Blog Post

See the detailed **blog post / announcement** for the v2.8 release: [https://blog.abp.io/abp/ABP-v2.8.0-Releases-%26-Road-Map](https://blog.abp.io/abp/ABP-v2.8.0-Releases-%26-Road-Map)

This post also covers the [road map](road-map.md) and other news for the ABP.IO Platform.

### Major Features / Enhancements

* Completely renewed the **[Lepton Theme](https://commercial.abp.io/themes) styles** and add a new one.
* New module: Created a **real time [Chat Module](https://commercial.abp.io/modules/Volo.Chat)** that is built on ASP.NET Core SignalR. It currently has only the MVC / Razor Pages UI. Angular UI is on the way.
* Implemented **[module entity extension](guides/module-entity-extensions.md) system** for the **Angular UI**. Also improved the system to better handle float/double/decimal, date, datetime, enum and boolean properties.
* **Gravatar** integration for the Angular UI.
* Managing product groups on a **tree view** for the [EasyCRM sample application](samples/easy-crm.md).

## 2.7 (2020-05-07)

### Blog Post

See the detailed **blog post / announcement** for the v2.7 release:  https://blog.abp.io/abp/ABP-Framework-v2_7_0-Has-Been-Released 

### Major Features

* New module: **Text template management** (with angular and mvc UI - document is [coming](modules/text-template-management.md)).
* **Dynamically add properties** to current entities of the depended modules (see [module entity extensions](guides/module-entity-extensions.md))
* To be able to add **navigation properties** to entities with the ABP Suite (see [navigation properties](https://docs.abp.io/en/commercial/latest/abp-suite/generating-crud-page#navigation-properties))
* Dynamically add **data table columns** on the user interface (see the documents: [angular](ui/angular/data-table-column-extensions.md), [mvc](ui/aspnetcore/data-table-column-extensions.md))
* Created a rich **sample solution**, named "Easy CRM" (see the document)

### Other Enhancements

* Allow to dynamically **override the logo**.
* **Optimize database migrations** & seed code for multi-tenant multi-database systems.
* ABP Suite: Make **menu item active** on navigation menu when selected.
* ABP Suite: Improve **enum usage** while creating new entities.
* Bug fixes in the [Lepton Theme](https://commercial.abp.io/themes), [ABP Suite](https://commercial.abp.io/tools/suite) and  other modules.

## See Also

* [Road map](road-map.md)
