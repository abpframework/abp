# ABP.IO Platform 8.2 RC Has Been Released

Today, we are happy to release the [ABP Framework](https://abp.io/) and [ABP Commercial](https://commercial.abp.io/) version **8.2 RC** (Release Candidate). This blog post introduces the new features and important changes in this new version.

Try this version and provide feedback for a more stable version of ABP v8.2! Thanks to all of you.

## Get Started with the 8.2 RC

Follow the steps below to try version 8.2.0 RC today:

1) **Upgrade** the ABP CLI to version `8.2.0-rc.3` using a command line terminal:

````bash
dotnet tool update Volo.Abp.Cli -g --version 8.2.0-rc.3
````

**or install** it if you haven't before:

````bash
dotnet tool install Volo.Abp.Cli -g --version 8.2.0-rc.3
````

2) Create a **new application** with the `--preview` option:

````bash
abp new BookStore --preview
````

See the [ABP CLI documentation](https://docs.abp.io/en/abp/latest/CLI) for all the available options.

> You can also use the [Get Started](https://abp.io/get-started) page to generate a CLI command to create a new application.

You can use any IDE that supports .NET 8.x, like [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/).

## Migration Guides

There are a few breaking changes in this version that may affect your application.
Please see the following migration documents, if you are upgrading from v8.x or earlier:

* [ABP Framework 8.x to 8.2 Migration Guide](https://docs.abp.io/en/abp/8.2/Migration-Guides/Abp-8_2)
* [ABP Commercial 8.x to 8.2 Migration Guide](https://docs.abp.io/en/commercial/8.2/migration-guides/v8_2)

## What's New with ABP Framework 8.2?

In this section, I will introduce some major features released in this version.
Here is a brief list of titles explained in the next sections:

//TODO: list topics...

## What's New with ABP Commercial 8.2?

We've also worked on ABP Commercial to align the features and changes made in the ABP Framework. The following sections introduce a few new features coming with ABP Commercial 8.2.

//TODO: explain new features...

## Community News

### ABP Dotnet Conf 2024 Wrap Up

![](abp-dotnet-conf-2024.png)

We organized ABP Dotnet Conference 2024 on May 2024 and we are happy to share the success of the conference, which captivated overwhelmingly interested live viewers from all over the world. 29 great line up of speakers which includes .NET experts and Microsoft MVPs delivered captivating talks that resonated with the audiences. Each of the talks attracted a great amount of interest and a lot of questions, sparking curiosity in the attendees.

Thanks to all speakers and attendees for joining our event. 🙏

> We shared our takeaways in a blog post, which you can read at [https://blog.abp.io/abp/ABP-Dotnet-Conference-2024-Wrap-Up](https://blog.abp.io/abp/ABP-Dotnet-Conference-2024-Wrap-Up).

### DevDays Europe 2024

![](devdays-europe.jpg)

Co-founder of [Volosoft](https://volosoft.com/), Alper Ebiçoğlu gave a speech about "How to Build a Multi-Tenant ASP.NET Core Application" in the [DevDays Europe 2024](https://devdays.lt/) on the 20th of May.

### DevOps Pro Europe 2024

![](devops-pro-europe.jpg)

We are thrilled to announce that the co-founder of [Volosoft](https://volosoft.com/) and Lead Developer of the [ABP Framework](https://abp.io/), Halil Ibrahim Kalkan gave a speech about "Building a Kubernetes Integrated Local Development Environment" in the [DevOps Pro Europe](https://devopspro.lt/) on the 24th of May.

### Devnot Dotnet Conference 2024

We are happy to announce that core team members of the [ABP Framework](https://abp.io/), [Alper Ebiçoğlu](https://twitter.com/alperebicoglu) and [Enis Necipoğlu](https://twitter.com/EnisNecipoglu) will give speeches at the [Devnot Dotnet Conference 2024](https://dotnet.devnot.com/) on 25th of May.

[Alper Ebiçoğlu](https://twitter.com/alperebicoglu) will talk about **"AspNet Core & Multitenancy"**:

![](devnot-dotnet-conference-alper-ebicoglu.png)

On the other hand, [Enis Necipoğlu](https://twitter.com/EnisNecipoglu) will talk about **"Reactive Programming with .NET MAUI"**:

![](devnot-dotnet-conference-enis-necipoglu.png)

### New ABP Community Articles

There are exciting articles contributed by the ABP community as always. I will highlight some of them here:

* [Ahmed Tarek](https://twitter.com/AhmedTarekHasa1) has created **four** new community articles:
  *  [🤔 When Implementations Affect Abstractions ⁉️](https://community.abp.io/posts/-when-implementations-affect-abstractions--ekx1o5xn)
  *  [👍 Design Best Practices In .NET C# 👀](https://community.abp.io/posts/design-best-practices-in-.net-c--eg8q8xh0)
  *  [👍 Chain of Responsibility Design Pattern In .NET C# 👀](https://community.abp.io/posts/chain-of-responsibility-design-pattern-in-.net-c--djmvkug1)
  *  [Flagged Enumerations: How To Represent Features Combinations Into One Field](https://community.abp.io/posts/flagged-enumerations-how-to-represent-features-combinations-into-one-field-9gj4l670)
*  [Engincan Veske](https://github.com/EngincanV) has created **three** new community articles:
   *  [Performing Case-Insensitive Search in ABP Based-PostgreSQL Application: Using citext and Collation](https://community.abp.io/posts/caseinsensitive-search-in-abp-basedpostgresql-application-c9kb05dc)
   *  [Sentiment Analysis Within ABP-Based Application](https://community.abp.io/posts/sentiment-analysis-within-abpbased-application-lbsfkoxq)
   *  [Reusing and Optimizing Machine Learning Models in .NET](https://community.abp.io/posts/reusing-and-optimizing-machine-learning-models-in-.net-qj4ycnwu)
* [Unlocking Modularity in ABP.io A Closer Look at the Contributor Pattern](https://community.abp.io/posts/unlocking-modularity-in-abp.io-a-closer-look-at-the-contributor-pattern-ixf6wgbw) by [Qais Al khateeb](https://community.abp.io/members/qais.alkhateeb@devnas-jo.com)
* [Deploy Your ABP Framework MVC Project to Azure Container Apps](https://community.abp.io/posts/deploy-your-abp-framework-mvc-project-to-azure-container-apps-r93u9c6d) by [Selman Koç](https://community.abp.io/members/selmankoc)
* [How claim type works in ASP NET Core and ABP Framework](https://community.abp.io/posts/how-claim-type-works-in-asp-net-core-and-abp-framework-km5dw6g1) by [Liming Ma](https://github.com/maliming)
* [Using FluentValidation with ABP Framework](https://community.abp.io/posts/using-fluentvalidation-with-abp-framework-2cxuwl70) by [Enes Döner](https://community.abp.io/members/Enes)
* [Using Blob Storage with ABP](https://community.abp.io/posts/using-blob-storage-with-abp-framework-jygtmhn4) by [Emre Kendirli](https://community.abp.io/members/emrekenderli)

Thanks to the ABP Community for all the content they have published. You can also [post your ABP-related (text or video) content](https://community.abp.io/posts/submit) to the ABP Community.

## Conclusion

This version comes with some new features and a lot of enhancements to the existing features. You can see the [Road Map](https://docs.abp.io/en/abp/8.2/Road-Map) documentation to learn about the release schedule and planned features for the next releases. Please try ABP v8.2 RC and provide feedback to help us release a more stable version.

Thanks for being a part of this community!