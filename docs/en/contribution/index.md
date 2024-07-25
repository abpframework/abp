# Contribution Guide

ABP is an [open source](https://github.com/abpframework) and community driven project. This guide is aims to help anyone wants to contribute to the project.

## ABP Community Website

If you want to write **articles** or **how to guides** related to the ABP and ASP.NET Core, please submit your article to the [abp.io/community](https://abp.io/community/) website.

## Code Contribution

You can always send pull requests to the GitHub repository.

- [Fork](https://docs.github.com/en/free-pro-team@latest/github/getting-started-with-github/fork-a-repo) the [ABP repository](https://github.com/abpframework/abp/) from GitHub.
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

## Resource Localization

ABP has a flexible [localization system](../framework/fundamentals/localization.md). You can create localized user interfaces for your own application.

In addition to that, the framework and the [pre-build modules](../modules) have localized texts. As an example, see [the localization texts for the Volo.Abp.UI package](https://github.com/abpframework/abp/blob/master/framework/src/Volo.Abp.UI/Localization/Resources/AbpUi/en.json). 

### Using the "abp translate" command

This is the recommended approach, since it automatically finds all missing texts for a specific culture and lets you to translate in one place.

* Clone the [ABP repository](https://github.com/abpframework/abp/) from Github.
* Install the [ABP CLI](../cli/index.md) if you haven't installed before.
* Run `abp translate -c <culture-name>` command for your language in the root folder of the abp repository. For example, use `abp translate -c fr` for French. Check [this document](https://docs.microsoft.com/en-us/bingmaps/rest-services/common-parameters-and-types/supported-culture-codes) to find the culture code for your language.
* This command creates a file in the same folder, named `abp-translation.json`. Open this file in your favorite editor and fill the missing text values.
* Once you done the translation, use `abp translate -a` command to apply changes to the related files.
* Send a pull request on GitHub.

### Manual Translation

If you want to make a change on a specific resource file, you can find the file yourself, make the necessary change (or create a new file for your language) and send a pull request on GitHub.

## Bug Report

If you find any bug, please [create an issue on the Github repository](https://github.com/abpframework/abp/issues/new).

## See Also

* [Contribution Guide for the Angular UI](angular-ui.md)
* [ABP Community Talks 2022.4: How can you contribute to the open source ABP?](https://www.youtube.com/watch?v=Wz4Z-O-YoPg&list=PLsNclT2aHJcOsPustEkzG6DywiO8eh0lB)