```json
//[doc-seo]
{
    "Description": "Explore the latest features and enhancements in ABP Studio with release notes detailing major updates and improvements for developers."
}
```

# ABP Studio Release Notes

This document contains **brief release notes** for each ABP Studio release. Release notes only include **major features** and **visible enhancements**. Therefore, they don't include all the development done in the related version. 

## 2.2.7 (2026-04-20) Latest

* Improved Blazor WebApp template setup for easier tiered application development
* Added application version tracking in analytics events
* Fixed issues in Basic Theme public website templates
* Improved PostgreSQL vector database support in templates
* Enhanced Blazor CRUD support with built-in Book management example
* Modernized React Native template components
* Updated to ABP 10.3 and Blazorise 2.0.4
* Improved run profile and PowerShell execution reliability
* Added AI Management and Rate Limiting modules to available module options  


## 2.2.6 (2026-04-08)

- Disable Scriban 7.0 cumulative output limit for template rendering

## 2.2.5 (2026-04-08)

- Upgraded GPT-5 → GPT-5.4 and improved AI management (providers, blob storage, CLI options)
- Fixed critical build issues (MongoDB, MAUI) and improved overall stability
- Enhanced monitoring (HTTP requests & exceptions)
- Added DBMS auto-detection from connection string
- Upgraded to ABP 10.2 and Scriban 7.0.0
- Improved developer experience and telemetry (PostHog)
- Minor UI fixes and workflow adjustments (manual build trigger)

## 2.2.4 (2026-03-25)

- Add `Template Create and Build` workflow
- Disable NuGetAudit in template common.props to prevent CLI deadlock during initial migration

## 2.2.3 (2026-03-24)

- Fix PostHog environment detection

## 2.2.1 (2026-02-20)

- Fix tiered Blazor WebApp template HttpApi reference
- Add LeptonX theme templates AuthServer and HttpApi projects
- Enable razor runtime compilation on templates
- Configure HttpClientFactoryOptions for CLI client
- Bump ABP to 10.1.1 and LeptonX to 5.1.1
- Blazor & Angular UIs: Add AI Management option to the Startup Templates (app-nolayers, app, ms templates)
- Handle docker container start failures in solution runner
- Fix import module version dropdown ordering
- Add PostHog integration for Studio and CLI


## 2.2.1 (2026-02-20)

* Added `abp run` and `abp watch` commands to Studio CLI.
* Added "Start and wait for ready" option in Solution Runner.
* Added Angular support to standard solution and module templates, with Angular templates upgraded to v21.
* Updated ABP Framework to `10.1.0` and LeptonX to `5.0.3`.
* Fixed various Solution Runner issues, including tooltips, log rendering, and stability problems.

## 2.1.9 (2026-01-30)

* Fixed MCP server CLI output problem.

## 2.1.8 (2026-01-29)

* Added Studio MCP server support to allow AI monitoring of applications linked to ABP Studio.
* Added `Open with > Cursor` option.
* Improved task failure handling and related log visibility.
* Fixed various Solution Runner issues, including memory/crash and log scrolling problems.

## 2.1.7 (2026-01-23)

* Added management UI for custom solution commands.
* Showed logs of background jobs.
* Updated Aspire to version `13.1`.

## 2.1.6 (2026-01-13)

* Enhanced runnable task logs window.
* Fixed tooltip line-height problem.

## 2.1.5 (2026-01-13)

* Added `version` command to Studio CLI.
* Updated ABP Framework to `10.0.2` and LeptonX to `5.0.2`.
* Fixed microservice solution build errors for Blazor Server and Angular.
* Improved Solution Runner behavior to avoid re-running applications after build errors.
* Replaced "Clear Cookies" with "Clear site data" in the tools section.

## 2.1.4 (2025-12-30)

* Fixed books sample for blazor-webapp tiered solution.
* Fixed K8s cluster deployment issues for microservices.
* Fixed docker build problem on microservice template.
* Showed logs of the executed tasks.

## 2.1.3 (2025-12-15)

* Updated `createCommand` and CLI help for multi-tenancy.
* Fixed `BookController` templating problem.

