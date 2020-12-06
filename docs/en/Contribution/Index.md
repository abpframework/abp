# Contribution Guide

ABP is an [open source](https://github.com/abpframework) and community driven project. This guide is aims to help anyone wants to contribute to the project.

## ABP Community Website

If you want to write **articles** or **how to guides** related to the ABP Framework and ASP.NET Core, please submit your article to the [community.abp.io](https://community.abp.io/) website.

## Code Contribution

You can always send pull requests to the GitHub repository.

- Clone the [ABP repository](https://github.com/abpframework/abp/) from GitHub.
- Build the repository using the `/build/build-all.ps1 -f` for one time.
- Make the necessary changes, including unit/integration tests.
- Send a pull request.

> When you open a solution in Visual Studio, you may need to execute `dotnet restore` in the root folder of the solution for one time, after it is fully opened in the Visual Studio. This is needed since VS can't properly resolves local references to projects out of the solution.

### GitHub Issues

Before making any change, please discuss it on the [Github issues](https://github.com/abpframework/abp/issues). In this way, no other developer will work on the same issue and your PR will have a better chance to be accepted.

#### Bug Fixes & Enhancements

You may want to fix a known bug or work on a planned enhancement. See [the issue list](https://github.com/abpframework/abp/issues) on Github.

#### Feature Requests

If you have a feature idea for the framework or modules, [create an issue](https://github.com/abpframework/abp/issues/new) on Github or attend to an existing discussion. Then you can implement it if it's embraced by the community.

## Document Translation

You may want to translate the complete [documentation](https://docs.abp.io) (including this one) to your mother language. If so, follow these steps:

* Clone the [ABP repository](https://github.com/abpframework/abp/) from Github.
* To add a new language, create a new folder inside the [docs](https://github.com/abpframework/abp/tree/master/docs) folder. Folder names can be "en", "es", "fr", "tr" and so on based on the language (see [all culture codes](https://msdn.microsoft.com/en-us/library/hh441729.aspx)).
* Get the ["en" folder](https://github.com/abpframework/abp/tree/master/docs/en) as a reference for the file names and folder structure. Keep the same naming if you are translating the same documentation.
* Send a pull request (PR) once you translate any document. Please translate documents & send PRs one by one. Don't wait to finish translations for all documents.

There are some fundamental documents need to be translated before publishing a language on the [ABP documentation web site](https://docs.abp.io):

* Index (Home)
* Getting Started
* Web Application Development Tutorial

A new language is published after these minimum translations have been completed.

## Resource Localization

ABP framework has a flexible [localization system](../Localization.md). You can create localized user interfaces for your own application.

In addition to that, the framework and the [pre-build modules](https://docs.abp.io/en/abp/latest/Modules/Index) have localized texts. As an example, see [the localization texts for the Volo.Abp.UI package](https://github.com/abpframework/abp/blob/master/framework/src/Volo.Abp.UI/Localization/Resources/AbpUi/en.json). 

### Using the "abp translate" command

This is the recommended approach, since it automatically finds all missing texts for a specific culture and lets you to translate in one place.

* Clone the [ABP repository](https://github.com/abpframework/abp/) from Github.
* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed before.
* Run `abp translate -c <culture-name>` command for your language in the root folder of the abp repository. For example, use `abp translate -c fr` for French. Check [this document](https://docs.microsoft.com/en-us/bingmaps/rest-services/common-parameters-and-types/supported-culture-codes) to find the culture code for your language.
* This command creates a file in the same folder, named `abp-translation.json`. Open this file in your favorite editor and fill the missing text values.
* Once you done the translation, use `abp translate -a` command to apply changes to the related files.
* Send a pull request on GitHub.

### Manual Translation

If you want to make a change on a specific resource file, you can find the file yourself, make the necessary change (or create a new file for your language) and send a pull request on GitHub.

## Bug Report

If you find any bug, please [create an issue on the Github repository](https://github.com/abpframework/abp/issues/new).
