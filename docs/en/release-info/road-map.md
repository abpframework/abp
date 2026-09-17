```json
//[doc-seo]
{
    "Description": "Explore the ABP Platform Road Map for insights on upcoming features, release schedules, and improvements in version 10.7, planned for August 2026."
}
```

# ABP Platform Road Map

This document provides a road map, release schedule, and planned features for the ABP Platform.

## Next Versions

### v10.7

The next planned version will be 10.7, which is scheduled to be released as a stable version in August 2026. Based on the currently open issues and pull requests across the ABP ecosystem, we will be mostly working on the following topics:

* Framework
  * Cookie authentication: refresh token support and distributed locking ([#25011](https://github.com/abpframework/abp/issues/25011))
  * jQuery 4.x upgrade, or removing jQuery as a dependency ([#25123](https://github.com/abpframework/abp/issues/25123))
  * AI agent skills distributed as versioned plugins ([#25712](https://github.com/abpframework/abp/issues/25712))
  * Hybrid UI / page embedding infrastructure ([#23102](https://github.com/abpframework/abp/issues/23102), [#23161](https://github.com/abpframework/abp/issues/23161))
  * Microsoft Agent Framework migration and native agent skills support ([#24310](https://github.com/abpframework/abp/issues/24310), [#25194](https://github.com/abpframework/abp/issues/25194))
  * Better ExtraProperties mapping for EF Core ([#23546](https://github.com/abpframework/abp/issues/23546))
  * Upgrading 3rd-party dependencies and general bug fixing in core framework packages

* ABP Suite
  * Replace the templating system with Scriban while preserving backward compatibility
  * Support for additional property types like `DateTimeOffset`, `TimeSpan` and numeric enums
  * Display names and ordering for properties and navigation properties
  * Filter on inherited properties and namespace-based UI foldering
  * Low-Code system integration
  * Improvements on generated code nullability, master-detail pages and file upload experiences

* ABP Studio
  * Low-Code platform integration
  * Theme Builder: live preview, project integration and import/export
  * Linux support and packaging improvements
  * Better React / React Native / Thin UI template experience
  * Better startup, diagnostics, module installation and solution runner experiences
  * More automation for microservice solution creation and service/module addition
  * Terminal, browser and built-in developer productivity enhancements

* Application Modules
  * AI Management: chat history
  * AI Management: multi-tenancy and tenant-scoped workspace capabilities
  * RAG: Cloudflare `/crawl` endpoint as a data source
  * New module: Chat with your data
  * Payment module e-mail notification improvements
  * CMS Kit and public website improvements
  * UI/UX improvements on existing application modules

* Updating existing tutorials & documents (with other UI & DB options)
  * Documentation updates for modern React, Studio workflows and new CLI capabilities
  * General refresh for roadmap, tutorials and feature comparison documents

## Backlog Items

The *Next Versions* section above shows the main focus of the planned versions. However, in each release, we add new features to the ABP platform.

### Framework

The ABP framework is [open source](https://github.com/abpframework/abp) and free for everyone. You can see its [public backlog](https://github.com/abpframework/abp/milestone/2). Here are some of the selected backlog items and longer-term topics:

* [#23102](https://github.com/abpframework/abp/issues/23102) / ABP Hybrid UI System: Re-using module UIs in different technologies
* [#23161](https://github.com/abpframework/abp/issues/23161) / Page Embedding Feature (aka Hybrid UI)
* [#25123](https://github.com/abpframework/abp/issues/25123) / Upgrade jQuery to 4.x, or consider removing it as dependency
* [#17093](https://github.com/abpframework/abp/issues/17093) / MVC UI: decouple jQuery
* [#24310](https://github.com/abpframework/abp/issues/24310) / Migrate Volo.Abp.AI Semantic Kernel to Microsoft Agent Framework
* [#25194](https://github.com/abpframework/abp/issues/25194) / Integrate Microsoft.Agents.AI for native Agent Skills support
* [#23575](https://github.com/abpframework/abp/issues/23575) / Support list/enumerable of complex types for ABP dynamic/static C# proxies on GET requests
* [#23546](https://github.com/abpframework/abp/issues/23546) / Better ExtraProperties mapping for EF Core
* [#23935](https://github.com/abpframework/abp/issues/23935) / Hybrid Cache Support for EntityCache
* [#22931](https://github.com/abpframework/abp/issues/22931) / Angular - Support dynamic URLs for breadcrumbs
* [#24742](https://github.com/abpframework/abp/issues/24742) / Add Support for LiteDB as a Database Provider
* [#24442](https://github.com/abpframework/abp/issues/24442) / Add Couchbase EF Core Provider Integration
* [#2882](https://github.com/abpframework/abp/issues/2882) / ABP gRPC Integration
* [#57](https://github.com/abpframework/abp/issues/57) / CQRS infrastructure
* [#58](https://github.com/abpframework/abp/issues/58) / Content localization system (multilingual entities)
* [#4223](https://github.com/abpframework/abp/issues/4223) / WebHook System
* [#162](https://github.com/abpframework/abp/issues/162) / Azure ElasticDB Integration for multitenancy
* [#2296](https://github.com/abpframework/abp/issues/2296) / Implementing Feature Toggle
* [#15932](https://github.com/abpframework/abp/issues/15932) / Introduce ABP Diagnostics Module
* [#16744](https://github.com/abpframework/abp/issues/16744) / State Management API
* [#119](https://github.com/abpframework/abp/issues/119) / REST API Versioning Improvements
* [#2087](https://github.com/abpframework/abp/issues/2087) / Add RavenDB Database support

### Application Modules / UI Themes

ABP Platform provides many (free and commercial) [pre-built application modules](../modules/index.md) and modern [UI themes](../ui-themes/index.md). In every release, many enhancements and bugfixes are delivered for these modules and themes. Important backlog topics currently include:

* AI Management module: chat history, multi-tenancy, tenant workspaces and operational hardening
* New module: Chat with your data
* RAG with external data sources such as website crawling
* Payment module: richer notifications and invoice-oriented scenarios
* CMS Kit: Meta information for SEO
* Audit logging UI: filter redesign and UX improvements
* Identity Pro: richer filtering and organization-unit UX improvements
* LeptonX and existing UIs: new layouts, styles and usability refinements

### ABP Studio

[ABP Studio](../studio/index.md) is a cross-platform desktop application for ABP and .NET developers to simplify and automate daily tasks of developers. It has a community (free) edition as well as commercial capabilities. Here are some of the important planned features and active backlog topics for the next ABP Studio versions:

* Low-Code: ABP Studio Integration
* Theme builder for LeptonX, including live preview, management UI and project integration
* Analyze user solutions to explore entities, domain services, application services, pages and other fundamental objects
* AI agent/browser capabilities and developer-assistant experiences
* Built-in command terminal and richer embedded development tools
* Built-in or embedded VS Code experience
* Better Linux and cross-platform support
* Better startup templates, including React Native, Thin UI, Angular SSR and public website scenarios
* More options while creating new solutions, modules and services
* Better environment-variable, deployment and Kubernetes experiences
* Compare changes on startup templates when a new ABP version is published
* ABP support integration and better diagnostics/error experiences

### ABP Suite

[ABP Suite](../suite/index.md) is a GUI application that is mainly used to generate CRUD-style pages in your application. You define your entity and it can generate all the code from the database to the UI. Here are some of the important planned features for the next ABP Suite versions:

* Replace the current templating system with Scriban while preserving backward compatibility
* Better nullability support in generated code
* Support for additional property types like `DateTimeOffset` and `TimeSpan`
* Handle image properties for entities (in addition to file properties, which are already supported)
* Allow to define display names and better ordering for properties and navigation properties
* Filter on inherited properties and improve relationship management experiences
* Allow to change generated file paths and support namespace-based foldering for generated UI code
* View-only (detail view) modal/page for an entity
* Custom form layouts on CRUD page generation
* Allow to create pages instead of modals for CRUD page generation
* Export child/detail entity records as a part of export operations for a main (master) entity

## Feature Requests

Vote for your favorite feature on the related GitHub issues (and write your thoughts). You can create an issue on [the GitHub repository](https://github.com/abpframework/abp) for your feature requests and suggestions.

## See Also

* [Release Notes](release-notes.md)