## 2.1.2 (2025-12-11)

* Fixed `SLNX` files in templates for macOS.
* Fixed `DbMigrator` problem on nolayers template.

## 2.1.1 (2025-12-11)

* Fixed duplicate workspace seeding issue.
* Fixed books sample problems when solution is tiered.
* Added AI Management module to `abpmdl` file.
* Improved skip running initial tasks text.
* Fixed unit test failures.
* Added `LanguageManagementDbContext` table creation in tests.
* Removed `ConfigureHttpClientProxies` method.
* Fixed issue with adding new services to existing Microservices.
* Fixed AI Management template issues.
* Reverted browser notification overlay fix.

## 2.1.0 (2025-12-08)

* Enhanced Module Installation UI with improved user experience.
* Added `AI Management` option to Startup Templates (app-nolayers, app).
* Added support for new `SLNX` solution file format.
* Enhanced modularity step in solution creation process.
* Fixed Swagger authorization issues when projects run via .NET Aspire.
* Fixed browser notification overlay problems.
* Added missing `Unit of Work` namespace in solution templates.
* Fixed JSON file formatting issues.
* Updated ABP Framework to `10.0.1` and LeptonX to `5.0.1`.
* Added MySQL compatibility warnings.
* Fixed initial tasks problems.
* Improved AI Assistant control UI with better margins and borders.

## 2.0.2 (2025-11-26)

* Fixed `.NET 10` installation problems.
* Added custom styles for code blocks in **Markdown** view.
* Fixed `OpenIddictCoreOptions` injection to use `IOptions`.
* Added IdentityModel package after KubernetesClient.

## 2.0.1 (2025-11-21)

* Added build step before adding EF Core migration.
* Updated `KubernetesClient` to version `18.0.5`.

## 2.0.0 (2025-11-20)

* Major upgrade to `.NET 10.0` and `ABP Framework 10.0`
* Replaced `IdentityModel` with `Duende.IdentityModel`.
* Added "Open on Start in Browser" option for .NET applications in Solution Runner.
* Added `Mapperly` configuration.
* Enabled user and tenant impersonation in Blazor client modules.
* Enhanced notification system to allow text copying.
* Added environment variable support for DesignTime DbContext.
* Used C# instead of JSON for Aspire AppHost project configuration.
* Fixed MongoDB image pulling problems.
* Improved AI Assistant with better code block visibility across themes.
* Added different cache paths for each browser instance.
* Fixed various UI issues including mouse pointer problems in trees and horizontal scrolling.
* Added `FileManagement` download URL configuration for tiered projects.
* Added chat SignalR configuration to Microservice Blazor apps.
* Updated `Blazorise` packages to version `1.8.6`.
* Fixed `BackToImpersonator` button in Microservice Template.
* Added log recording while crashing for better debugging.
* Enhanced tab headers for **Solution Runner** and **Kubernetes**.

## 1.4.2 (2025-10-30)

* Upgraded template dependencies for ABP Framework and LeptonX. (targeting ABP `9.3.6`)
* AI Assistant is now enabled for all customers.
* Fixed CLI default language problem during solution creation.
* Improved task auto-start logic and notification handling.
* Fixed Angular localization function inputs.
* Set default mobile frameworks to **None** in the UI.
* Disallowed dots (.) in module names of microservice sub-templates.
* Solution Runner: show vertical scrollbar when needed and disable the Properties window while the app is running.

## 1.4.1 (2025-10-16)

* Fixed AI Assistant chat problems.
* Added custom steps if built with CLI.
* Fixed Release configuration builds.   

## 1.4.0 (2025-10-15)

* The **Task Panel** has been introduced, providing a centralized place to manage and monitor background operations.
* Added **CLI application properties** window, making it easier to configure and manage command-line tool settings directly within the Studio UI.
* Added suggestion modal for building after creating service/web/gateway module.
* Fixed mismatching hosts file record namespace problem.
* Allow selecting `Default Profile` in Solution Runner.
* Refactored Angular scripts.
* Fixed: tools not browsable in Solution Runner with Aspire after Kubernetes deployment.

## 1.3.3 (2025-10-06)

