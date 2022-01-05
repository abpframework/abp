# ABP.IO Platform 4.2 Final Has Been Released!

[ABP Framework](https://abp.io/) and [ABP Commercial](https://commercial.abp.io/) 4.2 versions have been released today.

## What's New With 4.2?

Since all the new features are already explained in details with the [4.2 RC Announcement Post](https://blog.abp.io/abp/ABP-IO-Platform-v4-2-RC-Has-Been-Released), I will not repeat all the details again. See the [RC Blog Post](https://blog.abp.io/abp/ABP-IO-Platform-v4-2-RC-Has-Been-Released) for all the features and enhancements.

## Creating New Solutions

You can create a new solution with the ABP Framework version 4.2 by either using the `abp new` command or using the **direct download** tab on the [get started page](https://abp.io/get-started).

> See the [getting started document](https://docs.abp.io/en/abp/latest/Getting-Started) for details.

## How to Upgrade an Existing Solution

### Install/Update the ABP CLI

First of all, install the ABP CLI or upgrade to the latest version.

If you haven't installed yet:

```bash
dotnet tool install -g Volo.Abp.Cli
```

To update an existing installation:

```bash
dotnet tool update -g Volo.Abp.Cli
```

### ABP UPDATE Command

[ABP CLI](https://docs.abp.io/en/abp/latest/CLI) provides a handy command to update all the ABP related NuGet and NPM packages in your solution with a single command:

```bash
abp update
```

Run this command in the root folder of your solution.

## Migration Guide

Check [the migration guide](https://docs.abp.io/en/abp/latest/Migration-Guides/Abp-4_2) for the applications with the version 4.x upgrading to the version 4.2.

> It is strongly recommended to check the migration guide for this version. Especially, the new `IRepository.GetQueryableAsync()` method is a core change should be considered after upgrading the solution.

## About the Next Version

The next feature version will be 4.3. It is planned to release the 4.3 RC (Release Candidate) on March 11 and the final version on March 25, 2021.

We decided to slow down the feature development for the [next milestone](https://github.com/abpframework/abp/milestone/49). We will continue to improve the existing features and introduce new ones, sure, but wanted to have more time for the planning, documentation, creating guides and improving the development experience.