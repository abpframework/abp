﻿# ABP Framework Road Map

This document provides a road map, release schedule and planned features for the ABP Framework.

## Next Versions

### v4.4

This version will focus on **documentation** and **improvements** of current features. In addition, the following features are planned:

* Revisit the microservice demo solution ([#8385](https://github.com/abpframework/abp/issues/8385))
* A new UI Theme alternative to the Basic Theme ([#6132](https://github.com/abpframework/abp/issues/6132))
* Improvements and new features to the [CMS Kit](Modules/Cms-Kit.md) module ([#8380](https://github.com/abpframework/abp/issues/8380) [#8381](https://github.com/abpframework/abp/issues/8381))
* Razor engine support for text templating ([#8373](https://github.com/abpframework/abp/issues/8373))

**Planned release date**: End of Quarter 2, 2021. See the [4.4 milestone](https://github.com/abpframework/abp/milestone/52) to track the progress.

### v5.0

We planned to focus on the ABP Framework v5.0 after the 4.4 release. This version will focus on the following works:

* Upgrading to .NET 6.0 ([#9004](https://github.com/abpframework/abp/issues/9004))
* Upgrading to Bootstrap 5.x ([#8922](https://github.com/abpframework/abp/issues/8922))
* Alternative to IdentityServer4 ([#7221](https://github.com/abpframework/abp/issues/7221))
* Dapr integration ([#2183](https://github.com/abpframework/abp/issues/2183))
* Publishing distributed events as transactional ([#6126](https://github.com/abpframework/abp/issues/6126))
* Resource based authorization system ([#236](https://github.com/abpframework/abp/issues/236))
* API Versioning system: finalize & document ([#497](https://github.com/abpframework/abp/issues/497))
* Performance optimizations; Enabling .NET Trimming, using source generators and reducing reflection, etc.
* Improving the abp.io platform and work more on contents and documents

**Planned release date**: End of Quarter 4, 2021. See the [5.0 milestone](https://github.com/abpframework/abp/milestone/51) to track the progress.

## Backlog Items

The *Next Versions* section above shows the main focus of the planned versions. However, in each release we add new features to the core framework and the [application modules](Modules/Index.md).

Here, a list of major items in the backlog we are considering to work on in the next versions.

* [#2882](https://github.com/abpframework/abp/issues/2882) / Providing a gRPC integration infrastructure (while it is [already possible](https://github.com/abpframework/abp-samples/tree/master/GrpcDemo) to create or consume gRPC endpoints for your application, we plan to create endpoints for the [standard application modules](https://docs.abp.io/en/abp/latest/Modules/Index))
* [#1754](https://github.com/abpframework/abp/issues/1754) / Multi-lingual entities
* [#57](https://github.com/abpframework/abp/issues/57) / Built-in CQRS infrastructure
* [#336](https://github.com/abpframework/abp/issues/336) / Health Check abstractions
* [#2532](https://github.com/abpframework/abp/issues/2532), [#2564](https://github.com/abpframework/abp/issues/2465) / CosmosDB integration with EF Core and MongoDB API
* [#4223](https://github.com/abpframework/abp/issues/4223) / WebHook system
* [#162](https://github.com/abpframework/abp/issues/162) / Azure ElasticDB Integration for multitenancy
* [#2296](https://github.com/abpframework/abp/issues/2296) / Feature toggling infrastructure

You can always check the milestone planning and the prioritized backlog issues on [the GitHub repository](https://github.com/abpframework/abp/milestones) for a detailed road map. The backlog items are subject to change. We are adding new items and changing priorities based on the community feedbacks and goals of the project.

## Feature Requests

Vote for your favorite feature on the related GitHub issues (and write your thoughts). You can create an issue on [the GitHub repository](https://github.com/abpframework/abp) for your feature requests, but first search in the existing issues.