* Upgraded template dependencies for ABP Framework and LeptonX. (targeting ABP `9.3.5`)
* Fixed welcome page tutorial links.
* Improved error handling during Helm chart installation and custom command execution.
* Fixed microservice problems.
* Fixed connection string problems.

## 1.3.2 (2025-09-25)

* Enhanced AI Assistant with bug fixes and improvements.
* Implemented new public website layout for public website projects.
* Added container priority setting in Solution Runner.
* Fixed relative image path problems in markdown files and added SVG support.
* Enhanced Angular templates with application builder support.
* Fixed Aspire profile database creation issues in microservice template.

## 1.3.1 (2025-09-22)

* Added Blazor WebApp application information to ReadMe in application template.

## 1.3.0 (2025-09-22)

* Upgraded template dependencies for ABP Framework and LeptonX. (targeting ABP `9.3.4`)
* **Added .NET Aspire Integration** to ABP Studio and Microservice Startup Template.
* **Introduced AI Support Assistant** for enhanced development experience.
* Added new package option: **C# Console Application (With ABP)**.
* Enhanced Solution Runner with double-click browse functionality.
* Made Blazor WebApp option available for module templates.
* Updated React Native templates to use latest Expo/React Native standards.
* Removed LeptonX Theme Management by default from templates.
* Added Scriban template build-time validation.
* Enhanced MVC UI layer with localization and loading indicators.

## 1.2.2 (2025-08-27)

* Upgraded template dependencies for ABP Framework and LeptonX. (targeting ABP `9.3.2`)
* Fixed LeptonX Lite logo problems.
* Redesigned LeptonX footer component.
* Enhanced language selection with sorting by display name.
* Improved template configuration with default language handling.
* Optimized search depth for restore need detection.

## 1.2.1 (2025-08-14)

* Upgraded template dependencies for ABP Framework and LeptonX. (targeting ABP `9.3.1`)
* Migrated templates to standalone structure for Angular UI.
* Allowed relating tools with Kubernetes Services (allows to browse tools dashboard those are running in k8s).
* Made several enhancements for Solution Runner.
* Added test projects to **Application (Single Layer)** template (optional).


## 1.1.2 (2025-07-31)

* Upgraded template dependencies for ABP Framework and LeptonX. (targeting ABP `9.2.3`)
* Configured LeptonX Lite logos in the templates.
* Added browser tab memory feature to remember previously selected tabs.
* Enhanced tools section with default credentials display for first-time tool usage.
* Improved module and package loading with better error handling.

## 1.1.1 (2025-07-22)

* Enhanced tools section with clear cookies option.
* Fixed language management module name display for imported modules.
* Improved update window messaging with "Skip this version" option.
* Fixed Docker Compose file issues in microservice template.
* Resolved RabbitMQ tool cookie problems.

## 1.1.0 (2025-07-16)

* Upgraded template dependencies for ABP Framework and LeptonX. (targeting ABP `9.2.2`)
* Enhanced UI scaling for all windows and improved user experience.
* Added tools section in solution runner main area with basic Grafana dashboard for microservice template.
* Improved container management during application building.
* Enhanced background task exception handling.
* Added public account module package reference to Blazor WebApp client.
* Fixed tenant database context updating errors.
* Improved optional module selection UI with better documentation integration.

## 1.0.2 (2025-06-24)

* Enhanced the ABP NuGet package installation experience.
* Upgraded template dependencies for ABP Framework and LeptonX. (targeting ABP `9.2.1`)
* Replaced the `System.Data` package with `Microsoft.Data`.
* Fixed a dynamic-env file path configuration issue in Angular templates.
* Disabled Pushed Authorization for MAUI applications.
* Improved the IDE experience by displaying the main project in a dedicated 'main' folder and hiding `.abppkg` files.

## 1.0.1 (2025-06-13)

* Fixed an issue with language selection during solution creation.
* Resolved a logo visibility problem in the Angular semi-dark theme.
* Added and corrected the handling of CEF (Chromium Embedded Framework) resources for the Windows version. 

## 1.0.0 (2025-06-11)

