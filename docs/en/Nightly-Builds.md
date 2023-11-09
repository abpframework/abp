# Nightly Builds

All framework & module packages (both open-source and commercial) are deployed to MyGet every night on weekdays. So, you can install the latest dev-branch builds to try out functionality prior to release.

## Install & Uninstall Nightly Preview Packages

The latest version of nightly preview packages can be installed by running the below command in the root folder of the application:

```bash
abp switch-to-nightly 
```

> Note that this command doesn't create a project with nightly preview packages. Instead, it switches package versions of a project with the nightly preview packages.

After this command, a new NuGet feed will be added to the `NuGet.Config` file of your project. Then, you can get the latest code of the ABP Framework without waiting for the next release.

> ABP nightly NuGet feed is [https://www.myget.org/F/abp-nightly/api/v3/index.json](https://www.myget.org/F/abp-nightly/api/v3/index.json).

Also, this command creates `.npmrc` files containing two NPM registries in the directory where the `package.json` files are located in your solution.

> You can check the [abp-nightly gallery](https://www.myget.org/gallery/abp-nightly) (for NPM & NuGet / open-source) and [abp-commercial-npm-nightly gallery](https://www.myget.org/gallery/abp-commercial-npm-nightly) (for NPM / commercial) to see the all nightly preview packages.

If you're using the ABP Framework nightly preview packages, you can switch back to the stable version by using this command:

```bash
abp switch-to-stable
```

See the [ABP CLI documentation](./CLI.md) for more information.
