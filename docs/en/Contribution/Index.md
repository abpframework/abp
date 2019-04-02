**DRAFT**: This doc has been created as a draft. 
See https://help.github.com/en/articles/setting-guidelines-for-repository-contributors

# Contributing to ASP.NET Boilerplate

ASP.NET Boilerplate is an [open source](https://github.com/aspnetboilerplate/aspnetboilerplate) and community driven project. This guide is aims to help anyone wants to contribute to the project.

## Code Contribution

You can always send pull requests to the GitHub repository.

- Clone the [ASP.NET Boilerplate repository](https://github.com/aspnetboilerplate/aspnetboilerplate/) from GitHub.
- Make the required changes.
- Send a pull request.

Before making any change, please discuss it on the [GitHub issues page](https://github.com/aspnetboilerplate/aspnetboilerplate/issues). So that, other developers will not work on the same issue and your PR will have a better chance to be accepted.

### Bug Fixes & Enhancements

You may want to fix a known bug or work on a planned enhancement. See [the issue list](https://github.com/aspnetboilerplate/aspnetboilerplate/issues) on GitHub.

### Feature Requests

If you have a feature idea for the framework or modules, [create an issue](https://github.com/aspnetboilerplate/aspnetboilerplate/issues/new) on GitHub or attend to an existing discussion. Then you can implement it if it's embraced by the community.

## Document Contribution

You may want to improve the [documentation](https://aspnetboilerplate.com/Pages/Documents). If so, follow these steps:

* Clone the [ABP repository](https://github.com/aspnetboilerplate/aspnetboilerplate/) from GitHub.
* Documents are located in [/aspnetboilerplate/doc](https://github.com/aspnetboilerplate/aspnetboilerplate/tree/master/doc/WebSite) folder.
* Modify the documents and send pull request
* If you would like to add a new document, you need to add it to the navigation document as well. Navigation document is located in [doc/WebSite/Navigation.md](https://github.com/aspnetboilerplate/aspnetboilerplate/blob/master/doc/WebSite/Navigation.md).

## Resource Localization

ASP.NET Boilerplate framework has a [localization system](https://aspnetboilerplate.com/Pages/Documents/Localization). Localization resources are located in [Abp\Localization\Sources\AbpXmlSource](https://github.com/aspnetboilerplate/aspnetboilerplate/tree/dev/src/Abp/Localization/Sources/AbpXmlSource). 
You can add a new translation or update existing ones.
To add missing translation, see [this example pull request](https://github.com/aspnetboilerplate/aspnetboilerplate/pull/2471)

## Writing a New Module

The framework has pre-build modules, you can also add a new module. [Abp.Dapper](https://github.com/aspnetboilerplate/aspnetboilerplate/tree/dev/src/Abp.Dapper) is a contributed module. You can check Abp.Dapper module to make your own.

TODO: May be added step by step module development guide.

## Blog Posts & Tutorials

If you would like to write tutorials or blog posts for ASP.NET Boilerplate, please let us know (by creating a GitHub issue](https://github.com/aspnetboilerplate/aspnetboilerplate/issues), so we may add a link to your tutorial/post in the official documentation and we announce it on the official [Twitter account](https://twitter.com/aspboilerplate).

## Bug Report

If you would like to report a bug, please [create an issue on the GitHub repository](https://github.com/aspnetboilerplate/aspnetboilerplate/issues/new)

You need to fill out the issue template before posting a bug.

```markdown
### GitHub Issues

GitHub issues are for bug reports, feature requests and other discussions about the framework.

If you're creating a bug/problem report, please include followings:

* Your Abp package version.
* Your base framework: .Net Framework or .Net Core.
* Exception message and stack trace if available.
* Steps needed to reproduce the problem.

Please write in English.

### Stack Overflow

Please use Stack Overflow for your questions about using the framework, templates and samples:

https://stackoverflow.com/questions/tagged/aspnetboilerplate

Use aspnetboilerplate tag in your questions.

```