* **Solution Runner with Health Checks:** ABP Studio's Solution Runner now provides visual health monitoring that makes tracking your applications' status easily.
* **Improved Multi-DbContext Migration Handling:** ABP Studio now prompts you to select the correct DbContext for migration operations when working with multiple DbContexts.
* **Theme Style Selection on Project Creation:** When creating a new solution, you can now choose your theme, theme style, and layout right from the project creation wizard instead of having to configure these settings later.
* **Solution & Module Creation:** Introduced major enhancements, including language selection, database provider choice for microservices, improved folder handling, theme visualization, and better module installation recommendations.
* **MAUI & Blazor:** Configured the new MAUI/Blazor bundling system, added dashboard pages to MAUI projects, and applied various fixes for themes and dependencies.
* **Solution Runner & Docker:** Added Docker container support to the solution runner, enabling users to add and manage containers within run profiles.
* **Authentication & Authorization:** Fixed Swagger authentication, and added dynamic claims support for microservices.
* **ABP Studio Login:** Improved login flows with selecting account and organization support.
* **Language Selection:** Added language selection during solution creation. You can now include only the languages you need in your project.
* **Performance:** Sped up the development cycle by skipping package restores during project runs when no dependencies have changed.
* **Dependency Updates:** Upgraded ABP Framework, LeptonX, and other Microsoft dependencies to the latest versions. (targeting ABP `9.2.0`)
* **User Experience:** Implemented several UI/UX improvements, such as remembering user choices in wizards and sorting items alphabetically.
* **Testing & Internals:** Switched to `MongoSandbox` for integration tests, improved local development against abp.io websites, and made various fixes to CI/CD workflows.

## 0.9.26 (2025-04-30)

* Fixed the issue where C# applications would not stop when requested.  
* Added idle session timeout feature to Blazor WebAssembly applications.  
* Added “Setup as a modular solution” option to application startup templates.  
* Automatically added remote service base URL after generating C# proxies.
* Configured Helm charts for Kubernetes health check endpoints.  
* Fixed auditing issue in Blazor WebAssembly applications.  
* Fixed login error after registering a new user in Blazor WebApp.  
* Implemented password login flow in Studio CLI.  
* Supported non-root user mode in Docker Compose configurations.
* Upgraded templates to version `9.1.1`.

## 0.9.25 (2025-03-12)

* Added ready/health check for solution runner.

## 0.9.24 (2025-03-11)

* Added automatic installation of necessary dependencies.
* Added user feedback collection after the first week of application usage.
* Added ability to add business services while creating a new microservice solution.
* Fixed database migration issue during module installation.
* Fixed angular ESLint dependency issue.
* Upgraded templates to version `9.1.0`.

## 0.9.23 (2025-02-04)

* Fixed **Open with Terminal** option not working on macOS.
* Fixed dynamic port assignment for Abp Suite if the default port is unavailable.
* Added suite templates package to module templates.
* Added warning message in CLI if the connection string is broken.

## 0.9.22 (2025-01-22)

* Allowed to display multiple installation notes.
* Showed **Inner Exceptions** in the Solution Runner side-panel
* Hidden logs when no selected application is present.
* Added full docker compose to template.
* Zipped microservice module template for better structuring the solution.
* Upgraded templates to version `9.0.4`.

## 0.9.21 (2025-01-09)

* Showed a db test connection message while testing the database connection.
* Fixed books sample's application service.

## 0.9.20 (2025-01-08)

* Upgraded templates to version `9.0.3`.
* Fixed Invariant Culture problem in source code downloading.
* Added missing linux support to OldCliInstaller
* Increased database test connection timeout up to 10seconds.

## 0.9.19 (2025-01-02)

* Disabled auto-scroll when scrolled up in the logging section.
* Added localhost development certificate check during solution load.
* Added testing connection string in project creation.
* Made enhancements for exception handling.

## 0.9.18 (2024-12-24)

* Fixed Blazor WebApp Kubernetes problems.
* Added Visual Studio & Rider options to solution root.
* Fixed problems in blazor-server nolayers template.

## 0.9.17 (2024-12-17)

