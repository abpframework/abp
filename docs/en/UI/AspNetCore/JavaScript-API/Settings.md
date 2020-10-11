# ASP.NET Core MVC / Razor Pages UI: JavaScript Setting API

Localization API allows you to get the values of the settings on the client side. You can read the current value of a setting in the client side only if it is allowed by the setting definition (on the server side).

> This document only explains the JavaScript API. See the [settings document](../../../Settings.md) to understand the ABP setting system.

## Basic Usage

````js
//Gets a value as string.
var language = abp.setting.get('Abp.Localization.DefaultLanguage');

//Gets an integer value.
var requiredLength = abp.setting.getInt('Abp.Identity.Password.RequiredLength');

//Gets a boolean value.
var requireDigit = abp.setting.getBoolean('Abp.Identity.Password.RequireDigit');
````

## All Values

`abp.setting.values` can be used to obtain all the setting values as an object where the object properties are setting names and property values are the setting values.

An example value of this object is shown below:

````js
{
  Abp.Localization.DefaultLanguage: "en",
  Abp.Timing.TimeZone: "UTC",
  ...
}
````

