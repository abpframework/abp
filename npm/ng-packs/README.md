# Abp Ng Packages

<a href="https://github.com/abpframework/abp/actions?query=workflow%3AAngular">![action badge](https://img.shields.io/github/workflow/status/abpframework/abp/Angular)</a>
<a href="https://github.com/abpframework/abp/labels/ui-angular">![issues](https://img.shields.io/github/issues/abpframework/abp/ui-angular)</a>
<a href="https://github.com/abpframework/abp/pulls?utf8=%E2%9C%93&q=is%3Apr+is%3Aopen+label%3Aui-angular">![pull requests](https://img.shields.io/github/issues-pr-raw/abpframework/abp/ui-angular)</a>
<a href="https://npmjs.org/package/@abp/ng.core">![npm](https://img.shields.io/npm/dm/@abp/ng.core)</a>
![npm version](https://img.shields.io/npm/v/@abp/ng.core?label=version)

## Getting started

Run `yarn` to install all dependencies, then run `yarn prepare:workspace` to prepare the ABP packages (might take 2 minutes).

Run `yarn start` to start the `dev-app`. Navigate to http://localhost:4200/.

## Development

### Package
[Symlink Manager](https://github.com/mehmet-erim/symlink-manager) is used to manage symbolic link processes. Run `yarn symlink copy` to select the packages to develop.

### Application
The `dev-app` project is the same as the Angular UI template project. `dev-app` is used to see changes instantly.

If you will only develop the `dev-app`, you don't need to run `symlink-manager`.

> Reminder! If you have developed the `dev-app` template, you should do the same for the application and module templates.

For more information, see the [docs.abp.io](https://docs.abp.io)

If would you like contribute, see the [contribution guideline](./CONTRIBUTING.md).