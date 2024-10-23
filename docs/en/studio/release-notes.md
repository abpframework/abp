# ABP Studio Release Notes

This document contains **brief release notes** for each ABP Studio release. Release notes only include **major features** and **visible enhancements**. Therefore, they don't include all the development done in the related version. 

## 0.9.2 (2024-10-22)

* Added "Sample CRUD Page" option for pro templates
* Generated *Signing-Certificate* for appnolayers template
* Added test projects optionally for all templates
* Added automapper configuration to apps/web project for ms template
* Disabled *transaction* for `MongoDB` and `SQLite` by default

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