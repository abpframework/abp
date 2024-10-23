# ABP Platform Road Map

This document provides a road map, release schedule, and planned features for the ABP Platform.

## Next Versions

### v9.1

The next version will be 9.1 and planned to release the stable 9.1 version in January 2025. We will be mostly working on the following topics:

* Framework
  * Lazy expandable feature for documentation
  * Unify the bundling system of Blazor and MVC
  * SSR support for the Angular UI
  * Upgrading 3rd-party dependencies

* ABP Suite
  * Define navigation properties without target string property dependency
  * Improvements one-to-many scenarios
  * Access to default code generation templates for customized templates
  * File Upload Modal enhancements

* ABP Studio
  * Automate more details on new service creation for a microservice solution
  * Support multiple concurrent Kubernetes deployment/integration scenarios
  * Improve the module installation experience / installation guides
  * Auto-install 3rd-party dependencies
  * Better handle long log files
  * Allow to directly create new solutions with ABP's RC (Release Candidate) versions
  * Support Intel processors for Mac computers, support ARM chipset for Windows and support Linux OS
  * Improve client proxy generation experience
  * Modular Monolith Application Startup Template

* Application modules
  * Account module: Support mixed social/local login scenarios
  * Idle session warning
  * UI/UX improvements on existing application modules

* New tutorials
  * Microservice development

## Backlog Items

The *Next Versions* section above shows the main focus of the planned versions. However, in each release, we add new features to the ABP platform.

### Framework

The ABP framework is [open source](https://github.com/abpframework/abp) and free for everyone. You can see its [public backlog](https://github.com/abpframework/abp/milestone/2). Here, are some of the important features you can expect from next versions:

* [#236](https://github.com/abpframework/abp/issues/236) / Resource based authorization system
* [#2882](https://github.com/abpframework/abp/issues/2882) / Providing a gRPC integration infrastructure (while it is [already possible](https://github.com/abpframework/abp-samples/tree/master/GrpcDemo) to create or consume gRPC endpoints for your application, we plan to create endpoints for the [standard application modules](../modules/index.md))
* [#57](https://github.com/abpframework/abp/issues/57) / Built-in CQRS infrastructure
* [#58](https://github.com/abpframework/abp/issues/58) / Content localization system (multilingual entities)
* [#4223](https://github.com/abpframework/abp/issues/4223) / WebHook system
* [#162](https://github.com/abpframework/abp/issues/162) / Azure ElasticDB Integration for multitenancy
* [#2296](https://github.com/abpframework/abp/issues/2296) / Feature toggling infrastructure
* [#15932](https://github.com/abpframework/abp/issues/15932) / Introduce ABP Diagnostics Module
* [#16744](https://github.com/abpframework/abp/issues/16744) / State Management API
* [#17815](https://github.com/abpframework/abp/issues/17815) / Operation Rate Limiting

### Application Modules / UI Themes

ABP Platform provides many (free and commercial) [pre-built application modules](../modules/index.md) and modern [UI themes](../ui-themes/index.md). In every release, many enhancements and bugfixes are already done on the existing modules. In addition, here some of the planned features for next versions:

* LeptonX theme: New layouts, styles and components
* CMS Kit module: Meta information for SEO, media gallery, RSS feed, content versioning, social media streams
* Identity module: Idle session warning
* Payment module: Invoice system
* New module: User notification
* New module: Dynamic dashboard
* New module: User guiding
* New module: Keycloak integration

### ABP Studio

[ABP Studio](../studio/index.md) is a cross-platform desktop application for ABP and .NET developers to simplify and automate daily tasks of developers. It has a community (free) edition as well as commercial editions. It is released and versioned independently from the ABP platform and frequently released.

Here, are some of the important planned features for next ABP Studio versions:

* Theme builder for the LeptonX theme
* Analyze user solutions to explore entities, domain services, application services, pages and other fundamental objects.
* Swagger authentication support for the built-in browser
* Show related requests/events (traces) together in the solution runner panel
* Integrate common tool dashboards into ABP Studio (such a Garana, Redis, RabbitMQ, Kibana, etc)
* Built-in command terminal
* Automate all steps of new service creation for microservice solutions
* Container application type support for Solution Runner (to individually control docker dependencies)
* More options while creating new solutions
* Downloading samples in ABP studio
* Built-in ABP documentation experience
* Auto-execute terminal commands in markdown files
* Compare changes on the startup templates when a new ABP version is published
* Remove unused projects while downloading the source code of an existing module
* Testing/hosting applications for module templates
* Easily explore all module and package dependencies of a large solution
* Built-in deployment options
* Rapid application development features
* ABP support integration

### ABP Suite

[ABP Suite](../suite/index.md) is a GUI application that is mainly used to generate CRUD style pages in your application. You define your entity and it can generate all the code from the database layer to the UI layer. The generated code is clean and a perfect starting point to implement your custom requirements on top of it.

Here, are some of the important planned features for the next ABP Suite versions:

* Handle image properties for entities
* Allow to define extra properties for DTOs those are not a part of the entity
* Allow to create pages instead of modals for CRUD page generation
* View-only (detail view) modal/page for an entity
* Export child/detail entity records as a part of export operation for a main (master) entity
* Allow to accept attachments (files) for an entity
* Allow to add custom entity actions for an entity
* Allow to inherit from an existing entity class
* Custom form layouts on CRUD page generation

## Feature Requests

Vote for your favorite feature on the related GitHub issues (and write your thoughts). You can create an issue on [the GitHub repository](https://github.com/abpframework/abp) for your feature requests, but first search in the existing issues please. You can also contact info@abp.io for your feature requests and other suggestions.

## See Also

* [Release Notes](release-notes.md)
