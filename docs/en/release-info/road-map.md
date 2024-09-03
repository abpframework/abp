# ABP Platform Road Map

This document provides a road map, release schedule, and planned features for the ABP Platform.

## Next Versions

### v9.0

The next version will be 9.0 and planned to release the stable 9.0 version in November 2024. We will be mostly working on the following topics:

* Framework
  * Upgrading to .NET 9.0
  * Google Cloud Storage BLOB Provider
  * Better handling localization resources in a microservice scenario
  * SSR support for the Angular UI
  * Upgrading 3rd-party dependencies

* ABP Suite
  * Multiple navigation properties to the same target entity
  * Define navigation properties without target string property dependency
  * Improvements one-to-many scenarios
  * Access to default code generation templates for customized templates

* ABP Studio
  * Blazor WebApp UI option for the new startup templates
  * Execution order (dependency management) for Solution Runner
  * Automate more details on new service creation for a microservice solution
  * Support multiple concurrent Kubernetes deployment/integration scenarios
  * Show the README file when you create a new solution or open an existing solution
  * Improve ABP Suite code-generation possibilities for microservice solutions
  * Improve the module installation experience
  * Auto-install 3rd-party dependencies
  * Better handle long log files
  * Allow to directly create new solutions with ABP's RC (Release Candidate) versions
  * Support Intel processors for Mac computers, support ARM chipset for Windows and support Linux OS
  * Improve client proxy generation experience

* Application modules
  * Account module: Support mixed social/local login scenarios
  * UI/UX improvements on existing [application modules](../modules/index.md)

* New tutorials
  * Modular monolith development
  * Microservice development

## Backlog Items

The *Next Versions* section above shows the main focus of the planned versions. However, in each release, we add new features to the core framework and the [application modules](../modules/index.md).

Here is a list of major items in the backlog we are considering working on in the next versions.

### Framework

* [#86](https://github.com/abpframework/abp/issues/86) / GrapQL Integration
* [#236](https://github.com/abpframework/abp/issues/236) / Resource based authorization system
* [#2882](https://github.com/abpframework/abp/issues/2882) / Providing a gRPC integration infrastructure (while it is [already possible](https://github.com/abpframework/abp-samples/tree/master/GrpcDemo) to create or consume gRPC endpoints for your application, we plan to create endpoints for the [standard application modules](../modules))
* [#57](https://github.com/abpframework/abp/issues/57) / Built-in CQRS infrastructure
* [#4223](https://github.com/abpframework/abp/issues/4223) / WebHook system
* [#162](https://github.com/abpframework/abp/issues/162) / Azure ElasticDB Integration for multitenancy
* [#2296](https://github.com/abpframework/abp/issues/2296) / Feature toggling infrastructure
* [#16342](https://github.com/abpframework/abp/issues/16342) / CMS Kit: Meta information for SEO
* [#16260](https://github.com/abpframework/abp/issues/16260) / GCP Blob Storage Provider
* [#15932](https://github.com/abpframework/abp/issues/15932) / Introduce ABP Diagnostics Module
* [#16756](https://github.com/abpframework/abp/issues/16756) / Blob Storing - Provider configuration UI
* [#16744](https://github.com/abpframework/abp/issues/16744) / State Management API

### Modules / Themes

* New styles, components and features for the LeptonX theme.
* Payment module: Invoice system.
* Dynamic dashboard system.
* User guiding module.
* gRPC integration and implementation for all the pre-built modules.

### Tooling

* ABP Suite: Extra Properties on CRUD Page generation
* ABP Suite: Allow to create PAGE instead of MODAL for CRUD page generation
* ABP Suite: Export child/detail entity records
* CMS Kit features, including spam protection, social media feeds, multi-language support, and so on.

You can always check the milestone planning and the prioritized backlog issues on [the GitHub repository](https://github.com/abpframework/abp/milestones) for a detailed road map. The backlog items are subject to change. We are adding new items and changing priorities based on the community feedbacks and goals of the project.

## Feature Requests

Vote for your favorite feature on the related GitHub issues (and write your thoughts). You can create an issue on [the GitHub repository](https://github.com/abpframework/abp) for your feature requests, but first search in the existing issues please. You can also contact info@abp.io for your feature requests and other suggestions.

## See Also

* [Release Notes](release-notes.md)
