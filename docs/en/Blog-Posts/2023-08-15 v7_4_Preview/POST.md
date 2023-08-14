# ABP.IO Platform 7.4 RC Has Been Released

Today, we are happy to release the [ABP Framework](https://abp.io/) and [ABP Commercial](https://commercial.abp.io/) version **7.4 RC** (Release Candidate). This blog post introduces the new features and important changes in this new version.

Try this version and provide feedback for a more stable version of ABP v7.4! Thanks to all of you.

## Get Started with the 7.4 RC

Follow the steps below to try version 7.4.0 RC today:

1) **Upgrade** the ABP CLI to version `7.4.0-rc.1` using a command line terminal:

````bash
dotnet tool update Volo.Abp.Cli -g --version 7.4.0-rc.1
````

**or install** it if you haven't before:

````bash
dotnet tool install Volo.Abp.Cli -g --version 7.4.0-rc.1
````

2) Create a **new application** with the `--preview` option:

````bash
abp new BookStore --preview
````

See the [ABP CLI documentation](https://docs.abp.io/en/abp/latest/CLI) for all the available options.

> You can also use the [Get Started](https://abp.io/get-started) page to generate a CLI command to create a new application.

You can use any IDE that supports .NET 7.x, like [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/).

## Migration Guides

There are a few breaking changes in this version that may affect your application.
Please see the following migration documents, if you are upgrading from v7.2:

* [ABP Framework 7.3 to 7.4 Migration Guide](https://docs.abp.io/en/abp/7.4/Migration-Guides/Abp-7_4)

## What's New with ABP Framework 7.4?

In this section, I will introduce some major features released in this version. Here is a brief list of the titles that will be explained in the next sections:

//TODO:

## What's New with ABP Commercial 7.4?

We've also worked on [ABP Commercial](https://commercial.abp.io/) to align the features and changes made in the ABP Framework. The following sections introduce a few new features coming with ABP Commercial 7.4.

//TODO:

## Community News

### Community Talks 2023.6: Live Community Question Session

//TODO: mention about the talk and add a cover image!!!

### Volosoft Will Attend to DevNot Developer Summit 2023

//TODO: mention about the event!!!

### New ABP Community Posts

There are exciting articles contributed by the ABP community as always. I will highlight some of them here:

* [ABP Commercial - GDPR Module Overview](https://community.abp.io/posts/abp-commercial-gdpr-module-overview-kvmsm3ku) by [Engincan Veske](https://twitter.com/EngincanVeske)
* [Video: ABP Framework Data Transfer Objects](https://community.abp.io/videos/abp-framework-data-transfer-objects-qwebfqz5) by [Hamza Albreem](https://github.com/braim23)
* [Video: ABP Framework Essentials: MongoDB](https://community.abp.io/videos/abp-framework-essentials-mongodb-gwlblh5x) by [Hamza Albreem](https://github.com/braim23)
* [ABP Modules and Entity Dependencies](https://community.abp.io/posts/abp-modules-and-entity-dependencies-hn7wr093) by [Jack Fistelmann](https://github.com/nebula2)
* [How to add dark mode support to the Basic Theme in 3 steps?](https://community.abp.io/posts/how-to-add-dark-mode-support-to-the-basic-theme-in-3-steps-ge9c0f85) by [Enis Necipoğlu](https://twitter.com/EnisNecipoglu)
* [Deploying docker image to Azure with yml and bicep through Github Actions](https://community.abp.io/posts/deploying-docker-image-to-azure-with-yml-and-bicep-through-github-actions-cjiuh55m) by [Sturla](https://community.abp.io/members/Sturla)

Thanks to the ABP Community for all the content they have published. You can also [post your ABP-related (text or video) content](https://community.abp.io/articles/submit) to the ABP Community.  

## Conclusion

This version comes with some new features and a lot of enhancements to the existing features. You can see the [Road Map](https://docs.abp.io/en/abp/7.4/Road-Map) documentation to learn about the release schedule and planned features for the next releases. Please try ABP v7.4 RC and provide feedback to help us release a more stable version.

Thanks for being a part of this community!