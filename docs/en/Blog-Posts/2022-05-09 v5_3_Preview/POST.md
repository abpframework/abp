# ABP.IO Platform 5.3 RC.1 Has Been Released

Today, we are happy to release the [ABP Framework](https://abp.io/) and  [ABP Commercial](https://commercial.abp.io/) version **5.3 RC** (Release Candidate). This blog post introduces the new features and important changes in this new version.

> **The planned release date for the [5.3.0 Stable](https://github.com/abpframework/abp/milestone/69) version is May 31, 2022**.

Please try this version and provide feedback for a more stable ABP version 5.3! Thank you all.

## Get Started with the 5.3 RC

Follow the steps below to try the version 5.3.0 RC today:

1) **Upgrade** the ABP CLI to version `5.3.0-rc.1` using a command line terminal:

````bash
dotnet tool update Volo.Abp.Cli -g --version 5.3.0-rc.1
````

​	**or install** it if you haven't before:

````bash
dotnet tool install Volo.Abp.Cli -g --version 5.3.0-rc.1
````

2) Create a **new application** with the `--preview` option:

````bash
abp new BookStore --preview
````

See the [ABP CLI documentation](https://docs.abp.io/en/abp/latest/CLI) for all the available options.

> You can also use the *Direct Download* tab on the [Get Started](https://abp.io/get-started) page by selecting the **Preview checkbox**.

You can use any IDE that supports .NET 6.x, like **[Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)**.

---

<!-- TODO: we need to prepare a migration guide to explain AutoMapper v11 upgrade and mention it in the below section.? -->
## Migration Notes & Breaking Changes

---

## What's New with ABP Framework 5.2?

In this section, I will introduce some major features released with this version. Here, a brief list of titles explained in the next sections:

* Single-layer option added to the [*Get Started*](https://abp.io/get-started) page
* PWA Support for Blazor WASM & Angular UIs
* Introduce the `Volo.Abp.Gdpr.Abstractions` package
* Batch Publish Events from Outbox to the Event Bus
* Improvements on **eShopOnAbp** project
* LeptonX Lite Documentations & Status of the Project
* Progress of the OpenIddict module
* New Deployment Documentations
* Other news...

### Single-layer Option on Get Started Page

We've created a new startup template named `app-nolayers` and [announced](https://blog.abp.io/abp/ABP.IO-Platform-5-2-RC-Has-Been-Published) it in the previous version. In this version, we've also add this startup template option to the *Get Started* page, so anyone can also create a single layer application template via *Get Started* page.

![Get Started Page / app-nolayers](./app-nolayers-get-started.png)

### PWA Support for Blazor WASM & Angular UIs

---

## Community News

### New ABP Community Posts

* [Anto Subash](https://twitter.com/antosubash) created a series named ["Microservice with ABP"](https://blog.antosubash.com/posts/abp-microservice-series) and shared couple of video posts about the ABP Microservice solution.
* [Francisco Kadzi](https://github.com/CiscoNinja) has created his first ABP Community article that shows how to ["Customize ABP Angular Application UI with AdminLTE"](https://community.abp.io/posts/customize-abp-angular-application-ui-with-adminlte.-7qu1m67s).
* [Halil Ibrahim Kalkan](https://twitter.com/hibrahimkalkan) has created an article to show how to ["Dealing with Multiple Implementations of a Service in ASP.NET Core & ABP Dependency Injection"](https://community.abp.io/posts/dealing-with-multiple-implementations-of-a-service-in-asp.net-core-abp-dependency-injection-ysfp4ho2) with examples.
* Manoj Kumar submitted a new article about how to use "ABP authentication in a Flutter application". It was a frequently asked topic, which you can read [here](https://community.abp.io/posts/flutter-web-authentication-from-abp-mp6l2ehx).
* [Engincan Veske](https://twitter.com/EngincanVeske) created a new Community Article to show "Concurrency Check/Control in ABP". You can read from [here](https://community.abp.io/posts/handle-concurrency-with-ef-core-in-an-abp-framework-project-with-asp.net-core-mvc-jlkc3w8f).

### ABP Community Talks 2022.4 - "How can you contribute to the open source ABP Framework?" (May 10, 2022 - 17:00 UTC)

![](./community-talks-2022.4.png)

We've [asked you to pick the topic of the next Community Talks](https://twitter.com/abpframework/status/1514567683072745474?s=20&t=rJfHrB3DYDNsk2EXS8zBBQ) and you've chosen the "How to contribute to open source ABP Framework?" for the next talk topic. So, in this Community Talk, we will be talking about "How to contribute to ABP Framework" with one of the top contributors [Ismail Yılmaz](https://github.com/iyilm4z). The event will be on **May 10, 2022, at 17:00 (UTC)** on YouTube.

> You can register for the event from [here](https://kommunity.com/volosoft/events/abp-community-talks-20224-how-to-contribute-to-the-open-source-abp-framework-d9b50664), if you haven't registered yet.

You can also [subscribe to the Volosoft channel](https://www.youtube.com/channel/UCO3XKlpvq8CA5MQNVS6b3dQ) to inform about further ABP events and videos.

### Discord Server

We've created an official ABP Discord server so the ABP Community can interact with each other. 

Thanks to the ABP Community, **700+** people joined our Discord Server so far and it grows every day.

> You can read the [ABP Discord Server announcement post](https://blog.abp.io/abp/Official-ABP-Discord-Server-is-Here) to learn more about ABP Discord Server.

You can join our Discord Server from [here](https://discord.gg/abp), if you haven't yet.