# ABP Platform Road Map

This document provides a road map, release schedule, and planned features for the ABP Platform.

## Next Versions

### v8.2

The next version will be 8.2 and planned to release the stable 8.2 version in June 2024. We will be mostly working on the following topics:

* ABP Studio Community Edition
* Blazor Full-Stack UI ([#18289](https://github.com/abpframework/abp/issues/18289))
* Angular Universal ([#15782](https://github.com/abpframework/abp/issues/15782))
* Upgrading React Native template to the latest major release 0.72.7 ([#18191](https://github.com/abpframework/abp/issues/18191))
* Deployment Documents Improvements ([#15034](https://github.com/abpframework/abp/issues/15034))
* Improvements on the existing features and provide more guides.

See the [8.2 milestone](https://github.com/abpframework/abp/milestone/95) for all the issues we've planned to work on.

## Backlog Items

The *Next Versions* section above shows the main focus of the planned versions. However, in each release, we add new features to the core framework and the [application modules](../modules).

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
* Chat Module: Deleting messages & conversations

### Tooling

* Suite: File, DateOnly and TimeOnly types
* Suite: Export child/detail entity records
* CMS Kit features, including FAQ, spam protection, social media feeds, multi-language support, and so on.

You can always check the milestone planning and the prioritized backlog issues on [the GitHub repository](https://github.com/abpframework/abp/milestones) for a detailed road map. The backlog items are subject to change. We are adding new items and changing priorities based on the community feedbacks and goals of the project.

## Feature Requests

Vote for your favorite feature on the related GitHub issues (and write your thoughts). You can create an issue on [the GitHub repository](https://github.com/abpframework/abp) for your feature requests, but first search in the existing issues please. You can also contact info@abp.io for your feature requests and other suggestions.

## See Also

* [Release Notes](release-notes.md)
