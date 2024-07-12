# Preview Releases

The preview versions are released **~4 weeks before** releasing a major or minor (feature) version of ABP. They are released for developers to try and provide feedback to have more stable versions.

Versioning of a preview release is like that:

* 7.1.0-rc.1
* 8.0.0-rc.1
* 8.0.0-rc.2

More than one preview releases (like 8.1.0-rc.2 and 8.1.0-rc.3) might be published until the stable version (like 8.1.0).

## Using the Preview Versions

### Update the CLI

Before creating or updating an existing solution make sure to update the CLI to the latest preview version, for example:

````bash
dotnet tool update --global Volo.Abp.Studio.Cli
````

### New Solutions

Creating new solutions with the preview version is not supported. All new solutions must be created with the stable version.

### Existing Solutions

If you already have a solution and want to use/test the latest preview version, use the following [ABP CLI](../cli) command in the root folder of your solution.

````bash
abp switch-to-preview
````

You can return back to the latest stable using the `abp switch-to-stable ` command later.

````bash
abp switch-to-stable
````

## Providing Feedback

You can open an issue on the [GitHub repository](https://github.com/abpframework/abp/issues/new), if you find a bug or want to provide any kind of feedback.
