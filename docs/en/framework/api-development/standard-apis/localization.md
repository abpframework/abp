# Application Localization Endpoint

ABP provides a pre-built and standard endpoint that returns all the [localization](../../fundamentals/localization.md) resources and texts defined in the server.

> If you have started with ABP's startup solution templates and using one of the official UI options, then all these are set up for you and you don't need to know these details. However, if you are building a UI application from scratch, you may want to know this endpoint.

## HTTP API

`/api/abp/application-localization` is the main URL of the HTTP API that returns the localization data as a JSON string. I accepts the following query string parameters:

* `cultureName` (required): A culture code to get the localization data, like `en` or `en-US`.
* `onlyDynamics` (optional, default: `false`): Can be set to `true` to only get the dynamically defined localization resources and texts. If your client-side application shares the same localization resources with the server (like ABP's Blazor and MVC UIs), you can set `onlyDynamics` to `true`.

**Example request:**

````
/api/abp/application-localization?cultureName=en
````

## Script

For [ASP.NET Core MVC (Razor Pages)](../../ui/mvc-razor-pages/overall.md) applications, the same localization data is also available on the JavaScript side. `/Abp/ApplicationLocalizationScript` is the URL of the script that is auto-generated based on the HTTP API above.

**Example request:**

````
/Abp/ApplicationLocalizationScript?cultureName=en
````

See the [JavaScript API document](../../ui/mvc-razor-pages/javascript-api) for the ASP.NET Core UI.

Other UI types provide services native to the related platform. For example, see the [Angular UI localization documentation](../../ui/angular/localization.md) to learn how to use the localization values exposes by this endpoint.

