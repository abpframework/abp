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

### Fallback Logic

If given texts was not localized, localization method returns the given key as the localization result.

### Default Localization Resource

If you don't specify the localization resource name, it uses the **default localization resource** defined on the `AbpLocalizationOptions` (see the [localization document](../../../Localization.md)).

**Example: Using the default localization resource**

````js
var str = abp.localization.localize('HelloWorld'); //uses the default resource
````

### Format Arguments

If your localized string contains arguments, like `Hello {0}, welcome!`, you can pass arguments to the localization methods. Examples:

````js
var testSource = abp.localization.getResource('Test');
var str1 = testSource('HelloWelcomeMessage', 'John');
var str2 = abp.localization.localize('HelloWelcomeMessage', 'Test', 'John');
````

Assuming the `HelloWelcomeMessage` is localized as `Hello {0}, welcome!`, both of the samples above produce the output `Hello John, welcome!`.

## Other Properties & Methods

### abp.localization.values

`abp.localization.values` property stores all the localization resources, keys and their values.

### abp.localization.isLocalized

Returns a boolean indicating that if the given text was localized or not.

**Example**

````js
abp.localization.isLocalized('ProductName', 'MyResource');
````

Returns `true` if the `ProductName` text was localized for the `MyResource` resource. Otherwise, returns `false`. You can leave the resource name empty to use the default localization resource.

### abp.localization.defaultResourceName

`abp.localization.defaultResourceName` can be set to change the default localization resource. You normally don't set this since the ABP Framework automatically sets is based on the server side configuration.

### abp.localization.currentCulture

`abp.localization.currentCulture` returns an object to get information about the **currently selected language**.

An example value of this object is shown below:

````js
{
  "displayName": "English",
  "englishName": "English",
  "threeLetterIsoLanguageName": "eng",
  "twoLetterIsoLanguageName": "en",
  "isRightToLeft": false,
  "cultureName": "en",
  "name": "en",
  "nativeName": "English",
  "dateTimeFormat": {
    "calendarAlgorithmType": "SolarCalendar",
    "dateTimeFormatLong": "dddd, MMMM d, yyyy",
    "shortDatePattern": "M/d/yyyy",
    "fullDateTimePattern": "dddd, MMMM d, yyyy h:mm:ss tt",
    "dateSeparator": "/",
    "shortTimePattern": "h:mm tt",
    "longTimePattern": "h:mm:ss tt"
  }
}
````

### abp.localization.languages

Used to get list of all **available languages** in the application. An example value of this object is shown below:

````js
[
  {
    "cultureName": "en",
    "uiCultureName": "en",
    "displayName": "English",
    "flagIcon": null
  },
  {
    "cultureName": "fr",
    "uiCultureName": "fr",
    "displayName": "Français",
    "flagIcon": null
  },
  {
    "cultureName": "pt-BR",
    "uiCultureName": "pt-BR",
    "displayName": "Português",
    "flagIcon": null
  },
  {
    "cultureName": "tr",
    "uiCultureName": "tr",
    "displayName": "Türkçe",
    "flagIcon": null
  },
  {
    "cultureName": "zh-Hans",
    "uiCultureName": "zh-Hans",
    "displayName": "简体中文",
    "flagIcon": null
  }
]
````

