# ABP.IO Platform 8.3 Final Has Been Released!

![](cover-image.png)

[ABP](https://abp.io/) 8.3 stable version has been released today.

## What's New With Version 8.3?

All the new features were explained in detail in the [8.3 RC Announcement Post](https://blog.abp.io/abp/announcing-abp-8-3-release-candidate), so there is no need to review them again. You can check it out for more details. 

## Getting Started with 8.3

### Creating New Solutions

You can check the [Get Started page](https://abp.io/get-started) to see how to get started with ABP. You can either download [ABP Studio](https://abp.io/get-started#abp-studio-tab) (**recommended**, if you prefer a user-friendly GUI application - desktop application) or use the [ABP CLI](https://abp.io/docs/latest/cli) to create new solutions.

By default, ABP Studio uses stable versions to create solutions. Therefore, it will be creating the solution with the latest stable version, which is v8.3 for now, so you don't need to specify the version.

### How to Upgrade an Existing Solution

You can upgrade your existing solutions with either ABP Studio or ABP CLI. In the following sections, both approaches are explained:

### Upgrading via ABP Studio

If you are already using the ABP Studio, you can upgrade it to the latest version to align it with ABP v8.3. ABP Studio periodically checks for updates in the background, and when a new version of ABP Studio is available, you will be notified through a modal. Then, you can update it by confirming the opened modal. See [the documentation](https://abp.io/docs/latest/studio/installation#upgrading) for more info.

After upgrading the ABP Studio, then you can open your solution in the application, and simply click the **Switch to stable** action button to instantly upgrade your solution:

![](switch-to-stable.png)

### Upgrading via ABP CLI

Alternatively, you can upgrade your existing solution via ABP CLI. First, you need to install the ABP CLI or upgrade it to the latest version.

If you haven't installed it yet, you can run the following command:

```bash
dotnet tool install -g Volo.Abp.Studio.Cli
```

Or to update the existing CLI, you can run the following command:

```bash
dotnet tool update -g Volo.Abp.Studio.Cli
```

After installing/updating the ABP CLI, you can use the [`update` command](https://abp.io/docs/latest/CLI#update) to update all the ABP related NuGet and NPM packages in your solution as follows:

```bash
abp update
```

> You can run this command in the root folder of your solution.

## Migration Guides

There are a few breaking changes in this version that may affect your application. Please read the migration guide carefully, if you are upgrading from v8.2 or earlier: [ABP Version 8.3 Migration Guide](https://abp.io/docs/8.3/release-info/migration-guides/abp-8-3)

## Community News

### New ABP Community Posts

As always, exciting articles have been contributed by the ABP community. I will highlight some of them here:

* [Alper Ebicoglu](https://twitter.com/alperebicoglu) has created **three** new community articles:
    * [Do You Really Need Multi-tenancy?](https://abp.io/community/articles/do-you-really-need-multitenancy-hpwn44r3)
    * [What is Angular Schematics?](https://abp.io/community/articles/what-is-angular-schematics-2z4jusf5)
    * [Understanding Angular AOT vs JIT Compilations](https://abp.io/community/articles/understanding-angular-aot-vs-jit-compilations-0r0a0a3f)
* [Dynamic Widget Communication](https://abp.io/community/articles/dynamic-widget-communication-uvun7q23) by [Suhaib Mousa](https://suhaibmousa.com/)
* [Introducing the Google Cloud Storage BLOB Provider](https://abp.io/community/articles/introducing-the-google-cloud-storage-blob-provider-yrt6azc0) by [Engincan Veske](https://twitter.com/EngincanVeske)
* [Switching Between Organization Units](https://abp.io/community/articles/switching-between-organization-units-i5tokpzt) by [Liming Ma](https://github.com/maliming)

Thanks to the ABP Community for all the content they have published. You can also [post your ABP-related (text or video) content](https://abp.io/community/posts/submit) to the ABP Community.

## About the Next Version

The next feature version will be 9.0. You can follow the [release planning here](https://github.com/abpframework/abp/milestones). Please [submit an issue](https://github.com/abpframework/abp/issues/new) if you have any problems with this version.