* Added social login option to the "No Layers" Blazor WebAssembly template.
* Fixed AutoMapper missing configuration exception problem during module import.
* Fixed Blazor WebAssembly build issue for the MAUI template.
* Fixed a problem that prevented ABP Studio from opening on macOS.

## 0.9.16 (2024-12-11)

> This version does not work for macOS, we are currently working on that manner.

* Added a new command for refreshing the previously runned C#/Js Proxies.
* Added kubernetes configurations to Blazor WebApp for Microservice Template.
* Handled multiple dbcontexts for ef core migration operations.
* Made enhancements for dynamic localization feature in Microservice Template.
* Upgraded LeptonX Theme package versions to `4.0.2`.

## 0.9.15 (2024-12-05)

* Upgraded templates to version `9.0.1`.
* Fixed problems in the microservice service_nolayers template.
* Fixed microservice angular template for wrong file-management module reference.
* Fixed added extra lines in the hosts.txt file.

## 0.9.14 (2024-12-03)

* Refactored `dotnet watch` command in solution runner.
* Added multi-tenancy option for open source startup templates.
* Implemented adding angular library when a new microservice is created.
* Adjusted *Optional Modules* section in solution configurations
* Fixed bugs in the nolayers host project.

## 0.9.13 (2024-11-25)

* Angular - Theme-based Fixes for the Home Page.

## 0.9.12 (2024-11-25)

* Handled the `DynamicPermissionDefinitionsChangedEto` event to automatically add permissions for the admin role.
* Enhanced the Solution Configuration window with a more intuitive design and updated content.
* Improved MAUI application support by displaying all target frameworks in the Solution Runner and automatically setting the appropriate targetFramework based on the operating system.

## 0.9.11 (2024-11-21)

* Fixed the extension loading problem occured in v0.9.9 & v0.9.10.

## 0.9.10 (2024-11-21)

> Recommended to use v0.9.11+ for .NET 9.

* Added shortcut for Build & Start operation (CTRL + Click)
* Fixed extension loading loop problem in v0.9.9
* Fixed MAUI template for android

## 0.9.9 (2024-11-21)

> Recommended to use v0.9.11+ for .NET 9.

* Upgraded templates to .NET 9
* Fixed blazor wasm bundle problem in microservice template

## 0.9.8 (2024-11-20)

* Upgraded templates to version `8.3.4`

## 0.9.7 (2024-11-19)

* Added `AppearanceStyles` component to blazor server templates
* Fixed module import window
* Made several enhancements to the existing features

## 0.9.6 (2024-11-15)

* Added missing imports to templates
* Fixed bugs during EF Core package installation
* Show errors to the user when adding a database migration
* Changed empty solution description on create new solution wizard
* Fixed problems with templates created with Basic Theme
* Ensure the correct version is used when adding a new module/package to an existing solution

## 0.9.5 (2024-11-06)

* Added dynamic localization option to microservice template
* Added new template creation options for the Application (No Layers)
* Fixed the environment variable setting for .NET global tool and verification of tool installation
* Fixed log view auto-scrolling issue, ensuring smooth scrolling experience
* Upgraded templates to version `8.3.3`

## 0.9.4 (2024-10-31)

* Made the `TopMenuLayout` as the default layout type for microservice public website
* Fixed application crashing problem when the ABP Suite is opened

## 0.9.3 (2024-10-30)

* Added the **standard module template**
* Made enhancements on the pre-integrated browser
* Fixed Blazor WebAssembly UI being not run on kubernetes problem
* Added a database migration after a module added to the solution

## 0.9.2 (2024-10-22)

* Added a status bar to the pre-integrated browser for showing errors
* Added **Sample CRUD Page** option to pro templates
* Added test projects optionally for all templates
* Added **AutoMapper** configurations to microservice host projects
* Disabled **transaction** for *MongoDB* & *SQLite* by default.

## 0.9.1 (2024-10-10)

* Fixed the ABP Studio CLI's Bundle Command
* Fixed the Public Web project for the Microservice Template
* Removed the React Native for the Community Edition (open-source)
* Added multiple gateway and UI selection option during microservice creation
* Added External Logins item to user menu for the Blazor templates

## 0.8.4 (2024-10-07)

