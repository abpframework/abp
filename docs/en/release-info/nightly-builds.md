# Nightly Builds

All framework & module packages (both open-source and pro packages) are deployed to MyGet every night on weekdays. So, you can install the latest dev-branch builds to try out functionality prior to release.

## Install & Uninstall Nightly Preview Packages

The latest version of nightly preview packages can be installed by running the below command in the root folder of a solution:

```bash
abp switch-to-nightly
```

> `switch-to-nightly` command doesn't create a project with nightly preview packages. Instead, it switches package versions of an existing solution with the nightly preview packages. You can create a new solution with the latest version, then switch to nightly packages if you want.

After the `switch-to-nightly` command, a new NuGet feed will be added to the `NuGet.Config` file of your project. Then, you can get the latest code of ABP without waiting for the next release.

> ABP nightly NuGet feed is [https://www.myget.org/F/abp-nightly/api/v3/index.json](https://www.myget.org/F/abp-nightly/api/v3/index.json).

Also, the `switch-to-nightly` command creates `.npmrc` files containing two NPM registries in the directory where the `package.json` files are located in your solution.

> You can check the [ABP Nightly Gallery](https://www.myget.org/gallery/abp-nightly) (for the open source NPM & NuGet packages) and [ABP Pro Nightly Gallery](https://www.myget.org/gallery/abp-commercial-npm-nightly) (for the pro NPM packages) to see the all nightly preview packages.

If you're using the ABP nightly preview packages, you can switch back to the stable version by using the following command:

```bash
abp switch-to-stable
```

## See Also

[ABP CLI documentation](../cli)
