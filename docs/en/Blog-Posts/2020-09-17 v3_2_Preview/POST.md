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

### Get started with the Blazor UI

TODO

## What's New with the ABP Framework 3.2

d