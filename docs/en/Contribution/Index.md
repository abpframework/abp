## Contribution Guide

ABP is an [open source](https://github.com/abpframework) and community driven project. This guide is aims to help anyone wants to contribute to the project.

### Code Contribution

You can always send pull requests to the Github repository.

- Clone the [ABP repository](https://github.com/abpframework/abp/) from Github.
- Make the required changes.
- Send a pull request.

Before making any change, please discuss it on the [Github issues](https://github.com/abpframework/abp/issues). In this way, no other developer will work on the same issue and your PR will have a better chance to be accepted.

#### Bug Fixes & Enhancements

You may want to fix a known bug or work on a planned enhancement. See [the issue list](https://github.com/abpframework/abp/issues) on Github.

#### Feature Requests

If you have a feature idea for the framework or modules, [create an issue](https://github.com/abpframework/abp/issues/new) on Github or attend to an existing discussion. Then you can implement it if it's embraced by the community.

### Document Translation

You may want to translate the complete [documentation](https://abp.io/documents/) (including this one) to your mother language. If so, follow these steps:

* Clone the [ABP repository](https://github.com/abpframework/abp/) from Github.
* To add a new language, create a new folder inside the [docs](https://github.com/abpframework/abp/tree/master/docs) folder. Folder names can be "en", "es", "fr", "tr" and so on based on the language (see [all culture codes](https://msdn.microsoft.com/en-us/library/hh441729.aspx)).
* Get the ["en" folder](https://github.com/abpframework/abp/tree/master/docs/en) as a reference for the file names and folder structure. Keep the same naming if you are translating the same documentation.
* Send a pull request (PR) once you translate any document. Please translate documents & send PRs one by one. Don't wait to finish translations for all documents.

There are some fundamental documents need to be translated before publishing a language on the [ABP documentation web site](https://docs.abp.io):

* Getting Started documents
* Tutorials
* CLI

A new language is published after these minimum translations have been completed.

### Resource Localization

ABP framework has a flexible [localization system](../Localization.md). You can create localized user interfaces for your own application.

In addition to that, the framework and pre-build modules have already localized texts. As an example, see [the localization texts for the Volo.Abp.UI package](https://github.com/abpframework/abp/blob/master/framework/src/Volo.Abp.UI/Localization/Resources/AbpUi/en.json). You can create a new file in the [same folder](https://github.com/abpframework/abp/tree/master/framework/src/Volo.Abp.UI/Localization/Resources/AbpUi) to translate it.

* Clone the [ABP repository](https://github.com/abpframework/abp/) from Github.
* Create a new file for the target language for a localization text (json) file (near to the en.json file).
* Copy all texts from the en.json file.
* Translate the texts.
* Send pull request on Github.

ABP is a modular framework. So there are many localization text resource, one per module. To find all .json files, you can search for "en.json" after cloning the repository. You can also check [this list](Localization-Text-Files.md) for a list of localization text files.

### Blog Posts & Tutorials

If you decide to create some tutorials or blog posts on ABP, please inform us (by creating a [Github issue](https://github.com/abpframework/abp/issues)), so we may add a link to your tutorial/post in the official documentation and we can announce it on our [Twitter account](https://twitter.com/abpframework).

### Bug Report

If you find any bug, please [create an issue on the Github repository](https://github.com/abpframework/abp/issues/new).
