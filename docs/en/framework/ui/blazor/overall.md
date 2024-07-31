# Blazor UI: Overall

## Introduction

[Blazor](https://docs.microsoft.com/en-us/aspnet/core/blazor/) is a framework for building interactive client-side web UI with .NET. It is promising for a .NET developer that you can create Single-Page Web Applications using C# and the Razor syntax.

ABP provides infrastructure and integrations that make your Blazor development even easier, comfortable and enjoyable.

This document provides an overview for the ABP Blazor UI integration and highlights some major features.

### Getting Started

You can follow the documents below to start with the ABP and the Blazor UI now:

* [Get started](../../../get-started/index.md) with the Blazor UI for the ABP.
* [Web Application Development Tutorial](../../../tutorials/book-store/part-01.md) with the Blazor UI.

## Modularity

[Modularity](../../architecture/modularity/basics.md) is one of the key goals of the ABP. It is not different for the UI; It is possible to develop modular applications and reusable application modules with isolated and reusable UI pages and components.

The [application startup template](../../../solution-templates/layered-web-application) comes with some application modules pre-installed. These modules have their own UI pages embedded into their own NuGet packages. You don't see their code in your solution, but they work as expected on runtime.

## Dynamic C# Client Proxies

Dynamic C# Client Proxy system makes extremely easy to consume server side HTTP APIs from the UI. You just **inject** the [application service](../../architecture/domain-driven-design/application-services.md) **interface** and consume the remote APIs just like using local service method calls.

**Example: Get list of books from server and list on the UI**

````csharp
@page "/books"
@using Acme.BookStore.Books
@using Volo.Abp.Application.Dtos
@inject IBookAppService BookAppService

<ul>
    @foreach (var book in Books)
    {
        <li>
            @book.Name (by @book.AuthorName)
        </li>
    }
</ul>

@code {
    private IReadOnlyList<BookDto> Books { get; set; } = new List<BookDto>();

    protected override async Task OnInitializedAsync()
    {
        var result = await BookAppService.GetListAsync(
            new PagedAndSortedResultRequestDto()
        );
        
        Books = result.Items;
    }
}
````

* This razor component (page) uses `@inject IBookAppService BookAppService` to get a reference to the service proxy.
* It uses `BookAppService.GetListAsync` in the `OnInitializedAsync` and gets the list of the books, just like a regular C# method call.
* Finally, the page renders the books in a list on the UI.

ABP handles all the low level details for you, including a proper HTTP call, JSON serialization, exception handling and authentication.

See the [Dynamic C# Client Proxies](../../api-development/dynamic-csharp-clients.md) document for more.

## Theming System

ABP provides a complete [Theming](theming.md) system with the following goals:

* Reusable [application modules](../../../modules) are developed **theme-independent**, so they can work with any UI theme.
* UI theme is **decided by the final application**.
* The theme is distributed as NuGet package, so it is **easily upgradable**.
* The final application can **customize** the selected theme.

### Current Themes

Currently, three themes are **officially provided**:

* The [Basic Theme](basic-theme.md) is the minimalist theme with the plain Bootstrap style. It is **open source and free**.
* The [Lepton Theme](https://abp.io/themes) is a **commercial** theme developed by the core ABP team and is a part of the [ABP](https://abp.io/) license.
* The [LeptonX Theme](https://x.leptontheme.com/) is a theme that has both [commercial](https://docs.abp.io/en/commercial/latest/themes/lepton-x/blazor) and [lite](../../../ui-themes/lepton-x-lite/blazor.md) choices.

### Base Libraries

There are a set of standard libraries that comes pre-installed and supported by all the themes:

* [Twitter Bootstrap](https://getbootstrap.com/) as the fundamental HTML/CSS framework.
* [Blazorise](https://github.com/stsrki/Blazorise) as a component library that supports the Bootstrap and adds extra components like Data Grid and Tree.
* [FontAwesome](https://fontawesome.com/) as the fundamental CSS font library.
* [Flag Icon](https://github.com/lipis/flag-icons) as a library to show flags of countries.

These libraries are selected as the base libraries and available to the applications and modules.

> Bootstrap's JavaScript part is not used since the Blazorise library already provides the necessary functionalities to the Bootstrap components in a native way.

> Beginning from June, 2021, the Blazorise library has dual licenses; open source & commercial. Based on your yearly revenue, you may need to buy a commercial license. See [this post](https://blazorise.com/news/announcing-2022-blazorise-plans-and-pricing-updates) to learn more. The Blazorise license is bundled with ABP and commercial customers doesn’t need to buy an extra Blazorise license.

### The Layout

The themes provide the layout. So, you have a responsive layout with the standard features already implemented. The screenshot below has taken from the layout of the [Basic Theme](basic-theme.md):

![basic-theme-application-layout](../../../images/basic-theme-application-layout.png)

See the [Theming](theming.md) document for more layout options and other details.

### Layout Parts

A typical layout consists of multiple parts. The [Theming](theming.md) system provides [menus](navigation-menu.md), [toolbars](toolbars.md), [page alerts](page-alerts.md) and more to dynamically control the layout by your application and the modules you are using.

## Global Styles & Scripts / Bundling & Minification

ABP provides a standard way to manage the global script and style dependencies of an application. This is an essential feature for modularity since some modules may have such dependencies and they can declare dependencies in that way.

See the [Managing Global Scripts & Styles](global-scripts-styles.md) document.

## Services

ABP provides useful services that you can consume in your applications. Some of them are;

* [IUiMessageService](message.md) is used to show modal messages to the user.
* [IUiNotificationService](notification.md) is used to show toast-style notifications.
* [IAlertManager](page-alerts.md) is used to show in-page alerts.
* [ISettingProvider](settings.md) is used to access to the current setting values.
* `ICurrentUser` and `ICurrentTenant` is used to get information about the current user and the tenant.

## Dependency Injection

Razor components doesn't support [constructor injection](../../fundamentals/dependency-injection.md) by default. ABP makes possible to inject dependencies into the constructor of the code-behind file of a component.

**Example: Constructor-inject a service in the code-behind file of a component**

````csharp
using Microsoft.AspNetCore.Components;

namespace MyProject.Blazor.Pages
{
    public partial class Index
    {
        private readonly NavigationManager _navigationManager;

        public Index(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }
    }
}
````

ABP makes this possible by auto registering components to and resolving the component from the [Dependency Injection](../../fundamentals/dependency-injection.md) system.

> You can still continue to use property injection and the standard `[Inject]` approach if you prefer.

Resolving a component from the Dependency Injection system makes it possible to easily replace components of a depended module.

## Error Handling

Blazor, by default, shows a yellow line at the bottom of the page if any unhandled exception occurs. However, this is not useful in a real application.

ABP provides an [automatic error handling system](error-handling.md) for the Blazor UI.

## Customization

While the theme and some modules come as NuGet packages, you can still replace/override and customize them on need. See the [Customization / Overriding Components](customization-overriding-components.md) document.
