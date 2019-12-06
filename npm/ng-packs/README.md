<h1> Abp Ng Packages </h1>


## Getting started

Run `yarn` to install all dependencies, then run `yarn prepare:workspace` to prepare the ABP packages (might take 2 minutes).

Run `yarn start` to start the `dev-app`. Navigate to http://localhost:4200/.

## Development

### Package
[Symlink Manager](https://github.com/mehmet-erim/symlink-manager) is used to manage symbolic link processes. Run `yarn symlink copy` to select the packages to develop.

> Ignore the changes in the dist folder. The changes should be discarded.

### Application
The `dev-app` project is the same as the Angular UI template project. `dev-app` is used to see changes instantly.

If you will only develop the `dev-app`, you don't need to run `symlink-manager`.

> Reminder! If you have developed the `dev-app` template, you should do the same for the application and module templates.

For more information, see the [docs.abp.io](https://docs.abp.io)
