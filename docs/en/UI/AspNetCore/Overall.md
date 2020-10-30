# ASP.NET Core MVC / Razor Pages UI

## Introduction

ABP Framework provides a convenient and comfortable way of creating web applications using the ASP.NET Core MVC / Razor Pages as the User Interface framework.

ABP doesn't offer a new/custom way of UI development. You can continue to use your current skills to create the UI. However, it offers a lot of features to make your development easier and have a more maintainable code base.

### MVC vs Razor Pages

ASP.NET Core provides two models for UI development:

* **[MVC (Model-View-Controller)](https://docs.microsoft.com/en-us/aspnet/core/mvc/)** is the classic way that exists from the version 1.0. This model can be used to create UI pages/components and HTTP APIs.
* **[Razor Pages](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/)** was introduced with the ASP.NET Core 2.0 as a new way to create web pages.

**ABP Framework supports both** of the MVC and the Razor Pages models. However, it is suggested to create the **UI pages with Razor Pages** approach and use the **MVC model to build HTTP APIs**. So, all the pre-build modules, samples and the documentation is based on the Razor Pages for the UI development, while you can always apply the MVC pattern to create your own pages.

### Modularity

[Modularity](../../Module-Development-Basics.md) is one of the key goals of the ABP Framework. It is not different for the UI; It is possible to develop modular applications and reusable application modules with isolated and reusable UI pages and components.

The [application startup template](../../Startup-Templates/Application.md) comes with some application modules pre-installed. These modules have their own UI pages embedded into their own NuGet packages. You don't see their code in your solution, but they work as expected on runtime.

## Theme System

ABP Framework provides a complete [Theming](Theming.md) system with the following goals:

* Reusable [application modules](../../Modules/Index.md) are developed **theme-independent**, so they can work with any UI theme.
* UI theme is **decided by the final application**.
* The theme is distributed via NuGet/NPM packages, so it is **easily upgradable**.
* The final application can **customize** the selected theme.

### Current Themes

Currently, two themes are **officially provided**:

* The [Basic Theme](Basic-Theme.md) is the minimalist theme with the plain Bootstrap style. It is **open source and free**.
* The [Lepton Theme](https://commercial.abp.io/themes) is a **commercial** theme developed by the core ABP team and is a part of the [ABP Commercial](https://commercial.abp.io/) license.

There are also some community-driven themes for the ABP Framework (you can search on the web).

### Base Libraries

There are a set of standard JavaScript/CSS libraries that comes pre-installed and supported by all the themes:

- [Twitter Bootstrap](https://getbootstrap.com/) as the fundamental HTML/CSS framework.
- [JQuery](https://jquery.com/) for DOM manipulation.
- [DataTables.Net](https://datatables.net/) for data grids.
- [JQuery Validation](https://jqueryvalidation.org/) for client side & [unobtrusive](https://github.com/aspnet/jquery-validation-unobtrusive) validation
- [FontAwesome](https://fontawesome.com/) as the fundamental CSS font library.
- [SweetAlert](https://sweetalert.js.org/) to show fancy alert message and confirmation dialogs.
- [Toastr](https://github.com/CodeSeven/toastr) to show toast notifications.
- [Lodesh](https://lodash.com/) as a utility library.
- [Luxon](https://moment.github.io/luxon/) for date/time operations.
- [JQuery Form](https://github.com/jquery-form/form) for AJAX forms.
- [bootstrap-datepicker](https://github.com/uxsolutions/bootstrap-datepicker) to show date pickers.
- [Select2](https://select2.org/) for better select/combo boxes.
- [Timeago](http://timeago.yarp.com/) to show automatically updating fuzzy timestamps.
- [malihu-custom-scrollbar-plugin](https://github.com/malihu/malihu-custom-scrollbar-plugin) for custom scrollbars.

You can use these libraries directly in your applications, without needing to manually import your page.

### Layouts

The themes provide the standard layouts. So, you have responsive layouts with the standard features already implemented. The screenshot below has taken from the Application Layout of the [Basic Theme](Basic-Theme.md):

![basic-theme-application-layout](../../images/basic-theme-application-layout.png)

See the [Theming](Theming.md) document for more layout options and other details.

## Features

TODO