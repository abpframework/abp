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
Please see the following migration documents, if you are upgrading from v7.3:

* [ABP Framework 7.3 to 7.4 Migration Guide](https://docs.abp.io/en/abp/7.4/Migration-Guides/Abp-7_4)

## What's New with ABP Framework 7.4?

In this section, I will introduce some major features released in this version. Here is a brief list of the titles that will be explained in the next sections:

* Dynamic Setting Store
* Introducing the `AdditionalAssemblyAttribute`
* `CorrelationId` Support on Distributed Events
* Dynamic Database Migrations
* Other News

### Dynamic Setting Store

Prior to this version, it was hard to define settings in different microservices and centrally manage all setting definitions in a single admin application. To make that possible, we were adding project references for all the microservices' service contract packages from a single microservice, so it can know all the setting definitions and manage them. 

In this version, ABP Framework introduces the Dynamic Setting Store, which is an important feature that allows you to collect and get all setting definitions from a single point and overcome the setting management problems on microservices.

> *Note*: If you are upgrading from an earlier version and using the Setting Management module, you need to create a new migration and apply it to your database because a new database table has been added for this feature.

### Introducing the `AdditionalAssemblyAttribute`

In this version, we have introduced the `AdditionalAssemblyAttribute` to define additional assemblies to be part of a module. ABP Framework automatically registers all the services of your module to the [Dependency Injection System](https://docs.abp.io/en/abp/latest/Dependency-Injection). It finds the service types by scanning types in the assembly that defines your module class. Typically, every assembly contains a separate module class definition and modules are depend on each other using the `DependsOn` attribute.

In some rare cases, your module may consist of multiple assemblies, and only one of them define a module class, and you want to make the other assemblies parts of your module. This is especially useful, if you dont't want to depend on that module's dependencies.

In that case, you can use the `AdditionalAssembly` attribute as shown below:

```csharp
[DependsOn(...)] // Your module dependencies as you normally would do
[AdditionalAssembly(typeof(IdentityServiceModule))] // A type in the target assembly (in another assembly)
public class IdentityServiceTestModule : AbpModule
{
    ...
}
```

With the `AdditonalAssembly` attribute definition, ABP loads the assembly containing the `IdentityServiceModule` class as a part of the blog module. Notice that, in this case, none of the module dependencies of the `IdentityServiceModule` are loaded. Because, we are not depending on the `IdentityServiceModule`, instead we are just adding its assembly as a part of the `IdentityServiceTestModule`.

> You can check the [Module Development Basics](https://docs.abp.io/en/abp/7.4/Module-Development-Basics) documentation to learn more.

### `CorrelationId` Support on Distributed Events

In this version, `CorrelationId` (a unique key that is used in distributed applications to trace requests accross multiple services/operations) is being attached to the distributed events, so you can relate events with HTTP requests and can trace all the related activities.

ABP Framework generates a `correlationId` for the first time when an operation is started and then attachs the current `correlationId` to distributed events as an additional property. For example, if you are using the [transactional outbox or inbox pattern provided by ABP Framework](https://docs.abp.io/en/abp/latest/Distributed-Event-Bus#outbox-inbox-for-transactional-events), you can see the `correlationId` in the extra properties of the `IncomingEventInfo` or `OutgoingEventInfo` classes with the standard `X-Correlation-Id` key.

> You can check [this issue](https://github.com/abpframework/abp/issues/16773) for more information.

### Dynamic Database Migrations 

//TODO: ...

### Other News

* [OpenIddict](https://github.com/openiddict/openiddict-core/tree/4.7.0) library has been upgraded to **v4.7.0**. See [#17334](https://github.com/abpframework/abp/pull/17334) for more info.
* ABP v7.4 introduces the `Volo.Abp.Maui.Client` package, which is used by the MAUI mobile application in ABP Commercial. See [#17201](https://github.com/abpframework/abp/pull/17201) for more info.
* In this version, the `AbpAspNetCoreIntegratedTestBase` class gets a generic type parameter, which expect either a startup class or an ABP module class. This allows us to use configurations from an ABP module or old-style ASP.NET Core Startup class in a test application class and this simplifies the test application project. See [#17039](https://github.com/abpframework/abp/pull/17039) for more info.

## What's New with ABP Commercial 7.4?

We've also worked on [ABP Commercial](https://commercial.abp.io/) to align the features and changes made in the ABP Framework. The following sections introduce new features coming with ABP Commercial 7.4.

//TODO:

## Community News

### Volosoft Will Attend to DevNot Developer Summit 2023

![](developersummit.jpg)

We are thrilled to announce that the co-founder of [Volosoft](https://volosoft.com/) and Lead Developer of the ABP Framework, Halil Ibrahim Kalkan will give a speech about "Building a Kubernetes Integrated Local Development Environment" in the [DevNot's Developer Summit 2023 event](https://summit.devnot.com/) on the 7th of October.

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