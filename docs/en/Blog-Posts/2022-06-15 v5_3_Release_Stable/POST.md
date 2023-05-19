# ABP.IO Platform 5.3 Final Has Been Released!

[ABP Framework](https://abp.io/) and [ABP Commercial](https://commercial.abp.io/) 5.3 versions have been released today.

## What's New With 5.3?

Since all the new features are already explained in details with the [5.3 RC Announcement Post](https://blog.abp.io/abp/ABP.IO-Platform-5.3-RC-Has-Been-Published), I will not repeat all the details again. See the [RC Blog Post](https://blog.abp.io/abp/ABP.IO-Platform-5.3-RC-Has-Been-Published) for all the features and enhancements.

## Getting Started with 5.3

### Creating New Solutions

You can create a new solution with the ABP Framework version 5.3 by either using the `abp new` command or using the **direct download** tab on the [get started page](https://abp.io/get-started).

> See the [getting started document](https://docs.abp.io/en/abp/latest/Getting-Started) for more.

### How to Upgrade an Existing Solution

#### Install/Update the ABP CLI

First of all, install the ABP CLI or upgrade to the latest version.

If you haven't installed it yet:

```bash
dotnet tool install -g Volo.Abp.Cli
```

To update an existing installation:

```bash
dotnet tool update -g Volo.Abp.Cli
```

#### Upgrading Existing Solutions with the ABP Update Command

[ABP CLI](https://docs.abp.io/en/abp/latest/CLI) provides a handy command to update all the ABP related NuGet and NPM packages in your solution with a single command:

```bash
abp update
```

Run this command in the root folder of your solution.

## Migration Guides

Check the following migration guides for the applications with version 5.2 that are upgrading to version 5.3.

* [ABP Framework 5.2 to 5.3 migration guide](https://docs.abp.io/en/abp/5.3/Migration-Guides/Abp-5_3)
* [ABP Commercial 5.2 to 5.3 migration guide](https://docs.abp.io/en/commercial/5.3/migration-guides/v5_3)

## Community News

### New ABP Community Posts

Here are some of the recent posts added to the [ABP Community](https://community.abp.io/):

* [Integrating Elsa .NET Workflows with ABP Commercial](https://community.abp.io/posts/integrating-elsa-.net-workflows-with-abp-commercial-io32k420) by [kirtik](https://community.abp.io/members/kirtik).
* [ABP's Conventional Dependency Injection](https://community.abp.io/posts/abps-conventional-dependency-injection-948toiqy) by [iyilm4z](https://github.com/iyilm4z).
* [How to implement Single Sign-On with ABP commercial application](https://community.abp.io/posts/how-to-implement-single-signon-with-abp-commercial-application-m5ek38y9) by [kirtik](https://community.abp.io/members/kirtik).
* [Introduce DTM for Multi-Tenant Multi-Database Scene](https://community.abp.io/posts/introduce-dtm-for-multitenant-multidatabase-scene-23ziikbe) by [gdlcf88](https://github.com/gdlcf88).

Thanks to the ABP Community for all the contents they have published. You can also [post your ABP related (text or video) contents](https://community.abp.io/articles/submit) to the ABP Community.

## About the Next Version

The next feature version will be 6.0. It is planned to release the 6.0 RC (Release Candidate) on July 12 and the final version on August 16, 2022. You can follow the [release planning here](https://github.com/abpframework/abp/milestones).

Please [submit an issue](https://github.com/abpframework/abp/issues/new) if you have any problems with this version.
