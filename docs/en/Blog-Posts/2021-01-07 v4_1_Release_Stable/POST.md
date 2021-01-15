# ABP.IO Platform 4.1 Final Has Been Released!

[ABP Framework](https://abp.io/) and [ABP Commercial](https://commercial.abp.io/) 4.1 versions have been released today.

## What's New With 4.1?

Since all the new features are already explained in details with the [4.1 RC Announcement Post](https://blog.abp.io/abp/ABP.IO-Platform-v4.1-RC-Has-Been-Released), I will not repeat all the details again. See the [RC Blog Post](https://blog.abp.io/abp/ABP.IO-Platform-v4.1-RC-Has-Been-Released) for all the features and enhancements.

## Creating New Solutions

You can create a new solution with the ABP Framework version 4.1 by either using the `abp new` command or using the **direct download** tab on the [get started page](https://abp.io/get-started).

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

## ABP Community

We started to get more contributions by the community for the [ABP Community](https://community.abp.io/) contents. Thank you all!

We will be adding **Video Content** sharing system in a short time. We are planning to create short video contents, especially to explore the new features in every release. Again, we will be waiting video contributions by the community :)

## About the Next Versions

Planned preview date for the version **4.2 is January 14, 2021**. See the [Road Map](https://docs.abp.io/en/abp/latest/Road-Map) document and [GitHub Milestones](https://github.com/abpframework/abp/milestones) to learn what's planned for the next versions. We are trying to be clear about the coming features and the next release dates.