* Fixed the ABP Suite does not open problem for macOS
* Made improvements on the new microservice creation
* Allowed using browser shortcuts (copy, paste, new tab etc.) for macOS
* Prevented application being crashed on solution runner exceptions
* Included `WebApp.Client` project styles in the main application to enable CSS in Isolation

## 0.8.3 (2024-09-24)

* Allowed to set Execution Order (or dependency) from Solution Runner
* Added Icons for the Solution Runner's Context Menu
* Fixed MongoDB database issues for the Microservice Template
* Allowed parallel running for background tasks

## 0.8.2 (2024-09-19)

* Fixed WireGuard connection random port bug
* Automated steps after microservice solution creation
* Fixed unusable options/features for Trial License
* Fixed blazor-server single-layer template

## 0.8.1 (2024-09-12)

* Fixed MySQL connection problem for nolayers template
* Ignored failed solution runner profiles while loading
* Added required ModalBuilder extensions for the imported/installed modules for EF Core

## 0.8.0 (2024-09-11)

* Added `Blazor WebApp UI` to **app-nolayers** and **microservice** templates
* Fixed version problem for bundle command
* Fixed optional module integrations
* Added **LeptonX Theme** to module-list
* Fixed bug related to Kubernetes user-specified connection

## 0.7.9 (2024-08-29)

* Opened Readme.md file after module creation or opening a solution
* Refactor & made enhancements to microservice template
* Fixed some bugs that occur while creating a new module
* Synchronized new templates with the old templates
* Angular: Fixed logo problem in templates

## 0.7.8 (2024-08-23)

* Revised the new solution creation wizard and improved for UX
* Fixed bugs on the Blazor WebApp template
* Reduced logs count per application

## 0.7.7 (2024-08-14)

* Updated LeptonX logos in new templates
* ABP Suite: Opened modules with .sln path instead of directory (fixes some problems in Suite integration)
* Enhancements on Solution Runner

## 0.7.6 (2024-08-12)

* Removed redundant helm repo installation
* Made enhancements to the Welcome Page area
* Microservice Template: Enabled `DynamicPermissionStore` in AuthServer
* Added Blazor WebApp option for Application template
* Added **Solution Creation Info** action for identifying the solution

## 0.7.5 (2024-08-06)

* Fixed ABP Suite integration for macOS
* Fixed build errors after upgrading to ABP 8.2.1
* ABP Suite: Detected version inconsistencies and notified users to fix them automatically

## 0.7.4 (2024-07-31)

* Allowed creating open-source templates for active license owners
* Fixed basic theme problems in angular templates
* Handled docker related errors in solution runner
* Fixed versions of Chat Module packages

## 0.7.3 (2024-07-27)

* Added administrator mode indicator on the application title
* Made enhancements for aligning application title on macOS

## 0.7.2 (2024-07-26)

* Added `Blazor.Client` packages for alignment with new hosting logic
* Updated Microsoft packages to v8.0.4
* Revised application upgrade process
* Fixed profile photo problem in tiered projects
* Removed **LinkedAccounts** and **AuthorityDelegation** menu from templates

## 0.7.1 (2024-07-24)

* Fixed restart problem after installing extensions on macOS
* Provided a model for sharing the same Kubernetes cluster between developers
* Fixed the **Text Template Management** Module build problem in the app template
* Fixed login problem for the community edition
* Fixed all reported bugs & made enhancements

## 0.7.0 (2024-07-17)

* Added an option for building & starting C# applications directly
* Disabled redis on migrator when the solution is tiered
* Allowed Generating Static Proxies on ABP Studio UI 

## 0.6.9 (2024-06-27)

* Allowed to open ABP Studio by clicking on a `*.abpsln` file
* Fixed solution runner multiple instance problem
* Allowed to install an NPM package to a package

## 0.6.8 (2024-06-13)

* Added free to pro upgrade option
* Fixed microservice template helm chart bugs
* Made enhancements to the new CLI

## 0.6.7 (2024-05-31)

* Added free option to app templates
* Made social login optional for the app template
* Added open-source (free) templates
* Started showing angular projects on the browser tab
* Introduced the Community Edition
