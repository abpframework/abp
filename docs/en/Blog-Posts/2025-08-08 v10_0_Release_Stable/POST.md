# ABP.IO Platform 10.0 Final Has Been Released!

We are glad to announce that [ABP](https://abp.io/) 10.0 stable version has been released today. 

## What's New With Version 10.0?

All the new features were explained in detail in the [10.0 RC Announcement Post](https://abp.io/community/announcements/announcing-abp-10-0-release-candidate-86lrnyox), so there is no need to review them again. You can check it out for more details. 

## Getting Started with 10.0

### How to Upgrade an Existing Solution

You can upgrade your existing solutions with either ABP Studio or ABP CLI. In the following sections, both approaches are explained:

### Upgrading via ABP Studio

If you are already using the ABP Studio, you can upgrade it to the latest version. ABP Studio periodically checks for updates in the background, and when a new version of ABP Studio is available, you will be notified through a modal. Then, you can update it by confirming the opened modal. See [the documentation](https://abp.io/docs/latest/studio/installation#upgrading) for more info.

After upgrading the ABP Studio, then you can open your solution in the application, and simply click the **Upgrade ABP Packages** action button to instantly upgrade your solution:

![](upgrade-abp-packages.png)

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

You can run this command in the root folder of your solution to update all ABP related packages.

## Migration Guides

There are a few breaking changes in this version that may affect your application. Please read the migration guide carefully, if you are upgrading from v9.x: [ABP Version 10.0 Migration Guide](https://abp.io/docs/10.0/release-info/migration-guides/abp-10-0)

## Community News

### New ABP Community Articles

As always, exciting articles have been contributed by the ABP community. I will highlight some of them here:

* [Alper Ebiçoğlu](https://abp.io/community/members/alper)
  * [Optimize your .NET app for production Part 1](https://abp.io/community/articles/optimize-your-dotnet-app-for-production-for-any-.net-app-wa24j28e)
  * [Optimize your .NET app for production Part 2](https://abp.io/community/articles/optimize-your-dotnet-app-for-production-for-any-.net-app-2-78xgncpi)
  * [Return Code vs Exceptions: Which One is Better?](https://abp.io/community/articles/return-code-vs-exceptions-which-one-is-better-1rwcu9yi)
* [Sumeyye Kurtulus](https://abp.io/community/members/sumeyye.kurtulus)
  * [Building Scalable Angular Apps with Reusable UI Components](https://abp.io/community/articles/building-scalable-angular-apps-with-reusable-ui-components-b9npiff3)
  * [Angular Library Linking Made Easy: Paths, Workspaces and Symlinks](https://abp.io/community/articles/angular-library-linking-made-easy-paths-workspaces-and-5z2ate6e)
* [erdem çaygör](https://abp.io/community/members/erdem.caygor)
  * [Building Dynamic Forms in Angular for Enterprise](https://abp.io/community/articles/building-dynamic-forms-in-angular-for-enterprise-6r3ewpxt)
  * [From Server to Browser: Angular TransferState Explained](https://abp.io/community/articles/from-server-to-browser-angular-transferstate-explained-m99zf8oh)
* [Mansur Besleney](https://abp.io/community/members/mansur.besleney)
  * [Top 10 Exception Handling Mistakes in .NET](https://abp.io/community/articles/top-10-exception-handling-mistakes-in-net-jhm8wzvg)
* [Berkan Şaşmaz](https://abp.io/community/members/berkansasmaz)
  * [How to Dynamically Set the Connection String in EF Core](https://abp.io/community/articles/how-to-dynamically-set-the-connection-string-in-ef-core-30k87fpj)
* [Oğuzhan Ağır](https://abp.io/community/members/oguzhan.agir)
    * [The ASP.NET Core Dependency Injection System](https://abp.io/community/articles/the-asp.net-core-dependency-injection-system-3vbsdhq8)
* [Selman Koç](https://abp.io/community/members/selmankoc)
  * [5 Things Keep in Mind When Deploying Clustered Environment](https://abp.io/community/articles/5-things-keep-in-mind-when-deploying-clustered-environment-i9byusnv)
* [Muhammet Ali ÖZKAYA](https://abp.io/community/members/m.aliozkaya)
  * [Repository Pattern in ASP.NET Core](https://abp.io/community/articles/repository-pattern-in-asp.net-core-2dudlg3j)
* [Armağan Ünlü](https://abp.io/community/members/armagan)
  * [UI/UX Trends That Will Shape 2026](https://abp.io/community/articles/UI-UX-Trends-That-Will-Shape-2026-bx4c2kow)
* [Salih](https://abp.io/community/members/salih)
  * [What is That Domain Service in DDD for .NET Developers?](https://abp.io/community/articles/what-is-that-domain-service-in-ddd-for-.net-developers-uqnpwjja)
  * [Building an API Key Management System with ABP Framework](https://abp.io/community/articles/building-an-api-key-management-system-with-abp-framework-28gn4efw)
* [Fahri Gedik](https://abp.io/community/members/fahrigedik)
  * [Signal-Based Forms in Angular](https://abp.io/community/articles/signal-based-forms-in-angular-21-9qentsqs)

Thanks to the ABP Community for all the content they have published. You can also [post your ABP related (text or video) content](https://abp.io/community/posts/create) to the ABP Community.

## About the Next Version

The next feature version will be 10.1. You can follow the [release planning here](https://github.com/abpframework/abp/milestones). Please [submit an issue](https://github.com/abpframework/abp/issues/new) if you have any problems with this version.
