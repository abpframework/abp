# ABP.IO Platform 5.1 RC.1 Has Been Released

Today, we are releasing the [ABP Framework](https://abp.io/) and the [ABP Commercial](https://commercial.abp.io/) version **5.1 RC** (Release Candidate). This blog post introduces the new features and important changes in this new version.

> **The planned release date for the [5.1 stable](https://github.com/abpframework/abp/milestone/64) version is February 8, 2022**.

Please try this version and provide feedback for a more stable ABP version 5.1! Thank you all.

## Get Started with the 5.1 RC

follow the steps below to try the version 5.1 RC today;

1) **Upgrade** the ABP CLI to the version `5.1.0-rc.1` using a command line terminal:

````bash
dotnet tool update Volo.Abp.Cli -g --version 5.1.0-rc.1
````

**or install** if you haven't installed before:

````bash
dotnet tool install Volo.Abp.Cli -g --version 5.1.0-rc.1
````

2) Create a **new application** with the `--preview` option:

````bash
abp new BookStore --preview
````

See the [ABP CLI documentation](https://docs.abp.io/en/abp/latest/CLI) for all the available options.

> You can also use the *Direct Download* tab on the [Get Started](https://abp.io/get-started) page by selecting the **Preview checkbox**.

You can use any IDE that supports .NET 6.0 development (e.g. [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)).

### Migration Notes & Breaking Changes

TODO: Startup template changes

## What's new with 5.1?

In this section, I will introduce some major features released with this version.

### Header 1

TODO

## Community News

### ABP Community Talks 2021.1

![abp-community-talks-2022-1](abp-community-talks-2022-1.png)

This is the second episode of the ABP Community Talks and we are talking about microservice development with the ABP Framework, based on the [eShopOnAbp](https://github.com/abpframework/eShopOnAbp) reference solution. This live meeting will be at **January 20, 2022, 17:00 (UTC)** on YouTube.

**Join this event on the Kommunity platform: https://kommunity.com/volosoft/events/abp-community-talks-20221-microservice-development-acd0f44b**

You can also [subscribe to the Volosoft channel](https://www.youtube.com/channel/UCO3XKlpvq8CA5MQNVS6b3dQ) for reminders for further ABP events and videos.

### New ABP Community posts

Here, some of the recent posts added to the [ABP community](https://community.abp.io/):

* [Minimal API development with the ABP Framework](https://community.abp.io/articles/minimal-api-with-abp-hello-world-part-1-sg5i44p8) by [@antosubash](https://github.com/antosubash) (three parts, video tutorial).
* [Integrating the Syncfusion MVC Components to the ABP MVC UI](https://community.abp.io/articles/integrating-the-syncfusion-mvc-components-to-the-abp-mvc-ui-0gpkr1if) by [@EngincanV](https://github.com/EngincanV).
* [Add Tailwind CSS to your ABP Blazor UI](https://community.abp.io/articles/add-tailwindcss-to-your-abp-blazor-ui-vidiwzcy) by [@antosubash](https://github.com/antosubash) (video tutorial).
* [Import external users into the users Table from an ABP Framework application](https://community.abp.io/articles/import-external-users-into-the-users-table-from-an-abp-framework-application-7lnyw415) by [@bartvanhoey](https://github.com/bartvanhoey).

Thanks to the ABP Community for all the contents they have published. You can also [post your ABP and .NET related (text or video) contents](https://community.abp.io/articles/submit) to the ABP Community.

## Conclusion

In this blog post, I summarized the news about that new version and the ABP Community. Please try it and provide feedback by opening issues on [the GitHub repository](https://github.com/abpframework/abp). Thank you all!
