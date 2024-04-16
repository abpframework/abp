# ABP.IO Platform 8.1 Final Has Been Released!

[ABP Framework](https://abp.io/) and [ABP Commercial](https://commercial.abp.io/) 8.1 versions have been released today.

## What's New With Version 8.1?

All the new features were explained in detail in the [8.1 RC Announcement Post](https://blog.abp.io/abp/announcing-abp-8-1-release-candidate), so there is no need to review them again. You can check it out for more details. 

## Getting Started with 8.1

### Creating New Solutions

You can create a new solution with the ABP Framework version 8.1 by either using the `abp new` command or generating the CLI command on the [get started page](https://abp.io/get-started).

> See the [getting started document](https://docs.abp.io/en/abp/latest/Getting-Started) for more.

### How to Upgrade an Existing Solution

#### Install/Update the ABP CLI

First, install the ABP CLI or upgrade it to the latest version.

If you haven't installed it yet:

```bash
dotnet tool install -g Volo.Abp.Cli
```

To update the existing CLI:

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

There are a few breaking changes in this version that may affect your application.
Please see the following migration documents, if you are upgrading from v8.x or earlier:

* [ABP Framework 8.0 to 8.1 Migration Guide](https://docs.abp.io/en/abp/8.1/Migration-Guides/Abp-8_1)
* [ABP Commercial 8.0 to 8.1 Migration Guide](https://docs.abp.io/en/commercial/8.1/migration-guides/v8_1)

## Community News

### New ABP Community Posts

As always, exciting articles have been contributed by the ABP community. I will highlight some of them here:

* [Adding a Module to an ABP project made simple](https://community.abp.io/posts/adding-a-module-to-an-abp-project-made-simple-a8zw0j2m) by [Bart Van Hoey](https://twitter.com/@bartvanhoey)
* [Getting started with Abp Vue UI](https://community.abp.io/posts/getting-started-with-abp-vue-ui-4vfiv5io) by [Sajankumar Vijayan](https://community.abp.io/members/Sajan)
* [Liming Ma](https://github.com/maliming) has created **two** new community articles:
  * [How to share the cookies between subdomains](https://community.abp.io/posts/how-to-share-the-cookies-between-subdomains-jfrzggc2)
  * [Using Testcontainers in ABP Unit Test](https://community.abp.io/posts/using-testcontainers-in-abp-unit-test-b67gzpxg)
* [Ahmed Tarek](https://github.com/AhmedTarekHasan) has created **three** new community articles:
  * [Strategy Design Pattern In .NET C#](https://community.abp.io/posts/strategy-design-pattern-in-.net-c-vcgv11h5)
  * [Mediator Design Pattern In .NET C#](https://community.abp.io/posts/mediator-design-pattern-in-.net-c-pdsjp93n)
  * [SOLID: Liskov Substitution Principle Explained In .NET C#](https://community.abp.io/posts/solid-liskov-substitution-principle-explained-in-.net-c-hx2z8vo9)

Thanks to the ABP Community for all the content they have published. You can also [post your ABP-related (text or video) content](https://community.abp.io/articles/submit) to the ABP Community.

### New ABP Blog Posts

There are also some exciting blog posts written by the ABP team. You can see the following list for some of those articles:

* [Announcement: Our New ABP.IO Unified Platform](https://blog.abp.io/abp/our-new-abp.io-unified-platform) by [Alper Ebicoglu](https://twitter.com/alperebicoglu)
* [Join ABP.IO at Modern .NET Web Day](https://blog.abp.io/abp/Join-ABP.IO-at-Modern-.NET-Web-Day) by [Roo Xu](https://github.com/Roo1227)

## About the Next Version

The next feature version will be 8.2. You can follow the [release planning here](https://github.com/abpframework/abp/milestones). Please [submit an issue](https://github.com/abpframework/abp/issues/new) if you have any problems with this version.
