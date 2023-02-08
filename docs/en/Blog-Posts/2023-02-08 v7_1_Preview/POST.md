# ABP.IO Platform 7.1 RC Has Been Released

Today, we are happy to release the [ABP Framework](https://abp.io/) and [ABP Commercial](https://commercial.abp.io/) version **7.1 RC** (Release Candidate). This blog post introduces the new features and important changes in this new version.

Try this version and provide feedback for a more stable version of ABP v7.1! Thanks to all of you.

## Get Started with the 7.1 RC

Follow the steps below to try version 7.1.0 RC today:

1) **Upgrade** the ABP CLI to version `7.1.0-rc.1` using a command line terminal:

````bash
dotnet tool update Volo.Abp.Cli -g --version 7.1.0-rc.1
````

**or install** it if you haven't before:

````bash
dotnet tool install Volo.Abp.Cli -g --version 7.1.0-rc.1
````

2) Create a **new application** with the `--preview` option:

````bash
abp new BookStore --preview
````

See the [ABP CLI documentation](https://docs.abp.io/en/abp/latest/CLI) for all the available options.

> You can also use the [Get Started](https://abp.io/get-started) page to generate a CLI command for creating a new application.

You can use any IDE that supports .NET 7.x, like [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/).

## Migration Guides

ABP v7.1 doesn't introduce any breaking change.

## What's New with ABP Framework 7.1?

In this section, I will introduce some major features released in this version. In addition to these features, so many enhancements have been made in this version too. 

Here is a brief list of titles explained in the next sections:

* Blazor WASM option added to Application Single Layer Startup Template
* Introducing the `EventSynchronizer`
* Introducing the `DeleteDirectAsync` method for the `IRepository` interface
* Reducing cache duration for development environment
* Improvements on eShopOnAbp project
* Others

### Blazor WASM option added to Application Single Layer Startup Template

//TODO:!!!

### Introducing the `EventSynchronizer`

//TODO:!!!

### Introducing the `DeleteDirectAsync` method for the `IRepository` interface

//TODO:!!!

### Reducing cache duration for development environment

//TODO:!!!

### Improvements on eShopOnAbp project

K8s and Docker configurations have been made within this version (Dockerfiles and helm-charts have been added and image build scripts have been updated). See [#14083](https://github.com/abpframework/abp/issues/14083) for more information.

### Others

There are some new features on [CMS Kit Module](https://docs.abp.io/en/abp/latest/Modules/Cms-Kit/Index):

* Referral Links have been added to CMS Kit Comment Feature (optional). You can specify common referral links (such as "nofollow" and "noreferrer") for links in the comments. See [#15458](https://github.com/abpframework/abp/issues/15458) for more information.
* ReCaptcha verification has been added to CMS Kit Comment Feature (optional). You can enable ReCaptcha support to enable protaction against bots. See the [documentation](https://docs.abp.io/en/abp/7.1/Modules/Cms-Kit/Comments) for more information.

## What's New with ABP Commercial 7.1?

We've also worked on [ABP Commercial](https://commercial.abp.io/) to align the features and changes made in the ABP Framework. The following sections introduce a few new feature coming with ABP Commercial 7.1.

### Blazor WASM option added to Application Single Layer Pro Startup Template

//TODO:!!!

### Suite - MAUI Blazor Code Generation

//TODO:!!!

### Allowing entering a username while impersonating the tenant

//TODO:!!!

## Community News

### New ABP Community Posts

* [Sergei Gorlovetsky](https://community.abp.io/members/Sergei.Gorlovetsky) has created two new community articles:
  * [Why ABP Framework is one of the best tools for migration from legacy MS Access systems to latest Web app](https://community.abp.io/posts/why-abp-framework-is-one-of-the-best-tools-for-migration-from-legacy-ms-access-systems-to-latest-web-app-7l39eof0)
  * [ABP Framework — 5 steps Go No Go Decision Tree](https://community.abp.io/posts/abp-framework-5-steps-go-no-go-decision-tree-2sy6r2st)
* [Onur Pıçakcı](https://github.com/onurpicakci) has created his first ABP community article that explains how to contribute to ABP Framework. You can read it 👉 [here](https://community.abp.io/posts/how-to-contribute-to-abp-framework-46dvzzvj).
* [Maliming](https://github.com/maliming) has created a new community article to show how to convert create/edit modals to a page. You can read it 👉 [here](https://community.abp.io/posts/converting-createedit-modal-to-page-4ps5v60m).

We thank you all. We thank all the authors for contributing to the [ABP Community platform](https://community.abp.io/).

### Volosoft Attended to NDC London 2023

![](ndc-london.png)

Core team members of ABP Framework, [Halil Ibrahim Kalkan](https://twitter.com/hibrahimkalkan) and [Alper Ebicoglu](https://twitter.com/alperebicoglu) have attended the [NDC London 2023](https://ndclondon.com/) on the 23rd to 27th of January.  

### Community Talks 2023.1: LeptonX Customization

![](community-talks-conver-image.png)

In this episode of ABP Community Talks, 2023.1; we'll talk about **LeptonX Customization**. We will dive into the details and show you how to customize [LeptonX Theme](https://leptontheme.com/) with examples. The event will be live on Thursday, February 16, 2023 (20:00 - 21:00 UTC).

> Register to listen and ask your questions now 👉 https://kommunity.com/volosoft/events/abp-community-talks-20231-leptonx-customization-03f9fd8c.

## Conclusion

This version comes with some new features and a lot of enhancements to the existing features. You can see the [Road Map](https://docs.abp.io/en/abp/7.1/Road-Map) documentation to learn about the release schedule and planned features for the next releases. Please try the ABP v7.1 RC and provide feedback to help us release a more stable version.

Thanks for being a part of this community!