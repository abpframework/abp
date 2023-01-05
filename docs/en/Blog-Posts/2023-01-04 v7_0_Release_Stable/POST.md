# ABP.IO Platform 7.0 Final Has Been Released!

[ABP Framework](https://abp.io/) and [ABP Commercial](https://commercial.abp.io/) 7.0 versions have been released today.

## What's New With 7.0?

Since all the new features are already explained in detail in the [7.0 RC Announcement Post](https://blog.abp.io/abp/ABP.IO-Platform-7.0-RC-Has-Been-Published), I will not repeat all the details again. See the [RC Blog Post](https://blog.abp.io/abp/ABP.IO-Platform-7.0-RC-Has-Been-Published) for all the features and enhancements.

## Getting Started with 7.0

### Creating New Solutions

You can create a new solution with the ABP Framework version 7.0 by either using the `abp new` command or generating the CLI command on the [get started page](https://abp.io/get-started).

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

There are breaking changes in this version that may affect your application. Please see the following migration documents, if you are upgrading from v6.x:

* [ABP Framework 6.x to 7.0 Migration Guide](https://docs.abp.io/en/abp/7.0/Migration-Guides/Abp-7_0)
* [ABP Commercial 6.x to 7.0 Migration Guide](https://docs.abp.io/en/commercial/7.0/migration-guides/v7_0)

## Community News

### Highlights from .NET 7.0?

Our team has closely followed the ASP.NET Core and Entity Framework Core 7.0 releases, read Microsoft's guides and documentation and adapt the changes to our ABP.IO Platform. We are proud to say that we've shipped the ABP 7.0 RC.1 based on .NET 7.0 just after Microsoft's .NET 7.0 release.

In addition to the ABP's .NET 7.0 upgrade, our team has created 13 great articles to highlight the important features coming with ASP.NET Core 7.0 and Entity Framework Core 7.0.

You can read [this post](https://volosoft.com/Blog/Highlights-for-ASP.NET-Entity-Framework-Core-NET-7.0) to see the list of all articles.

### New ABP Community Posts

In addition to [the 13 articles to highlight .NET 7.0 features written by our team]((https://volosoft.com/Blog/Highlights-for-ASP.NET-Entity-Framework-Core-NET-7.0)), here are some of the recent posts added to the [ABP Community](https://community.abp.io/):

* [liangshiwei](https://github.com/realLiangshiwei) has created a new community article, that shows [How to Use the Weixin Authentication for MVC / Razor Page Applications](https://community.abp.io/posts/how-to-use-the-weixin-authentication-for-mvc-razor-page-applications-a33e0wti).
* [Jasen Fici](https://community.abp.io/posts/deploying-abp.io-to-an-azure-appservice-ma8kukdp) has created a new article: [Deploying abp.io to an Azure AppService](https://community.abp.io/posts/deploying-abp.io-to-an-azure-appservice-ma8kukdp).

Thanks to the ABP Community for all the content they have published. You can also [post your ABP related (text or video) content](https://community.abp.io/articles/submit) to the ABP Community.

## About the Next Version

The next feature version will be 7.1. You can follow the [release planning here](https://github.com/abpframework/abp/milestones).

Please [submit an issue](https://github.com/abpframework/abp/issues/new) if you have any problems with this version.
