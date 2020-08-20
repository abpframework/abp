## Preview Releases

The preview versions are released 2 weeks before releasing a minor or major version of ABP Framework. They are released for developers to try and provide feedback to us to release more stable versions. 

### Using Preview Versions

#### New Solutions

To create a project for testing the preview version, you can select the "preview" option on the [download page](https://abp.io/get-started) or use the "**--preview**" parameter with the [ABP CLI](CLI.md) new command:

````bash
abp new Acme.BookStore --preview
````

This command will create a new project using the latest RC/Preview NuGet packages, NPM packages and solution template. Whenever the stable version is released, you can switch the packages to the stable version for your solution using the `abp switch-to-stable` command in the root folder of your solution.

#### Existing Solutions

If you already have a solution and want to use/test the latest RC/Preview version, use the following [ABP CLI](CLI.md) command in the root folder of your solution.

````bash
abp switch-to-preview
````

You can return back to the latest stable using the `abp switch-to-stable ` command later.

````bash
abp switch-to-stable
````

### Providing Feedback

You can open an issue on the [GitHub repository](https://github.com/abpframework/abp/issues/new), if you find a bug or want to provide any kind of feedback.