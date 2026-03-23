```json
//[doc-seo]
{
    "Description": "Explore the ABP Platform Road Map for insights on upcoming features, release schedules, and improvements in version 10.3, planned for April 2026."
}
```

# ABP Platform Road Map

This document provides a road map, release schedule, and planned features for the ABP Platform.

## Next Versions

### v10.3

The next version will be 10.3 and is planned to be released as a stable version in April 2026. We will be mostly working on the following topics:

* Framework
  * Resource-Based Authorization Improvements
  * Handle datetime/timezone in `AbpExtensibleDataGrid` Component
  * Upgrading 3rd-party Dependencies
  * Enhancements in the Core Points

* ABP Suite
  * Improvements on the generated codes for nullability
  * Improvements on Master-Detail Page Design (making it more compact)
  * Improvements One-To-Many Scenarios
  * File Upload Modal Enhancements

* ABP Studio
  * Allow to Directly Create New Solutions with ABP's RC (Release Candidate) Versions
  * Integrate AI Management Module with all solution templates and UIs (for Blazor & Angular UIs)
  * Automate More Details on New Service Creation for a Microservice Solution
  * Allow to Download ABP Samples from ABP Studio
  * Support Multiple Concurrent Kubernetes Deployment/Integration Scenarios
  * Improve the Module Installation Experience / Installation Guides

* Application Modules
  * AI Management: Chat History & Visual Improvements on the playground
  * CMS Kit: Enhancements for Some Features (Rating, Dynamic Widgets, FAQ and more...)
  * UI/UX Improvements on Existing Application Modules

* Updating Existing Tutorials & Documents (with Other UI & DB Options)
  * Microservice Development
  * Modular Monolith Development

## Backlog Items

The *Next Versions* section above shows the main focus of the planned versions. However, in each release, we add new features to the ABP platform.

### Framework

The ABP framework is [open source](https://github.com/abpframework/abp) and free for everyone. You can see its [public backlog](https://github.com/abpframework/abp/milestone/2). Here, are some of the important features you can expect from next versions:

* [#2882](https://github.com/abpframework/abp/issues/2882) / Providing a gRPC integration infrastructure (while it is [already possible](https://github.com/abpframework/abp-samples/tree/master/GrpcDemo) to create or consume gRPC endpoints for your application, we plan to create endpoints for the [standard application modules](../modules/index.md))
* [#57](https://github.com/abpframework/abp/issues/57) / Built-in CQRS infrastructure
* [#58](https://github.com/abpframework/abp/issues/58) / Content localization system (multilingual entities)
* [#4223](https://github.com/abpframework/abp/issues/4223) / WebHook system
* [#162](https://github.com/abpframework/abp/issues/162) / Azure ElasticDB Integration for multitenancy
* [#2296](https://github.com/abpframework/abp/issues/2296) / Feature toggling infrastructure
* [#15932](https://github.com/abpframework/abp/issues/15932) / Introduce ABP Diagnostics Module
* [#16744](https://github.com/abpframework/abp/issues/16744) / State Management API
* [#17815](https://github.com/abpframework/abp/issues/17815) / Operation Rate Limiting
* [#119](https://github.com/abpframework/abp/issues/119) / REST API Versioning Improvements
* [#2087](https://github.com/abpframework/abp/issues/2087) / RavenDB Database Support

### Application Modules / UI Themes

ABP Platform provides many (free and commercial) [pre-built application modules](../modules/index.md) and modern [UI themes](../ui-themes/index.md). In every release, many enhancements and bugfixes are already done on the existing modules. In addition, here some of the planned features for next versions:

* LeptonX theme: New layouts, styles and components
* CMS Kit module: Meta information for SEO, media gallery, RSS feed, content versioning, social media streams
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
* Built-in command terminal
* Automate all steps of new service creation for microservice solutions
* More options while creating new solutions
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

* Handle image properties for entities (in addition to file properties, which is already supported)
* Allow to define extra properties for DTOs those are not a part of the entity
* Allow to create pages instead of modals for CRUD page generation
* View-only (detail view) modal/page for an entity
* Export child/detail entity records as a part of export operation for a main (master) entity
* Allow to add custom entity actions for an entity
* Allow to inherit from an existing entity class
* Custom form layouts on CRUD page generation

## Feature Requests

Vote for your favorite feature on the related GitHub issues (and write your thoughts). You can create an issue on [the GitHub repository](https://github.com/abpframework/abp) for your feature requests, but please first search the existing issues. You can also contact [info@abp.io](mailto:info@abp.io) for your feature requests and other suggestions.

## See Also

* [Release Notes](release-notes.md)
