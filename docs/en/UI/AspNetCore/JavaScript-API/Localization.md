# ASP.NET Core MVC / Razor Pages UI: JavaScript Localization API

Localization API allows you to reuse the server side localization resources in the client side.

> This document only explains the JavaScript API. See the [localization document](../../../Localization.md) to understand the ABP localization system.

## Basic Usage

`abp.localization.getResource(...)` function is used to get a localization resource:

````js
var testResource = abp.localization.getResource('Test');
````

Then you can localize a string based on this resource:

````js
var str = testResource('HelloWorld');
````

`abp.localization.localize(...)` function is a shortcut where you can both specify the text name and the resource name:

````js
var str = abp.localization.localize('HelloWorld', 'Test');
````

`HelloWorld` is the text to localize, where `Test` is the localization resource name here.

### Default Localization Resource

If you don't specify the localization resource name, it uses the **default localization resource** defined on the `AbpLocalizationOptions` (see the *Default Resource* section above). Example:

````js
var str = abp.localization.localize('HelloWorld'); //uses the default resource
````

### Format Arguments

If your localized string contains arguments, like `Hello {0}, welcome!`, you can pass arguments to the localization methods. Examples:

````js
var str1 = abp.localization.getResource('Test')('HelloWelcomeMessage', 'John');
var str2 = abp.localization.localize('HelloWelcomeMessage', 'Test', 'John');
````

Assuming the `HelloWelcomeMessage` is localized as `Hello {0}, welcome!`, both of the samples above produce the output `Hello John, welcome!`.