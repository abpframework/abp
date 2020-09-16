# ABP Framework 3.2 RC with the new Blazor UI

We are extremely excited today to release the ABP Framework (and the ABP Commercial, as always) version `3.2.0-rc.1` (Release Candidate). This release includes an early preview version of the **Blazor UI** for the ABP.IO Platform.

## The Blazor UI

While the Blazor UI should be considered as **experimental** for now, it is possible to develop your application today.

### Fundamental Services

Currently implemented some important framework features;

* **Authentication** through the MVC backend using the OpenId Connect authorization code flow. So, all the current login options (login, register, forgot password, external/social logins...) are supported.
* **Authorization**, using the ABP Framework **permissions** as well as the standard authorization system.
* **Localization** just works like the MVC UI.
* **Basic Theme** with top main menu.
* **Dynamic C# HTTP API proxies**, so you can directly consume your backend API by injecting the application service interfaces.
* Some other **fundamental services** like ISettingProvider, IFeatureChecker, ICurrentUser

Also, the standard .net services are already available, like caching, logging, validation and much more. Since the ABP Framework is layered itself, all the non MVC UI related features are already available.

### Pre-Built Modules

And some modules have been implemented;

* **Identity** module is pre-installed and provides user**, role and permission management**.
* **Profile management** page is implemented to allow to change password and personal settings.

### About the Blazorise Library

We've selected the [Blazorise](https://blazorise.com/) as a fundamental UI library for the Blazor UI. It already support different HTML/CSS frameworks and significantly increases the developer productivity.

We also have a good news: **[Mladen Macanović](https://github.com/stsrki)**, the creator of the Blazorise, is **joining to the core ABP Framework team** in the next weeks. We are excited to work with him to bring the power of these two successfully projects together.

### Get started with the Blazor UI

If you want to try the Blazor UI today, follow the instructions below.

First, install the the latest [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) preview version:

````bash
dotnet tool update Volo.Abp.Cli -g --version 3.2.0-rc.1
````

Then you can create a new solution using the *abp new* command:

````bash
abp new AbpBlazorDemo -u blazor
````

> See the ABP CLI documentation for the additional options, like MongoDB database or separated authentication server.

Open the generated solution using the latest Visual Studio 2019. You will see a solution structure like the picture below:

TODO

* Run the `.DbMigrator` project to create the database and seed the initial data.
* Run the `HttpApi.Host` project for the server side.
* Run the `.Blazor` project to start the Blazor UI.

TODO

## What's New with the ABP Framework 3.2

d