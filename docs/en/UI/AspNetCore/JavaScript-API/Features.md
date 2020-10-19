# ASP.NET Core MVC / Razor Pages UI: JavaScript Features API

`abp.features` API allows you to check features or get the values of the features on the client side. You can read the current value of a feature in the client side only if it is allowed by the feature definition (on the server side).

> This document only explains the JavaScript API. See the [Features](../../../Features.md) document to understand the ABP Features system.

## Basic Usage

`abp.features.values` can be used to access to the all feature values.

````js
var excelExportFeatureValue = abp.features.values["ExportingToExcel"];
````

Then you can check the value of the feature to perform your logic.