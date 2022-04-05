# ABP.IO Platform 5.2 Final Has Been Released!

[ABP Framework](https://abp.io/) and [ABP Commercial](https://commercial.abp.io/) 5.2 versions have been released today.

## What's New With 5.2?

Since all the new features are already explained in details with the [5.2 RC Announcement Post](https://blog.abp.io/abp/ABP.IO-Platform-5-2-RC-Has-Been-Published), I will not repeat all the details again. See the [RC Blog Post](https://blog.abp.io/abp/ABP.IO-Platform-5-2-RC-Has-Been-Published) for all the features and enhancements.

## Creating New Solutions

You can create a new solution with the ABP Framework version 5.2 by either using the `abp new` command or using the **direct download** tab on the [get started page](https://abp.io/get-started).

> See the [getting started document](https://docs.abp.io/en/abp/latest/Getting-Started) for more.

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

Check [the migration guide](https://docs.abp.io/en/abp/5.2/Migration-Guides/Abp-5_2) for the applications with the version 5.x upgrading to the version 5.2.

## About the Next Version

The next feature version will be 5.3. It is planned to release the 5.3 RC (Release Candidate) on May 03 and the final version on May 31, 2022. You can follow the [release planning here](https://github.com/abpframework/abp/milestones).

Please [submit an issue](https://github.com/abpframework/abp/issues/new) if you have any problem with this version.
