# ABP Version 7.0 Migration Guide

This document is a guide for upgrading ABP v6.0 solutions to ABP v7.0. There is a change in this version that may affect your applications, please read it carefully and apply the necessary changes to your application.

## Hybrid JSON was removed.

Since [System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview) library supports more custom features in NET 7, ABP no longer need the hybrid Json feature.

### Previous Behavior

There is a `Volo.Abp.Json` package which contains the `AbpJsonModule` module.
`Serialization/deserialization` features of [System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview) and [Nettonsoft](https://www.newtonsoft.com/json/help/html/SerializingJSON.htm) are implemented in this module.

We use [System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview) first,  More custom cases can be handled with [Nettonsoft](https://www.newtonsoft.com/json/help/html/SerializingJSON.htm) by configuring `UnsupportedTypes` of `AbpSystemTextJsonSerializerOptions`.

### New Behavior

We created `Volo.Abp.Json.SystemTextJson` and `Volo.Abp.Json.Newtonsoft` as separate packages, which means you can only use one of them in your project. The default is to use `SystemTextJson`. If you want `Newtonsoft`, please also use `Volo.Abp.AspNetCore.Mvc.NewtonsoftJson` in your web project.

* Volo.Abp.Json.Abstractions 
* Volo.Abp.Json.Newtonsoft
* Volo.Abp.Json.SystemTextJson
* Volo.Abp.Json (Depends on `Volo.Abp.Json.SystemTextJson` by default to prevent breaking)
* Volo.Abp.AspNetCore.Mvc.NewtonsoftJson

The `AbpJsonOptions` now has only two properties, which are

* `InputDateTimeFormats(List<string>)`: Formats of input JSON date, Empty string means default format. You can provide multiple formats to parse the date.
* `OutputDateTimeFormat(string)`: Format of output json date, Null or empty string means default format.

Please remove all `UnsupportedTypes` add custom `Modifiers` to control serialization/deserialization behavior.

Check the docs to see the more info: https://github.com/abpframework/abp/blob/dev/docs/en/JSON.md#configuration

Check the docs to see how to customize a JSON contract: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/custom-contracts

