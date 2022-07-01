# Nightly Builds

All framework & module packages are deployed to MyGet every night in weekdays. So, you can use or test the latest code without waiting for the next release.

## Install & Uninstall Nightly Preview Packages

> This command doesn't create a project with nightly preview packages. Instead, it switches package versions of a project with the nightly preview packages.

The latest version of nightly preview packages can be installed by the running below command in the root folder of the application:

```bash
abp switch-to-nightly 
```

After this command, a new NuGet feed will be added to the `NuGet.Config` file of your project. Then, you can get the latest code of ABP Framework without waiting for the next release.

> You can check the [abp-nightly gallery](https://www.myget.org/gallery/abp-nightly) to see the all nightly preview packages.

If you're using the ABP Framework nightly preview packages, you can switch back to stable version using this command:

```bash
abp switch-to-stable
```

See the [ABP CLI documentation](./CLI.md) for more information.
